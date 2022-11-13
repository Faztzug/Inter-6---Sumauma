using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

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
    static public bool onCutscene;
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

    public static bool animalColetadoNaFase;
    public static bool plantaColetadaNaFase;

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
        Application.targetFrameRate = 60;
        if(cutSceneGOCam != null)
        {
            cutsceneCamera = cutSceneGOCam.GetComponent<Camera>();
            var cutSceneAnim = cutSceneGOCam.GetComponent<Animator>();
            if(cutSceneAnim != null 
            && cutSceneAnim.runtimeAnimatorController != null
            && cutSceneAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length > 1)
            {
                SetMainCamera();
                SetCutsceneCamera();
            }
            else
            {
                SetCutsceneCamera();
                SetMainCamera();
            }
        }
        mainCanvas.ResumeGame();
        mainCanvas.GetColectableImages();
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

    public void CheckEndStage()
    {
        if(!animalColetadoNaFase || !plantaColetadaNaFase) return;

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
        if (GetSceneName() == "Fase 3")
        {
            if(saveData.unlockLevelsTo < 4)
            {
                saveData.unlockLevelsTo = 4;
                saveManager.SaveGame(saveData);
            }
            LoadScene("Menu inicial");
        }
    }

    public static string GetSceneName() => SceneManager.GetActiveScene().name;

    public static void ReloadScene(float waitTime)
    {
        var ob = GameStateInstance;
        var sceneName = ob.gameObject.scene.name;
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
        onCutscene = true;
    }

    public static void SetMainCamera()
    {
        gameState.cutsceneCamera?.gameObject.SetActive(false);
        gameState.mainCamera.gameObject.SetActive(true);
        onCutscene = false;
    }

    IEnumerator LoadSceneCourotine(float waitTime, string sceneName)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        SceneManager.LoadScene(sceneName);
    }

    public static void InstantiateSound(Sound sound, Vector3 position, float destroyTime = 10f)
    {
        var AudioObject = GameObject.Instantiate(gameState.GenericAudioSourcePrefab, position, Quaternion.identity);
        var audioSource = AudioObject.GetComponent<AudioSource>();
        sound.PlayOn(audioSource);
        Destroy(AudioObject, destroyTime);
    }
}
