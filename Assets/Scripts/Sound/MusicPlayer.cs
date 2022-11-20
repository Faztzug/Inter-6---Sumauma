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
        
        GameState.SettingsUpdated += UpdateVolume;
    }

    public void UpdateVolume()
    {
        audioSource.volume = GameState.SaveData.mute ? 0f : musicSound.volume * GameState.SaveData.musicVolume;
    }

    private void OnDestroy() 
    {
        GameState.SettingsUpdated -= UpdateVolume;
    }
}
