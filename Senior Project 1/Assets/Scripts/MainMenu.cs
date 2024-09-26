using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool level1complete = false;
    public GameObject mainMenu;
    public GameObject LevelMenu;

    public void StartGame()
    {
        //open 2nd level menu
        LevelMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Level2()
    {
        if (level1complete == true)
        {
            SceneManager.LoadScene("Level 2");
        }
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        LevelMenu.SetActive(false);
    }
}
