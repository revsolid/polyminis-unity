using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpochDetailDataGrid : MonoBehaviour
{
	public Text SpeciesText1;
	public Text SpeciesText2;
	public Text SpeciesText3;

	// Use this for initialization
	void Start()
	{
		Reset();
	}

	void Reset()
	{
		SpeciesText1.gameObject.SetActive(false);
		SpeciesText2.gameObject.SetActive(false);
		SpeciesText3.gameObject.SetActive(false);
	}
	
	public void UpdateGrid(List<string> names, List<float> values, List<float> deltas)
	{
		Reset();
		
		Text[] texts = { SpeciesText1, SpeciesText2, SpeciesText3 };
		
		for(int i = 0; i < names.Count; ++i)
		{
			texts[i].gameObject.SetActive(true);
			texts[i].text = string.Format("{0}: {1:###.#%}", names[i], values[i]);//, deltas[i]);
		}
	}
}