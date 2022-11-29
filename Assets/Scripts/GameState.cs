using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class GameState : MonoBehaviour
{
    static public MainCanvas mainCanvas;
    static public CinemachineFreeLook cinemachineFreeLook;
    public Transform playerTransform;
    static public Transform PlayerTransform => GameStateInstance.playerTransform;
    public bool isPlayerDead = false;
    static public bool IsPlayerDead {
        get => GameStateInstance.isPlayerDead;
        set => GameStateInstance.isPlayerDead = value;
    }
    public static bool isPlayerDashing {get; set;} = false;
    public static bool isGamePaused {get; set;} = false;
    public bool godMode = false;
    static public bool GodMode => GameStateInstance.godMode;
    static public void ToogleGodMode() => GameStateInstance.godMode = !GodMode;
    public Transform playerLookAt;
    static public bool isOnCutscene;
    static public bool hasOpenCutsceneBook;
    static public bool skipCutscene;
    private Camera mainCamera;
    static public Camera MainCamera { get => gameState.mainCamera; }
    private Camera cutsceneCamera;
    private static GameState gameState;
    [SerializeField] private GameObject GenericAudioSourcePrefab;
    private GameState() { }

    public static GameState GameStateInstance => gameState;
    public SaveData saveData;
    public static SaveData SaveData { get => gameState.saveData; set => gameState.saveData = value; }
    public static SaveManager saveManager = new SaveManager();
    public static Action OnSettingsUpdated;
    public static Action OnCutsceneEnd;

    public static bool animalColetadoNaFase;
    public static bool plantaColetadaNaFase;
    private bool jumpBook;

    private void Awake()
    {
        mainCamera = Camera.main;
        var cutSceneGOCam = GameObject.FindGameObjectWithTag("CutsceneCamera");
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
        cinemachineFreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = playerTransform;
        cinemachineFreeLook.LookAt = playerLookAt;
        gameState = this;
        animalColetadoNaFase = false;
        plantaColetadaNaFase = false;
        SaveData = saveManager.LoadGame();

        jumpBook = saveData.jumpCutscene;
        animalColetadoNaFase = SaveData.animalColetadoNaFase;
        plantaColetadaNaFase = SaveData.plantaColetadaNaFase;
        Application.targetFrameRate = 60;
        mainCanvas.ResumeGame();
        Debug.Log("HAS CUTSCENE CAM? " + cutSceneGOCam != null);
        
        if(cutSceneGOCam != null)
        {
            cutsceneCamera = cutSceneGOCam.GetComponent<Camera>();
            var timelineAnim = cutSceneGOCam.GetComponent<PlayableDirector>();
            if(timelineAnim != null && timelineAnim.duration > 1f && !SaveData.jumpCutscene)
            {
                SetMainCamera();
                SetCutsceneCamera();
                Debug.Log("duration " + timelineAnim.duration);
                StartCoroutine(EndCutsceneOnTime((float)timelineAnim.duration));
            }
            else
            {
                Debug.Log("empty clip");
                SetCutsceneCamera();
                SetMainCamera();
                StartCoroutine(EndCutsceneOnTime(1f));
            }
        }
        else 
        {
            Debug.Log("CAM IS NULL");
            StartCoroutine(EndCutsceneOnTime(1f));
        }

        saveData.jumpCutscene = false;
        mainCanvas.GetColectableImages();
        UpdateQuality();
        OnSettingsUpdated?.Invoke();
        saveManager.SaveGame(saveData);
    }
    private void Start() 
    {
        var checkpoint = 
        new Vector3(saveData.checkpointPosition[0], saveData.checkpointPosition[1], saveData.checkpointPosition[2]);
        if(checkpoint != Vector3.zero)
        {
            playerTransform.GetComponent<Movimento>().GoToCheckPoint(checkpoint);
        } 
    }

    private void Update()
    {
        if(isOnCutscene && Input.GetButtonDown("Pause"))
        {
            StartCoroutine(EndCutsceneOnTime(0f));
            StartCoroutine(StartJumpBook());
        }
    }

    public static void RestartStage()
    {
        SaveData.jumpCutscene = false;
        saveManager.ResetCheckPointValue(SaveData);
        saveManager.SaveGame(SaveData);
        ReloadScene(0f, false);
    }

    public static bool KeyItemAlreadyColected(ColectableType type)
    {
        if (type == ColectableType.Animal) return animalColetadoNaFase;
        else if (type == ColectableType.Planta) return plantaColetadaNaFase;
        else return false;
    }

    public static string GetSceneName() => SceneManager.GetActiveScene().name;

    public static void ReloadScene(float waitTime, bool jumpCutscene = true)
    {
        var ob = GameStateInstance;
        var sceneName = ob.gameObject.scene.name;
        SaveData.jumpCutscene = jumpCutscene;
        saveManager.SaveGame(SaveData);
        ob.StartCoroutine(ob.LoadSceneCourotine(waitTime, sceneName));
    }

    public static void LoadScene(string sceneName, float waitTime = 0)
    {
        var ob = GameStateInstance;
        ob.StartCoroutine(ob.LoadSceneCourotine(waitTime, sceneName));
    }

    public static void SetCutsceneCamera()
    {
        gameState.mainCamera.gameObject.SetActive(false);
        gameState.cutsceneCamera?.gameObject.SetActive(true);
        isOnCutscene = true;
    }

    public static void SetMainCamera()
    {
        Debug.Log("Set Main Camera");
        gameState.cutsceneCamera?.gameObject.SetActive(false);
        gameState.mainCamera.gameObject.SetActive(true);
        isOnCutscene = false;
        cinemachineFreeLook.m_YAxisRecentering.RecenterNow();
        cinemachineFreeLook.m_RecenterToTargetHeading.RecenterNow();
        cinemachineFreeLook.m_XAxis.m_Recentering.RecenterNow();
        cinemachineFreeLook.m_YAxis.m_Recentering.RecenterNow();
    }

    public static void SetCheckPoint(Vector3 position)
    {
        SaveData.checkpointPosition = new float[3]{position.x, position.y, position.z};
        saveManager.SaveGame(SaveData);
        Debug.Log(string.Join(", ", SaveData.checkpointPosition));
    }

    public static void UpdateQuality()
    {
        if(QualitySettings.GetQualityLevel() != (int)SaveData.quality)
        {
            QualitySettings.SetQualityLevel((int)SaveData.quality);
        }
    }

    IEnumerator LoadSceneCourotine(float waitTime, string sceneName)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator EndCutsceneOnTime(float waitTime)
    {
        mainCanvas.ResumeGame();
        yield return new WaitForSeconds(waitTime);
        Debug.Log("End Cutscene On Time");
        SetMainCamera();
        mainCanvas.warningAnim.SetActive(true);
        OnCutsceneEnd?.Invoke();
        if (jumpBook) yield break;
        mainCanvas.PauseGame();
        yield return new WaitForSecondsRealtime(0.1f);
        var historyPage = 0;
        if(GetSceneName() == "Fase 1") historyPage = 4;
        if(GetSceneName() == "Fase 2") historyPage = 10;
        if(GetSceneName() == "Fase 3") historyPage = 12;

        mainCanvas.book.FlipToPage(historyPage);

        hasOpenCutsceneBook = true;
    }
    IEnumerator StartJumpBook()
    {
        yield return new WaitForSecondsRealtime(2f);
        jumpBook = true;
    }

    public static void InstantiateSound(Sound sound, Vector3 position, float destroyTime = 10f)
    {
        var AudioObject = GameObject.Instantiate(gameState.GenericAudioSourcePrefab, position, Quaternion.identity);
        var audioSource = AudioObject.GetComponent<AudioSource>();
        sound.PlayOn(audioSource);
        Destroy(AudioObject, destroyTime);
    }

    public static void ItemColected(Colectables item, ColectableType itemType)
    {
        switch (itemType)
        {
            case ColectableType.Animal:
            animalColetadoNaFase = true;
            break;
            case ColectableType.Planta:
            plantaColetadaNaFase = true;
            break;
        }
        switch (item)
        {
            case Colectables.Heliconia:
            SaveData.heliconiaColetada = true;
            break;
            case Colectables.Onca:
            SaveData.oncaColetada = true;
            break;
            case Colectables.Planta2:
            SaveData.planta2 = true;
            break;
            case Colectables.Animal2:
            SaveData.animal2 = true;
            break;
            case Colectables.Planta3:
            SaveData.planta3 = true;
            break;
            case Colectables.Animal3:
            SaveData.animal3 = true;
            break;
        }
        saveManager.SaveGame(SaveData);
        mainCanvas.GetColectableImages();
        gameState.CheckEndStage();
    }

    public static bool GetColectableSaveState(Colectables item)
    {
        switch (item)
        {
            case Colectables.Heliconia:
            return SaveData.heliconiaColetada;

            case Colectables.Onca:
            return SaveData.oncaColetada;


            case Colectables.Planta2:
            return SaveData.planta2;

            case Colectables.Animal2:
            return SaveData.animal2;

            
            case Colectables.Planta3:
            return SaveData.planta3;

            case Colectables.Animal3:
            return SaveData.animal3;
        }
        return false;
    }

    public void CheckEndStage(bool force = false)
    {
        SaveData.animalColetadoNaFase = animalColetadoNaFase;
        SaveData.plantaColetadaNaFase = plantaColetadaNaFase;

        if(!animalColetadoNaFase || !plantaColetadaNaFase) return;
        SaveData = saveManager.ResetCheckPointValue(SaveData);

        if (GetSceneName() == "Fase 1")
        {
            if(saveData.unlockLevelsTo < 2)
            {
                saveData.unlockLevelsTo = 2;
                saveManager.SaveGame(saveData);
            }
            LoadScene("Fase 2");
        }
        if (GetSceneName() == "Fase 2")
        {
            if(saveData.unlockLevelsTo < 3)
            {
                saveData.unlockLevelsTo = 3;
                saveManager.SaveGame(saveData);
            }
            LoadScene("Fase 3");
        }
        if (GetSceneName() == "Fase 3" && force)
        {
            if(saveData.unlockLevelsTo < 4)
            {
                saveData.unlockLevelsTo = 4;
                saveManager.SaveGame(saveData);
            }
            //LoadScene("Menu inicial");
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        mainCanvas.PauseGame();
        yield return new WaitForSecondsRealtime(1f);
        mainCanvas.book.FlipToPage(20);
        yield return new WaitForSecondsRealtime(7f);
        LoadScene("Menu inicial");
    }
}
