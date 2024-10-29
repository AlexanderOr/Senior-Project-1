using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour
{
    public List<Spells> availableSpells;
    public HPManager manager;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        availableSpells = new List<Spells>();
        Spells basicFireSpell = Resources.Load<Spells>("Spells/FireCone");
        Spells basicIceSpell = Resources.Load<Spells>("Spells/BasicIceSpell");
        Spells basicNatureSpell = Resources.Load<Spells>("Spells/BasicNatureSpell");
        Spells basicArcaneSpell = Resources.Load<Spells>("Spells/BasicArcaneSpell");
        Spells basicRockSpell = Resources.Load<Spells>("Spells/BasicRockSpell");
        Spells AdvancedFireSpell = Resources.Load<Spells>("Spells/FireLance");
        Spells AdvancedIceSpell = Resources.Load<Spells>("Spells/AdvancedIceSpell");
        Spells AdvancedRockSpell = Resources.Load<Spells>("Spells/AdvancedRockSpell");
        Spells AdvancedArcaneSpell = Resources.Load<Spells>("Spells/AdvancedArcaneSpell");
        Spells AdvancedNatureSpell = Resources.Load<Spells>("Spells/AdvancedNatureSpell");
        Spells UtilityNatureSpell = Resources.Load<Spells>("Spells/UtilityNatureSpell");
        availableSpells.Add(UtilityNatureSpell);
        availableSpells.Add(AdvancedFireSpell);
        availableSpells.Add(AdvancedIceSpell);
        availableSpells.Add(AdvancedRockSpell);
        availableSpells.Add(AdvancedArcaneSpell);
        availableSpells.Add(AdvancedNatureSpell);
        availableSpells.Add(basicFireSpell);
        availableSpells.Add(basicIceSpell);
        availableSpells.Add(basicNatureSpell);
        availableSpells.Add(basicArcaneSpell);
        availableSpells.Add(basicRockSpell);

        for (i=0; i < 5; i++)
        {
            manager.SetSpell(availableSpells[i]);
        }
        
    }

}
