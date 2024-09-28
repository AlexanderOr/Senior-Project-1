using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMenuManager : MonoBehaviour
{

    public GameObject SpellMenu;
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SpellMenu.SetActive(true);
            playerController.isPaused = true;
        }
    }

    public void Confirm()
    {
        SpellMenu.SetActive(false);
        playerController.isPaused = false;
        //equip spell to player
    }
}
