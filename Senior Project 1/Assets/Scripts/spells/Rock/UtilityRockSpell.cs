using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "UtilityRockSpell", menuName = "Spells/Utility Rock", order = 1)]
public class UtilityRockSpells : Spells
{
    public GameObject UtilityRockPrefab;
    public GameObject Player;
    public PlayerController Playercontroller;
    public float Duration = 3;
    public float range;
    public float angle;

    public void Awake()
    {
        Debug.Log("Called Awake");
        //Playercontroller = Player.GetComponent<PlayerController>();
    }



    public override void CastSpell(Vector2 targetPosition, Vector2 playersPosition)
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        // Create and orient the fire cone spell towards the target position
        Vector2 direction = targetPosition - playersPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate and rotate the cone
        GameObject UtilityRockSpell = Instantiate(UtilityRockPrefab, playersPosition, Quaternion.Euler(0, 0, 0));
        UtilityRockSpell.transform.parent = Player.transform;

        // Optionally, add logic for duration, effects, etc.
        Playercontroller.ImmuneRock(Duration);
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }

    public override void SetDescription()
    {
        description = $"A Utility rock spell that protects the player from damage for {Duration} seconds.";
    }
}
