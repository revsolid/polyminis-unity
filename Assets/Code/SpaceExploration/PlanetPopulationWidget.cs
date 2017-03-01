using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetPopulationWidget : MonoBehaviour {

    public Slider[] PopulationSliders;
    public Text[] Labels;
	// Use this for initialization
	void Start () {
		
	}
	
	
    public void UpdateWidget(PlanetModel p)
    {
        int i = 0;

        foreach (SpeciesModel sm in p.Species)
        {
            PopulationSliders[i].maxValue = 100f;
            PopulationSliders[i].value = sm.Percentage;
            Labels[i].text = sm.SpeciesName + " ( " + sm.Percentage + "% ) ";
            print(sm.SpeciesName + " : " + sm.Percentage);
            i++;
        }
    }

}
