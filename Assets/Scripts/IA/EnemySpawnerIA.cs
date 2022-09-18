using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerIA : EnemyIA
{
    [SerializeField] protected float touchDamage = -15;
    private PlayerHealth health;
    [SerializeField] private GameObject[] enemysPrefabs;
    [SerializeField] private List<EnemyIA> enemysList;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemys;

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

        enemysList.RemoveAll(e => e.alive == false);


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
