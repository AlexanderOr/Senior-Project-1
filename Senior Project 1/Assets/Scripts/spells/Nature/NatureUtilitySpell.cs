using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityNatureSpell", menuName = "Spells/Utility Nature", order = 1)]
public class UtilityNatureSpells : Spells
{
    public GameObject UtilityNaturePrefab;
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
        // Create and orient the fire cone spell towards the target position
        Vector2 direction = targetPosition - playersPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate and rotate the cone
        GameObject UtilityNatureSpell = Instantiate(UtilityNaturePrefab, playersPosition, Quaternion.Euler(0, 0, 0));

        // Optionally, add logic for duration, effects, etc.
        Playercontroller.Heal();
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }
}
