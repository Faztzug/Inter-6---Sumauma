using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(Collider))]
public class Spiky : MonoBehaviour
{
    [SerializeField] private float damageByTouch;
    private PlayerHealth health;
    protected Movimento playerMove;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(health == null) health = other.transform.GetComponent<PlayerHealth>();
            if(playerMove == null) playerMove = other.gameObject.GetComponent<Movimento>();
            health?.UpdateHealth(damageByTouch);
            playerMove?.KnockBack(this.transform.position);
        }
    }

    private void OnValidate() 
    {
        if(damageByTouch > 0) damageByTouch = -damageByTouch;
    }
}
