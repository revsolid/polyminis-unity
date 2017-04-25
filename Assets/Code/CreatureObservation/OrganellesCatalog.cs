using UnityEngine;
using System.Collections;

public class OrganellesCatalog : MonoBehaviour
{
    public Organelle PlasmaPrototype;
    
    public Actuator HorMovPrototype;
    public Actuator VerMovPrototype;
    
    public Organelle SpeedPrototype;
    public Organelle ThermoHotPrototype;
    public Organelle ThermoColdPrototype;

    public Organelle GetForTID(int tid)
    {
        TraitModel trait; 
        
        // TODO: This is pretty terrible, but without some external tool / Reflection support 
        // there is no other way. Since time's pressing this is acceptable. I repent for my sins.
        
        if (Almanac.Instance.TraitData.TryGetValue(tid, out trait))
        {
            Organelle[] organelles = { HorMovPrototype, VerMovPrototype, ThermoHotPrototype, ThermoColdPrototype };
            string[]    internalNames = { "hormov", "vermov", "hotresist", "coldprototype" };
            for (int i = 0; i < organelles.Length; ++i)
            {
                if (trait.InternalName == internalNames[i])
                {
                   return GameObject.Instantiate(organelles[i]);
                }
            }
        }
        
        return GameObject.Instantiate(PlasmaPrototype);
    }
}