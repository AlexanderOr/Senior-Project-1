using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateIceSpell", menuName = "Spells/Ultimate Ice", order = 1)]
public class UltimateIceSpells : Spells
{
    public GameObject UltimateIcePrefab;
    public GameObject Player;
    public PlayerController Playercontroller;
    public float Duration = 5;
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
        GameObject UltimateIceSpell = Instantiate(UltimateIcePrefab, playersPosition, Quaternion.Euler(0, 0, 0));
        UltimateIceSpell.transform.parent = Player.transform;

        // Optionally, add logic for duration, effects, etc.
        Debug.Log($"Casting {SpellName} towards {targetPosition}");

    }
}
