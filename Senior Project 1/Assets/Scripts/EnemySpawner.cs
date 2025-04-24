using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject RunningEnemy;
    public GameObject RangedEnemy;
    public GameObject BossEnemy;
    public GameObject RandomEnemyObject;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public float spawnRadius = 20f;
    public int randomEnemy;
    

    public PlayerController playerController;
    public GameTimer gameTimer;

    public Vector2 arenaMax;
    public Vector2 arenaMin;

    int maxAttempts = 10;
    int attempts = 0;
    bool validPosition = false;
    public Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
    }

    public void SpawnEnemy()
    {
        if (gameTimer.Timer > 60f)
        {
            Debug.Log(gameTimer.Timer);
            randomEnemy = Random.Range(1, 3);
        }
        else
        {
            randomEnemy = 1;
        }
        

        if (randomEnemy == 1)
        {
            RandomEnemyObject = Enemy;
        }
        else if (randomEnemy == 2) 
        {
            RandomEnemyObject = RangedEnemy;
        }

        if(playerController.isPaused == false)
        {
            do
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;  // Random direction around the player
                spawnPosition = playerController.transform.position + (Vector3)randomDirection * spawnRadius;

                if ((spawnPosition.x >= arenaMin.x && spawnPosition.x <= arenaMax.x) && 
                    (spawnPosition.y >= arenaMin.y && spawnPosition.y <= arenaMax.y))
                {
                    validPosition = true;
                }

                attempts++;

            }
            while (!validPosition && attempts < maxAttempts);

            if(validPosition)
            {
                Instantiate(RandomEnemyObject, spawnPosition, Quaternion.identity);
            }
        }

        if (stopSpawning == true)
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
