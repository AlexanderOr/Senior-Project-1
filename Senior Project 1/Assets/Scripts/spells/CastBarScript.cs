using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastBarScript : MonoBehaviour
{
    public GameObject CastBar;
    public TMP_Text SpellName;
    public TMP_Text SpellTime;
    public Image Icon;
    public Image CastingBar;

    private Coroutine castingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        CastBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateBar(Spells spell)
    {
        // Activate the cast bar and set initial values
        CastingBar.color = spell.SpellColor;
        CastingBar.fillAmount = 0; // Reset fill amount
        Icon.sprite = spell.Icon;
        SpellName.text = spell.SpellName;
        SpellTime.text = spell.CastTime.ToString("F1"); // Show 1 decimal place
        CastBar.SetActive(true);
                   
        // Start the casting coroutine
        if (castingCoroutine != null)
        {
            StopCoroutine(castingCoroutine);
        }
        castingCoroutine = StartCoroutine(FillCastBar(spell.CastTime));
    }

    private IEnumerator FillCastBar(float castTime)
    {
        float elapsedTime = 0;

        // Update the bar over time
        while (elapsedTime < castTime)
        {
            elapsedTime += Time.deltaTime;
            CastingBar.fillAmount = elapsedTime / castTime; // Set fill amount
            SpellTime.text = (castTime - elapsedTime).ToString("F1"); // Update remaining time
            yield return null;
        }

        // Ensure the bar is fully filled and reset after completion
        CastingBar.fillAmount = 1;
        CastBar.SetActive(false);
    }
}
