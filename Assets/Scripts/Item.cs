using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{ 
    [SerializeField] protected int ammount;
    [SerializeField] protected Sound collectSound;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) CollectItem(other);
    }

    public virtual void CollectItem(Collider info)
    {
        
    }

    public virtual void DestroyItem()
    {
        GameState.InstantiateSound(collectSound, transform.position);
        this.gameObject.SetActive(false);
    }
}
