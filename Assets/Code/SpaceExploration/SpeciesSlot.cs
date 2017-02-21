using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeciesSlot : MonoBehaviour {

	public SpeciesModel Species;
	public Button ExtractButton;
	public Button SampleButton;
	public Text SpeciesNameText;

	// Use this for initialization
	void Start ()
	{
		if (Species.CreatorName != Session.Instance.UserName)	
		{
			ExtractButton.gameObject.SetActive(false);	
			SampleButton.gameObject.SetActive(false);
		}
		Debug.Log(Species.CreatorName);
	}
	
	// Update is called once per frame
	void Update ()
	{
		SpeciesNameText.text = Species.SpeciesName;
	}
}
