using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject RunningEnemy;
    public GameObject RangedEnemy;
    public GameObject BossEnemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public float spawnRadius = 20f;

    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
    }

    public void SpawnEnemy()
    {
        if(playerController.isPaused == false)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;  // Random direction around the player
            Vector3 spawnPosition = playerController.transform.position + (Vector3)randomDirection * spawnRadius;

            Instantiate(Enemy, spawnPosition, Quaternion.identity);
        }

        if (stopSpawning )
        {
            CancelInvoke("SpawnEnemy");
        }
    }

    public void SpawnBoss()
    {
        //spawn boss
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = playerController.transform.position + (Vector3)randomDirection * spawnRadius;

        Instantiate(BossEnemy, spawnPosition, Quaternion.identity);
        stopSpawning = true;
    }
}
