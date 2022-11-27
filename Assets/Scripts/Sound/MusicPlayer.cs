using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private Sound musicSound;
    private AudioClip CurrentPlaying;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicSound.Setup(audioSource);

        UpdateVolume();

        audioSource.Play();
        CurrentPlaying = musicSound.clip;
        
        GameState.OnSettingsUpdated += UpdateVolume;
    }

    public void UpdateVolume()
    {
        if(GameState.GameStateInstance != null)
        {
            audioSource.volume = GameState.SaveData.mute ? 0f : musicSound.volume * GameState.SaveData.musicVolume;
        }
        else if(MenuInicial.MenuInicialInstance != null)
        {
            audioSource.volume = MenuInicial.SaveData.mute ? 0f : musicSound.volume * MenuInicial.SaveData.musicVolume;
        }
    }

    private void OnDestroy() 
    {
        GameState.OnSettingsUpdated -= UpdateVolume;
    }
}
