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
		ExtractButton.gameObject.SetActive(Species.CreatorName == Session.Instance.UserName);
		SampleButton.gameObject.SetActive(Species.CreatorName == Session.Instance.UserName);
		SpeciesNameText.text = Species.SpeciesName +"("+Species.Percentage+")";
	}
}
