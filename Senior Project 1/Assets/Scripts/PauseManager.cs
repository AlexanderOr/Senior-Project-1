using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject PauseMenu;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            playerController.isPaused = true;
        }
;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        playerController.isPaused = false;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        playerController.isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
