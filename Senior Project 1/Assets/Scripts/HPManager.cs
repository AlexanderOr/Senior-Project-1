using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HPManager : MonoBehaviour
{
    public SpellHolder spellHolder;
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

    public GameObject SpellBorder1;
    public GameObject SpellBorder2;
    public GameObject SpellBorder3;
    public GameObject SpellBorder4;
    public GameObject SpellBorder5;


    public void SetSpell(Spells spells)
    {
        if (spells != null)
        {
            if (spellHolder.i == 0)
            {
                spellIcon1.sprite = spells.Icon;
            }
            else if (spellHolder.i == 1)
            {
                spellIcon2.sprite = spells.Icon;
            }
            else if (spellHolder.i == 2)
            {
                spellIcon3.sprite = spells.Icon;
            }
            else if (spellHolder.i == 3)
            {
                spellIcon4.sprite = spells.Icon;
            }
            else if (spellHolder.i == 4)
            {
                spellIcon5.sprite = spells.Icon;
            }

        }
    }

    void Update()
    {
        health.fillAmount = playerController.Player_HP / 100f;
        XP.fillAmount = playerController.Player_EXP / 100f;
        healthText.text = playerController.Player_HP.ToString() + " / 100";
        XPText.text = playerController.Player_EXP.ToString() + " / 100";

        switch (playerController.currentSpellIndex)
        {
            case 0: 
                SpellBorder1.SetActive(true); 
                SpellBorder2.SetActive(false);
                SpellBorder3.SetActive(false);
                SpellBorder4.SetActive(false);
                SpellBorder5.SetActive(false);
                break;
            case 1:
                SpellBorder1.SetActive(false);
                SpellBorder2.SetActive(true);
                SpellBorder3.SetActive(false);
                SpellBorder4.SetActive(false);
                SpellBorder5.SetActive(false);
                break;
            case 2:
                SpellBorder1.SetActive(false);
                SpellBorder2.SetActive(false);
                SpellBorder3.SetActive(true);
                SpellBorder4.SetActive(false);
                SpellBorder5.SetActive(false);
                break;
            case 3:
                SpellBorder1.SetActive(false);
                SpellBorder2.SetActive(false);
                SpellBorder3.SetActive(false);
                SpellBorder4.SetActive(true);
                SpellBorder5.SetActive(false);
                break;
            case 4:
                SpellBorder1.SetActive(false);
                SpellBorder2.SetActive(false);
                SpellBorder3.SetActive(false);
                SpellBorder4.SetActive(false);
                SpellBorder5.SetActive(true);
                break;
            default:
                SpellBorder1.SetActive(true);
                SpellBorder2.SetActive(false);
                SpellBorder3.SetActive(false);
                SpellBorder4.SetActive(false);
                SpellBorder5.SetActive(false);
                break;

        }
        
    }

}
