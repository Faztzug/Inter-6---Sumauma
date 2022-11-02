using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(Collider))]
public class Spiky : MonoBehaviour
{
    public float damageByTouch;
    [SerializeField] private bool damageOnTrigger;
    private PlayerHealth health;
    protected Movimento playerMove;

    private void OnCollisionEnter(Collision other)
    {
        DoDamage(other.gameObject);
    }

    private void OnCollisionStay(Collision other) 
    {
        DoDamage(other.gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(damageOnTrigger) DoDamage(other.gameObject);
    }

    private void DoDamage(GameObject other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(health == null) health = other.transform.GetComponent<PlayerHealth>();
            if(playerMove == null) playerMove = other.gameObject.GetComponent<Movimento>();
            if(playerMove?.onKnockBack == true) return;
            health?.UpdateHealth(damageByTouch);
            playerMove?.KnockBack(this.transform.position);
        }
    }

    private void OnValidate() 
    {
        if(damageByTouch > 0) damageByTouch = -damageByTouch;
    }
}
