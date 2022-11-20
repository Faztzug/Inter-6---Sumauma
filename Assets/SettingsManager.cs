using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle mute;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private Toggle fps;


    private void Start() 
    {
        var data = GameState.SaveData;
        mute.isOn = data.mute;
        music.value = data.musicVolume;
        sfx.value = data.sfxVolume;
        quality.value = (int)data.quality;
        fps.isOn = data.showFPS;

        GameState.SettingsUpdated += SettingsHasUpdated;
    }
    public void UpdateSave()
    {
        GameState.saveManager.SaveGame(GameState.SaveData);
        GameState.SettingsUpdated?.Invoke();
    }
    public void MuteChanged(bool value)
    {
        GameState.SaveData.mute = value;
    }
    public void MusicChanged(float value)
    {
        GameState.SaveData.musicVolume = value;
    }
    public void SFXChanged(float value)
    {
        GameState.SaveData.sfxVolume = value;
    }
    public void QualityChanged(int value)
    {
        GameState.SaveData.quality = (Quality)value;
    }
    public void FPSChanged(bool value)
    {
        GameState.SaveData.showFPS = value;
    }

    private void SettingsHasUpdated()
    {
        GameState.UpdateQuality();
        Debug.Log("SETTINGS UPDATE");
    }

    private void OnDestroy() 
    {
        GameState.SettingsUpdated -= SettingsHasUpdated;
    }
}
