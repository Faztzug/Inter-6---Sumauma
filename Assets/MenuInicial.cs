using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class MenuInicial : MonoBehaviour
{
    private Camera mainCamera;
    static public Camera MainCamera { get => menuInicial.mainCamera; }
    private Camera cutsceneCamera;
    private static MenuInicial menuInicial;
    public static MenuInicial MenuInicialInstance => menuInicial;
    private MenuInicial() { }

    [SerializeField] private Book _book;
    public static Book book => menuInicial._book;

    public SaveData saveData;
    public static SaveData SaveData { get => menuInicial.saveData; set => menuInicial.saveData = value; }
    public static SaveManager saveManager = new SaveManager();
    public static Action SettingsUpdated;

    private void Awake()
    {
        mainCamera = Camera.main;

        menuInicial = this;
        SaveData = saveManager.LoadGame();
        Application.targetFrameRate = 60;

        saveData.jumpCutscene = false;
        saveData = saveManager.ResetCheckPointValue(saveData);
        UpdateQuality();
        SettingsUpdated?.Invoke();
        saveManager.SaveGame(saveData);
    }
    private void Start() 
    {

    }

    public static string GetSceneName() => SceneManager.GetActiveScene().name;

    public static void ReloadScene(float waitTime)
    {
        var ob = MenuInicialInstance;
        var sceneName = ob.gameObject.scene.name;
        SaveData.jumpCutscene = true;
        saveManager.SaveGame(SaveData);
        ob.StartCoroutine(ob.LoadSceneCourotine(waitTime, sceneName));
    }

    public static void LoadScene(string sceneName, float waitTime = 0)
    {
        var ob = MenuInicialInstance;
        ob.StartCoroutine(ob.LoadSceneCourotine(waitTime, sceneName));
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
}
