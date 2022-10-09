using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireGenericIA : EnemyIA
{
    [SerializeField] protected float touchDamage = -10;
    protected PlayerHealth health;
    protected Movimento playerMove;

    protected virtual void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player"))
        {
            if(health == null) health = collisionInfo.gameObject.GetComponent<PlayerHealth>();
            if(playerMove == null) playerMove = collisionInfo.gameObject.GetComponent<Movimento>();
            if(health != null) 
            {   
                if(playerMove.onKnockBack) return;
                health.UpdateHealth(touchDamage);
                playerMove.KnockBack(this.transform.position);
            }
            else Debug.Log("Player Health Null?");
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if(touchDamage > 0) touchDamage = -touchDamage;
    }
}
