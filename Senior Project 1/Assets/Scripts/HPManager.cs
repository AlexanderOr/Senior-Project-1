using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPManager : MonoBehaviour
{
    public Image health;
    public TMP_Text healthText;
    public PlayerController playerController;
    public Image XP;



    void Start()
    {

    }

    void Update()
    {
        health.fillAmount = playerController.Player_HP / 100f;
        XP.fillAmount = playerController.Player_EXP / 100f;
        healthText.text = playerController.Player_HP.ToString();
    }

}
