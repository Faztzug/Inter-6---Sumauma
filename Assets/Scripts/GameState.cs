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

    private void Awake()
    {
        mainCamera = Camera.main;
        var cutSceneGOCam = GameObject.FindGameObjectWithTag("CutsceneCamera");
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
        cinemachineFreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = playerTransform;
        cinemachineFreeLook.LookAt = playerLookAt;
        gameState = this;
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
    }

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
