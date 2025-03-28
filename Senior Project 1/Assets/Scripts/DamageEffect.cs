using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image damageImage; // Assign in the Inspector
    public float fadeSpeed = 1.5f; // Adjust fade speed

    private void Start()
    {
        damageImage.color = new Color(1, 0, 0, 0); // Ensure it's invisible at start
    }

    public void ShowDamageEffect()
    {
        Debug.Log("called effect");
        damageImage.color = new Color(1, 0, 0, 0.5f); // Flash red
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        while (damageImage.color.a > 0)
        {
            Color newColor = damageImage.color;
            newColor.a -= Time.deltaTime * fadeSpeed;
            damageImage.color = newColor;
            yield return null;
        }
    }
}
