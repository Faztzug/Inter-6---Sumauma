using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolim : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            var moveScript =  other.gameObject.GetComponent<Movimento>();
            moveScript.ImpulseJump(force);
        }
    }
}
