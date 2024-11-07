using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject RunningEnemy;
    public GameObject RangedEnemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

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
            Instantiate(Enemy, transform.position, transform.rotation);
        }

        if (stopSpawning )
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}
