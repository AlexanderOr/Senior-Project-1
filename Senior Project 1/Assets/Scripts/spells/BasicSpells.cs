using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Fire, Ice, Earth, Nature, Arcane}
public abstract class Spells : ScriptableObject
{
    public string SpellName;
    public int Level;
    public SpellType type;
    public int CastTime;
    public int CoolDown;
    public Sprite Icon;

    public abstract void CastSpell(Vector2 targetPosition);

}

