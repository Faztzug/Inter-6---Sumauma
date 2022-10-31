using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(-3, 3)]
    public float pitch = 1f;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource audioSource;
    
    public void Setup(AudioSource audioSource)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.playOnAwake = playOnAwake;
    }

    public void PlayOn(AudioSource audioSource)
    {
        Setup(audioSource);
        audioSource.PlayOneShot(clip);
    }

}
