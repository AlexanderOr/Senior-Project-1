using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Fire, Ice, Earth, Nature, Arcane}

public abstract class Spells : ScriptableObject
{
    public string SpellName;
    public string description;
    public int Level;
    public SpellType type;
    public float CastTime;
    public float CoolDown;
    public Sprite Icon;
    public int Damage;
    public bool isBleeding;
    public bool isStunned;
    public bool isKnockedBack;
    public float ForceAmount = 10;
    public float speedReduction = 0f;
    public AudioClip SpellSound;
    public Color SpellColor;
    public bool DeleteOnHit = false;

    public abstract void CastSpell(Vector2 targetPosition, Vector2 playerPosition);

    public abstract void SetDescription();

}

