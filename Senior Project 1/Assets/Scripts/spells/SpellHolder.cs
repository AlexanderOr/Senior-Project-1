using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public List<Spells> availableSpells;
    public HPManager manager;

    // Start is called before the first frame update
    void Start()
    {
        availableSpells = new List<Spells>();
        Spells basicFireSpell = Resources.Load<Spells>("Spells/FireCone");
        Spells basicIceSpell = Resources.Load<Spells>("Spells/BasicIceSpell");
        availableSpells.Add(basicFireSpell);
        availableSpells.Add(basicIceSpell);

        manager.SetSpell(basicFireSpell);
        manager.SetSpell(basicIceSpell);
    }

}
