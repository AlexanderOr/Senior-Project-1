using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HPManager : MonoBehaviour
{
    public Image health;
    public TMP_Text healthText;
    public PlayerController playerController;
    public Image XP;
    public TMP_Text XPText;

    //spells
    public Image spellIcon1;
    public Image spellIcon2;
    public Image spellIcon3;
    public Image spellIcon4;
    public Image spellIcon5;


   public void SetSpell(Spells spells)
    {
        if (spells != null)
        {
            spellIcon1.sprite = spells.Icon;
            spellIcon2.sprite = spells.Icon;
        }
    }

    void Update()
    {
        health.fillAmount = playerController.Player_HP / 100f;
        XP.fillAmount = playerController.Player_EXP / 100f;
        healthText.text = playerController.Player_HP.ToString() + " / 100";
        XPText.text = playerController.Player_EXP.ToString() + " / 100";
    }

}
