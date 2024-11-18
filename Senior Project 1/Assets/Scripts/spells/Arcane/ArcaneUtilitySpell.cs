using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityArcaneSpell", menuName = "Spells/Utility Arcane", order = 1)]
public class UtilityArcaneSpells : Spells
{
    public GameObject UtilityArcanePrefab;
    public GameObject Player;
    public PlayerController Playercontroller;
    public float range;
    public float angle;

    public void Awake()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        //Playercontroller = Player.GetComponent<PlayerController>();
    }


    public override void CastSpell(Vector2 targetPosition, Vector2 playersPosition)
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        // Create and orient the fire cone spell towards the target position
        Vector2 direction = targetPosition - playersPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate and rotate the cone
        //GameObject UtilityNatureSpell = Instantiate(UtilityArcanePrefab, playersPosition, Quaternion.Euler(0, 0, 0));

        // Optionally, add logic for duration, effects, etc.
        //Playercontroller.Teleport(targetPosition);
        Player.transform.position = targetPosition;
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }

    public override void SetDescription()
    {
        description = $"A Utility arcane spell that teleports the player to the cursor.";
    }
}
