using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private bool isInGame;

    [Header("Interactables")]
    [SerializeField] private Toggle mute;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private Toggle fps;
    private SaveManager saveManager = new SaveManager();


    private void Start() 
    {
        var data = isInGame ? GameState.SaveData : MenuInicial.SaveData;
        mute.isOn = data.mute;
        music.value = data.musicVolume;
        sfx.value = data.sfxVolume;
        quality.value = (int)data.quality;
        fps.isOn = data.showFPS;

        if(isInGame) GameState.OnSettingsUpdated += SettingsHasUpdated;
        else MenuInicial.SettingsUpdated += SettingsHasUpdated;
    }
    public void UpdateSave()
    {
        saveManager.SaveGame(GetSaveData());
        if(isInGame) GameState.OnSettingsUpdated?.Invoke();
        else MenuInicial.SettingsUpdated?.Invoke();
    }
    public void MuteChanged(bool value)
    {
        GetSaveData().mute = value;
    }
    public void MusicChanged(float value)
    {
        GetSaveData().musicVolume = value;
    }
    public void SFXChanged(float value)
    {
        GetSaveData().sfxVolume = value;
    }
    public void QualityChanged(int value)
    {
        GetSaveData().quality = (Quality)value;
    }
    public void FPSChanged(bool value)
    {
        GetSaveData().showFPS = value;
    }

    private SaveData GetSaveData()
    {
        if(isInGame) return GameState.SaveData;
        else return MenuInicial.SaveData;
    }

    private void SettingsHasUpdated()
    {
        if(isInGame) GameState.UpdateQuality();
        else MenuInicial.UpdateQuality();
        Debug.Log("SETTINGS UPDATE");
    }

    private void OnDestroy() 
    {
        if(isInGame) GameState.OnSettingsUpdated -= SettingsHasUpdated;
        else MenuInicial.SettingsUpdated -= SettingsHasUpdated;
    }
}
