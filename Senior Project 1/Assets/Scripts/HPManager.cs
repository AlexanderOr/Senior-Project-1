using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPManager : MonoBehaviour
{
    public SpellHolder spellHolder;
    public Image health;
    public TMP_Text healthText;
    public PlayerController playerController;
    public Image XP;
    public TMP_Text XPText;
    public TMP_Text LevelText;

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

    public TMP_Text spellLevel1;
    public TMP_Text spellLevel2;
    public TMP_Text spellLevel3;
    public TMP_Text spellLevel4;
    public TMP_Text spellLevel5;

    public int spellLevel1int;
    public int spellLevel2int;
    public int spellLevel3int;
    public int spellLevel4int;
    public int spellLevel5int;

    public string spellName1;
    public string spellName2;
    public string spellName3;
    public string spellName4;
    public string spellName5;

    public void SetSpell(Spells spells)
    {
        if (spells != null)
        {
            if (spellHolder.i == 0)
            {
                spellIcon1.sprite = spells.Icon;
                spellLevel1.text = spells.Level.ToString();
                spellLevel1int = spells.Level;
                spellName1 = spells.name;
            }
            else if (spellHolder.i == 1)
            {
                spellIcon2.sprite = spells.Icon;
                spellLevel2.text = spells.Level.ToString();
                spellLevel2int = spells.Level;
                spellName2 = spells.name;
            }
            else if (spellHolder.i == 2)
            {
                spellIcon3.sprite = spells.Icon;
                spellLevel3.text = spells.Level.ToString();
                spellLevel3int = spells.Level;
                spellName3 = spells.name;
            }
            else if (spellHolder.i == 3)
            {
                spellIcon4.sprite = spells.Icon;
                spellLevel4.text = spells.Level.ToString();
                spellLevel4int = spells.Level;
                spellName4 = spells.name;
            }
            else if (spellHolder.i == 4)
            {
                spellIcon5.sprite = spells.Icon;
                spellLevel5.text = spells.Level.ToString();
                spellLevel5int = spells.Level;
                spellName5 = spells.name;
            }

        }
    }

    public void UpdateLevel(Spells spells)
    {
        Debug.Log(spells.name);
        Debug.Log(spells);
        Debug.Log(spellName1);

        if (spells.name == spellName1)
        {
            spellLevel1int++;
            spellLevel1.text = spellLevel1int.ToString();
        }
        else if (spells.name == spellName2)
        {
            spellLevel2int++;
            spellLevel2.text = spellLevel2int.ToString();
        }
        else if (spells.name == spellName3)
        {
            spellLevel3int++;
            spellLevel3.text = spellLevel3int.ToString();
        }
        else if (spells.name == spellName4)
        {
            spellLevel4int++;
            spellLevel4.text = spellLevel4int.ToString();
        }
        else if (spells.name == spellName5)
        {
            spellLevel5int++;
            spellLevel5.text = spellLevel5int.ToString();
        }

    }

    void Update()
    {
        health.fillAmount = (float)PlayerController.Player_HP / PlayerController.Player_MaxHP;
        XP.fillAmount = (float)PlayerController.Player_EXP / PlayerController.Player_MaxEXP;
        healthText.text = PlayerController.Player_HP.ToString() + " / " + PlayerController.Player_MaxHP.ToString();
        XPText.text = PlayerController.Player_EXP.ToString() + " / " + PlayerController.Player_MaxEXP.ToString();
        LevelText.text = "Level: " + PlayerController.Player_Level.ToString();

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
