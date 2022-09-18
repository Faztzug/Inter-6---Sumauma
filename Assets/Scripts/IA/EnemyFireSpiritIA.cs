using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireSpiritIA : EnemyIA
{
    [SerializeField] protected float touchDamage;
    private PlayerHealth health;
    protected override void AsyncUpdateIA()
    {
        base.AsyncUpdateIA();
        FollowPlayer();
    }

    protected virtual void FollowPlayer()
    {
        playerPos = player.position;
        playerPos.y = 0;

        pos = transform.position;
        pos.y = 0;
        
        distance = Vector3.Distance(pos, playerPos);


        if(agent.isOnNavMesh)
        {
            if(distance > minPlayerDistance && distance < findPlayerDistance)
            {
                playerPos = player.position;
                agent.SetDestination(playerPos);
            }
            else 
            {
                pos = transform.position;
                agent.SetDestination(pos);
                rgbd.velocity = Vector3.zero;
                rgbd.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " OUT OF NAV MESH!");
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player"))
        {
            if(health == null) health = collisionInfo.gameObject.GetComponent<PlayerHealth>();
            if(health != null) health.UpdateHealth(touchDamage);
            else Debug.Log("Player Health Null?");
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if(touchDamage > 0) touchDamage = -touchDamage;
    }
}
