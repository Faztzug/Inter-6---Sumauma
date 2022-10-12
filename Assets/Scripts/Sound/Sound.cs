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

    [HideInInspector]
    public AudioSource audioSource;

}
