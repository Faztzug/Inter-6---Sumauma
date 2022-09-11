using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiky : MonoBehaviour
{
    [SerializeField] private float damageByTouch;
    private PlayerHealth health;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(health == null) health = other.transform.GetComponent<PlayerHealth>();
            health.UpdateHealth(damageByTouch);
        }
    }

    private void OnValidate() 
    {
        if(damageByTouch > 0) damageByTouch = -damageByTouch;
    }
}
