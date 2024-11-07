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

        // Determine spell category based on level
        List<Spells> possibleSpells = GetAvailableSpellOptions();

        // Offer three random choices from the available spells
        List<Spells> choices = new List<Spells>();
        for (int i = 0; i < 3 && i < possibleSpells.Count; i++)
        {
            Spells randomSpell;

            // Keep generating a random spell until we find one that isn’t already in choices
            do
            {
                randomSpell = possibleSpells[Random.Range(0, possibleSpells.Count)];
            } while (choices.Contains(randomSpell));

            choices.Add(randomSpell);
            Debug.Log(randomSpell);
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
        if (PlayerController.Player_Level >= 10) possibleSpells.AddRange(ultimateSpells);

        return possibleSpells;
    }

    public void AddOrUpgradeSpell(Spells spell)
    {
        if (playerSpells.ContainsKey(spell))
        {
            // Upgrade spell level if player already owns it
            playerSpells[spell]++;
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

