using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformParent : MonoBehaviour
{
    private Transform player;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            other.transform.parent = null;
            player = null;
        }
    }

    public void FreePlayer() 
    {
        if(player != null) player.transform.parent = null;
    }
}
