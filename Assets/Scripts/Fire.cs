using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float damageBySec;
    private PlayerHealth health;

    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(health == null) health = other.gameObject.GetComponent<PlayerHealth>();
            health.UpdateHealth(damageBySec * Time.fixedDeltaTime);
        }
    }

    private void OnValidate() 
    {
        if(damageBySec > 0) damageBySec = -damageBySec;
    }
}
