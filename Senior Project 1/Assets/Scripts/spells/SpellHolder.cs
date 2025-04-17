using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public HPManager manager;
    public SpellMenuManager spellMenuManager;
    public PlayerController playerController;
    public int i;

    public List<Spells> availableSpells;
    public List<Spells> basicSpells;
    public List<Spells> advancedSpells;
    public List<Spells> utilitySpells;
    public List<Spells> ultimateSpells;

    public List<Spells> playerSpell;

    public Dictionary<Spells, int> playerSpells = new Dictionary<Spells, int>();

    // Start is called before the first frame update
    void Start()
    {
        LoadSpells();
        LevelUp();

    }

    void LoadSpells()
    {
        basicSpells = new List<Spells> {
            Resources.Load<Spells>("Spells/FireCone"),
            Resources.Load<Spells>("Spells/BasicIceSpell"),
            Resources.Load<Spells>("Spells/BasicNatureSpell"),
            Resources.Load<Spells>("Spells/BasicArcaneSpell"),
            Resources.Load<Spells>("Spells/BasicRockSpell")
        };

        advancedSpells = new List<Spells> {
            Resources.Load<Spells>("Spells/FireLance"),
            Resources.Load<Spells>("Spells/AdvancedIceSpell"),
            Resources.Load<Spells>("Spells/AdvancedRockSpell"),
            Resources.Load<Spells>("Spells/AdvancedArcaneSpell"),
            Resources.Load<Spells>("Spells/AdvancedNatureSpell")
        };

        utilitySpells = new List<Spells> {
            Resources.Load<Spells>("Spells/UtilityNatureSpell"),
            Resources.Load<Spells>("Spells/UtilityArcaneSpell"),
            Resources.Load<Spells>("Spells/UtilityRockSpell")
        };

        ultimateSpells = new List<Spells> {
            Resources.Load<Spells>("Spells/UltimateFireSpell"),
            Resources.Load<Spells>("Spells/UltimateIceSpell"),
            Resources.Load<Spells>("Spells/UltimateNatureSpell"),
            Resources.Load<Spells>("Spells/UltimateArcaneSpell"),
            Resources.Load<Spells>("Spells/UltimateRockSpell")
        };

        availableSpells = new List<Spells>();
        availableSpells.AddRange(basicSpells);
    }

    public void LevelUp()
    {
        PlayerController.Player_Level++;

        // Offer three random choices from the available spells
        List<Spells> choices = new List<Spells>();
        if (playerSpell.Count >= 5)
        {
            // Generate upgrade options or heal option
            for (int i = 0; i < 3; i++)
            {
                if (Random.Range(0, 4) < 3) // 75% chance to offer a spell upgrade
                {
                    Spells randomOwnedSpell = playerSpell[Random.Range(0, playerSpell.Count)];
                    if (!choices.Contains(randomOwnedSpell))
                    {
                        choices.Add(randomOwnedSpell);
                    }
                    else
                    {
                        i--; // Retry if duplicate
                    }
                }
                else // 25% chance to offer a heal option
                {
                    // Assuming you have a placeholder or method for heal option
                    Spells healOption = Resources.Load<Spells>("Spells/HealOption");
                    if (!choices.Contains(healOption))
                    {
                        choices.Add(healOption);
                    }
                    else
                    {
                        i--; // Retry if duplicate
                    }
                }
            }
        }
        else
        {
            // If player has fewer than 5 spells, generate new spell options
            List<Spells> possibleSpells = GetAvailableSpellOptions();
            for (int i = 0; i < 3 && i < possibleSpells.Count; i++)
            {
                Spells randomSpell;
                do
                {
                    randomSpell = possibleSpells[Random.Range(0, possibleSpells.Count)];
                } while (choices.Contains(randomSpell));

                choices.Add(randomSpell);
                randomSpell.SetDescription();
            }
        }

        // Display choices to player (pseudo-code for UI interaction)
        spellMenuManager.Activate(choices);
        // DisplayChoices(choices);

        // Assume player selects one of the choices
        //Spells chosenSpell = choices[Random.Range(0, choices.Count)];
        //AddOrUpgradeSpell(chosenSpell);
    }

    private List<Spells> GetAvailableSpellOptions()
    {
        List<Spells> possibleSpells = new List<Spells>();

        // Add spells based on player level thresholds
        if (PlayerController.Player_Level >= 1) possibleSpells.AddRange(basicSpells);
        if (PlayerController.Player_Level >= 3) possibleSpells.AddRange(advancedSpells);
        if (PlayerController.Player_Level >= 5) possibleSpells.AddRange(utilitySpells);
        if (PlayerController.Player_Level >= 7) possibleSpells.AddRange(ultimateSpells);

        return possibleSpells;
    }

    public void AddOrUpgradeSpell(Spells spell)
    {
        if (playerSpells.ContainsKey(spell))
        {
            // Upgrade spell level if player already owns it
            playerSpells[spell]++;
            //Debug.Log(playerSpells[spell]);
            manager.UpdateLevel(spell);
        }
        else
        {
            // Add new spell at level 1 if player doesn't own it
            playerSpells[spell] = 1;
            playerSpell.Add(spell);
            availableSpells.Add(spell);
            manager.SetSpell(spell);
            i++;
        }

    }
}

