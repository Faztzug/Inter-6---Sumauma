using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolim : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] Sound boingSound;
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            var moveScript =  other.gameObject.GetComponent<Movimento>();
            moveScript.ImpulseJump(force);
            boingSound.PlayOn(audioSource);
            moveScript.podeDoubleJump = true;
        }
    }
}
