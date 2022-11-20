using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireSpiritIA : EnemyFireGenericIA
{
    protected override void AsyncUpdateIA()
    {
        base.AsyncUpdateIA();
        transform.GetChild(0).localPosition = Vector3.zero;
        FollowPlayer();
    }

    protected virtual void FollowPlayer()
    {
        playerPos = player.position;
        playerPos.y = 0;

        pos = transform.position;
        pos.y = 0;
        
        distance = Vector3.Distance(pos, playerPos);

        if(!agent.enabled) return;
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
            health?.UpdateHealth(-0.5f);
        }
    }
}
