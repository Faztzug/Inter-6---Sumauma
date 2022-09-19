using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{ 
    [SerializeField] protected int ammount;
    [SerializeField] protected AudioClip clip;
    protected AudioSource audioSource;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) CollectItem(other);
    }

    public virtual void CollectItem(Collider info)
    {
        // if(info.TryGetComponent<AudioSource>(out AudioSource audio))
        // {
        //     audioSource = audio;
        // }
    }

    public virtual void DestroyItem()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        this.gameObject.SetActive(false);
    }
}
