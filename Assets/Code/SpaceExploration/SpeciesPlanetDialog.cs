﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpeciesPlanetAction
{
	Extract,
	Deploy,
	Research,
	Sample,
	Erroring
}

public class SpeciesPlanetDialog : MonoBehaviour
{
	public Slider BiomassSlider;
	public PolyminisDialog MainDialog;
	public Text BiomassValue;
	
	public SpeciesPlanetAction CurrentAction = SpeciesPlanetAction.Deploy;
	
	public PlanetModel PlanetModel;
	public SpeciesModel SpeciesModel;
	
	// Use this for initialization
	void Start ()
	{
		BiomassValue.text = BiomassSlider.normalizedValue + "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (CurrentAction)	
		{
			case SpeciesPlanetAction.Deploy:
				MainDialog.DialogMessage.text = "How much Biomass do you want to Deploy this species with?";
				BiomassSlider.gameObject.SetActive(true);
				break;
			case SpeciesPlanetAction.Extract:
				MainDialog.DialogMessage.text  = "How much Biomass do you want to Extract from this Species?";
				BiomassSlider.gameObject.SetActive(true);
				break;
			case SpeciesPlanetAction.Research:
				MainDialog.DialogMessage.text = "Do you want to research this Species? (Researching takes a slot)";
				BiomassSlider.gameObject.SetActive(false);
				break;
			case SpeciesPlanetAction.Sample:
				MainDialog.DialogMessage.text = "Do you want to Sample DNA from this Species?";
				BiomassSlider.gameObject.SetActive(false);
				break;
			case SpeciesPlanetAction.Erroring:
				MainDialog.DialogMessage.text = "Something went Wrong :S";
				BiomassSlider.gameObject.SetActive(false);
				break;
		}
		if (!BiomassSlider.gameObject.active)
		{
			BiomassValue.text = "";
		}
		
	}
	
	public void OnSliderValueChange()
	{
		BiomassValue.text = BiomassSlider.normalizedValue + "";
	}
	
	public void OnAccept()
	{
		PlanetInteractionCommand deployCommand;
		switch (CurrentAction)	
		{
			case SpeciesPlanetAction.Deploy:
				deployCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.DEPLOY);
				deployCommand.Epoch = PlanetModel.Epoch; 
				deployCommand.PlanetId = PlanetModel.ID; 
				deployCommand.DeployedBiomass = 0.10f;
				deployCommand.Species = SpeciesModel;
				Connection.Instance.Send(JsonUtility.ToJson(deployCommand));
				break;
			case SpeciesPlanetAction.Extract:
				deployCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.EXTRACT);
				deployCommand.Epoch = PlanetModel.Epoch; 
				deployCommand.PlanetId = PlanetModel.ID; 
				//deployCommand.ExtractedPercentage = 0.10f;
				deployCommand.Species = SpeciesModel;
				Connection.Instance.Send(JsonUtility.ToJson(deployCommand));
				break;
				break;
			case SpeciesPlanetAction.Research:
				break;
		}
		Destroy(gameObject);
	}
}
