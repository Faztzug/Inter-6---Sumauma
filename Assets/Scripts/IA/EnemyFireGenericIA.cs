using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireGenericIA : EnemyIA
{
    [SerializeField] protected float touchDamage = -10;
    [SerializeField] protected float knockBackForce = 10;
    protected PlayerHealth health;
    protected Movimento playerMove;
    protected bool grounded = true;
    [SerializeField] private float knockBackTime = 0.5f;
    private float knockBackCounterTime;
    public bool onKnockBack => knockBackCounterTime > 0;

    protected virtual void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player"))
        {
            if(health == null) health = collisionInfo.gameObject.GetComponent<PlayerHealth>();
            if(playerMove == null) playerMove = collisionInfo.gameObject.GetComponent<Movimento>();
            if(health != null) 
            {   
                if(playerMove.onKnockBack || GameState.IsPlayerDead) return;
                health.UpdateHealth(touchDamage);
                playerMove.KnockBack(this.transform.position);
            }
            else Debug.Log("Player Health Null?");
        }

        
        if(collisionInfo.gameObject.layer == 0)
        {
            grounded = true;
        }
    }

    protected override void Update()
    {
        knockBackCounterTime -= Time.deltaTime;
        if(knockBackCounterTime <= 0 && grounded)
        {
            agent.enabled = true;
        } 
        else
        {
            var vel = rgbd.velocity;
            vel.y -= 50f * Time.deltaTime;
            rgbd.velocity = vel;
        }
        base.Update();
    }
    public void KnockBack(Vector3 maksPos)
    {
        agent.enabled = false;
        grounded = false;
        Debug.Log("KNOCK BACK");
        var vectorDistance = this.transform.position - maksPos;
        var knockBackDir = vectorDistance.normalized;
        knockBackDir.y = 0.8f;
        rgbd.AddForce(knockBackDir * knockBackForce, ForceMode.Impulse);
        knockBackCounterTime = knockBackTime;
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if(touchDamage > 0) touchDamage = -touchDamage;
    }
}
