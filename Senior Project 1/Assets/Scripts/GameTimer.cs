using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float Timer = 0;
    public float endTime; // 10 mins (60 seconds x 10)
    public bool bossSpawned = false;
    public TMP_Text timerText;
    public PlayerController playerController;

    public EnemySpawner enemySpawner;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.isPaused == false)
        {
            Timer += Time.deltaTime;
            UpdateTimerText();

        }

        if (Timer >= endTime && bossSpawned == false) 
        {
            //spawn boss
            enemySpawner.SpawnBoss();
            enemySpawner.stopSpawning = true;
            timerText.gameObject.SetActive(false);
            bossSpawned=true;
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(Timer / 60);  // Calculate minutes
        int seconds = Mathf.FloorToInt(Timer % 60);  // Calculate seconds

        // Update TMP text with formatted time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
