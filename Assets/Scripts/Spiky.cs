using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(Collider))]
public class Spiky : MonoBehaviour
{
    public float damageByTouch;
    [SerializeField] private bool damageOnTrigger;
    [SerializeField] private bool resetDoubleJumpOnTouch;
    [SerializeField] private bool knockBackOnlyUp;
    private PlayerHealth health;
    protected Movimento playerMove;
    private float DamageCoolDown = 0.5f;

    private void Update() 
    {
        DamageCoolDown -= Time.deltaTime;
    }

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
            if(playerMove != null && playerMove.onKnockBack == true || DamageCoolDown > 0) return;
            Debug.Log("SPIKY");
            DamageCoolDown = 0.5f;
            health?.UpdateHealth(damageByTouch);
            var knockPos = knockBackOnlyUp ? GameState.PlayerTransform.position : this.transform.position;
            if(resetDoubleJumpOnTouch && playerMove != null) 
            {
                playerMove?.KnockBack(knockPos, 1f);
                playerMove.podeDoubleJump = true;
                playerMove?.ResetKnockBackTimer();
            }
            else playerMove?.KnockBack(knockPos);
        }
    }

    private void OnValidate() 
    {
        if(damageByTouch > 0) damageByTouch = -damageByTouch;
    }
}
