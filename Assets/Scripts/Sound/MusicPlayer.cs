using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private string playThis;
    private string CurrentPlaying;
    [Range(0, 1)]
    public float overallVolume = 1;
    private AudioSource[] allAudioSources;

    private void Awake()
    {
        foreach (MusicPlayer music in FindObjectsOfType<MusicPlayer>())
        {
            Debug.Log(gameObject.name + " Trying to change music...");
            music.ChangeMusic(playThis);
        }

        DontDestroy();
    }


    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;

        }

        PlayAudio(playThis);

        allAudioSources = GetComponents<AudioSource>();

        UpdateVolume();
    }

    public void ChangeMusic(string name)
    {
        if (CurrentPlaying != name && CurrentPlaying != null)
        {
            foreach (Sound sound in sounds)
            {
                sound.audioSource.Stop();

            }

            Debug.Log("Musica: " + CurrentPlaying + "alterada para: " + name);
            PlayAudio(name);
        }
    }

    public void PlayAudio(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.audioSource.Play();
                CurrentPlaying = sound.name;
            }

        }
    }

    public void UpdateVolume()
    {
        StartCoroutine(UpdateVolumeCourotine());
    }

    IEnumerator UpdateVolumeCourotine()
    {
        yield return new WaitForEndOfFrame();

        int length = allAudioSources.Length;
        Debug.Log("Update Volume");

        for (int i = 0; i < length; i++)
        {
            allAudioSources[i].volume = sounds[i].volume * overallVolume;
        }
    }

    private void DontDestroy()
    {
        for (int i = 0; i < Object.FindObjectsOfType<MusicPlayer>().Length; i++)
        {
            if (Object.FindObjectsOfType<MusicPlayer>()[i] != this)
            {
                if (Object.FindObjectsOfType<MusicPlayer>()[i].gameObject.name == gameObject.name)
                {
                    //DestroyImmediate(this.gameObject);
                    gameObject.SetActive(false);
                    Destroy(this.gameObject);
                }

            }
        }


        DontDestroyOnLoad(gameObject);
    }
}
