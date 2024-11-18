using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpellMenuManager : MonoBehaviour
{

    public GameObject SpellMenu;
    public PlayerController playerController;

    public Image[] spellIcons; // Assign these in the Inspector
    public Button[] confirmButtons; // Assign these in the Inspector
    public TextMeshProUGUI[] spellDescriptions;
    public TextMeshProUGUI[] spellNames;

    private List<Spells> currentSpellChoices;
    public SpellHolder spellHolder;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < confirmButtons.Length; i++)
        {
            int index = i; // Capture index for the lambda
            confirmButtons[i].onClick.AddListener(() => OnConfirmChoice(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(List<Spells> choices)
    {
        SpellMenu.SetActive(true);
        

        // Store current choices for reference when a button is clicked
        currentSpellChoices = choices;

        // Update the UI with each spell's icon
        for (int i = 0; i < choices.Count; i++)
        {
            spellIcons[i].sprite = choices[i].Icon; // Assuming each Spell has an icon property
            spellIcons[i].gameObject.SetActive(true);
            spellDescriptions[i].text = choices[i].description;
            spellNames[i].text = choices[i].SpellName;
            confirmButtons[i].gameObject.SetActive(true);
            spellDescriptions[i].gameObject.SetActive(true);
            spellNames[i].gameObject.SetActive(true);
        }

        // Hide any extra UI elements if fewer than 3 choices
        for (int i = choices.Count; i < spellIcons.Length; i++)
        {
            spellIcons[i].gameObject.SetActive(false);
            confirmButtons[i].gameObject.SetActive(false);
            spellDescriptions[i].gameObject.SetActive(false);
            spellNames[i].gameObject.SetActive(false);
        }

        SpellMenu.SetActive(true); // Show the menu
        playerController.isPaused = true;
    }

    private void OnConfirmChoice(int choiceIndex)
    {
        if (choiceIndex < currentSpellChoices.Count)
        {
            var chosenOption = currentSpellChoices[choiceIndex];

            // Check if the chosen option is a HealOption
            if (chosenOption is HealOption healOption)
            {
                healOption.ApplyHeal(spellHolder.playerController); // Apply healing
            }
            else
            {
                spellHolder.AddOrUpgradeSpell(chosenOption); // Add or upgrade spell
            }
        }

        // Hide the menu after selection
        SpellMenu.SetActive(false);
        playerController.isPaused = false;
    }
}
