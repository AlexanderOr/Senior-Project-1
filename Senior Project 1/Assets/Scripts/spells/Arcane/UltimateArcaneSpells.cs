using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateArcaneSpell", menuName = "Spells/Ultimate Arcane", order = 1)]
public class UltimateArcaneSpells : Spells
{
    public GameObject UltimateArcanePrefab;
    //public GameObject Player;
    public float range;
    public float angle;


    public override void CastSpell(Vector2 targetPosition, Vector2 playersPosition)
    {
        // Create and orient the fire cone spell towards the target position
        Vector2 direction = targetPosition - playersPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate and rotate the cone
        GameObject UltimateArcane = Instantiate(UltimateArcanePrefab, playersPosition, Quaternion.Euler(0, 0, angle));

        // Optionally, add logic for duration, effects, etc.
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }

    public override void SetDescription()
    {
        description = $"A ultimate arcane spell that deals {Damage + (5 * (Level - 1))} + ({(5 * (Level))}) damage around the player.";
    }
}
