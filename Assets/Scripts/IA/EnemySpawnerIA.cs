using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerIA : EnemyFireGenericIA
{
    [SerializeField] private GameObject[] enemysPrefabs;
    [SerializeField] private List<EnemyIA> enemysList;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemys;
    [SerializeField] private EnemyDrop drops;

    protected override void Start()
    {
        base.Start();

        playerPos = player.position;
        playerPos.y = 0;

        pos = transform.position;
        pos.y = 0;
        
        distance = Vector3.Distance(pos, playerPos);
    }

    protected override void AsyncUpdateIA()
    {
        base.AsyncUpdateIA();

        playerPos = player.position;
        playerPos.y = 0;

        pos = transform.position;
        pos.y = 0;
        
        distance = Vector3.Distance(pos, playerPos);

        enemysList.RemoveAll(e => e == null || e.alive == false);


        if(maxEnemys >= enemysList.Count && distance < findPlayerDistance)
        {
            int iRngPrefab = Random.Range(0, enemysPrefabs.Length);
            int iRngPos = Random.Range(0, spawnPoints.Length);

            var enemy = Instantiate(enemysPrefabs[iRngPrefab], spawnPoints[iRngPos].position, transform.rotation).GetComponent<EnemyIA>();
            enemy.ForceUpdateIA();
            enemysList.Add(enemy);
        }
    }

    protected override void Update()
    {
        base.Update();
        RotateTowardsPlayer();
    }

    protected virtual void RotateTowardsPlayer()
    {
        if(distance > findPlayerDistance) return;

        //var smoothness = 3f;
        var currentRotation = transform.eulerAngles;
        transform.LookAt(player);
        var desiredRot = transform.eulerAngles;

        //currentRotation.y = Mathf.Lerp (currentRotation.y, desiredRot.y, Time.deltaTime * smoothness);
        currentRotation.y = desiredRot.y;
        transform.eulerAngles = currentRotation;
    }

    public override void EnemyDeath()
    {
        drops.Drop();
        Debug.Log("Spawner is dead");
        base.EnemyDeath();
    }
}
