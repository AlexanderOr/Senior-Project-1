using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public List<Spells> availableSpells;

    // Start is called before the first frame update
    void Start()
    {
        availableSpells = new List<Spells>();
        Spells basicFireSpell = Resources.Load<Spells>("Spells/FireConeSpell");
        availableSpells.Add(basicFireSpell);    }

}
