using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireConeSpell : Spells
{
    public GameObject FireConePrefab;
    public GameObject Player;
    public float range;
    public float angle;


    public override void CastSpell(Vector2 targetPosition)
    {
        throw new System.NotImplementedException();

        // Create and orient the fire cone spell towards the target position
        Vector2 direction = targetPosition - (Vector2)Player.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate and rotate the cone
        GameObject fireCone = Instantiate(FireConePrefab, Player.transform.position, Quaternion.Euler(0, 0, angle));

        // Optionally, add logic for duration, effects, etc.
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }
}
