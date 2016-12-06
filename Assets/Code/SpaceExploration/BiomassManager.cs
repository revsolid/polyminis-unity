using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BiomassManager : MonoBehaviour {

    public OrbitalUI Orbital;
    public Text BiomassCounter;
    static int Biomass;


    public static void ConsumeBiomass(int amount)
    {
        if (amount <= Biomass)
        {
            Biomass -= amount;
        }
    }

    public static void AddBiomass(int amount)
    {
        Biomass += amount;
    }

    public void Update()
    {
        BiomassCounter.text = "" + Biomass;
    }

    public static bool HasBiomass(int amount)
    {
        return amount <= Biomass;
    }
    
}
