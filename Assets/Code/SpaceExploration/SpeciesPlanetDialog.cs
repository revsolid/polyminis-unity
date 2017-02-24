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
		PlanetInteractionCommand pInteractionCommand;
		switch (CurrentAction)	
		{
			case SpeciesPlanetAction.Deploy:
				pInteractionCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.DEPLOY);
				pInteractionCommand.Epoch = PlanetModel.Epoch; 
				pInteractionCommand.PlanetId = PlanetModel.ID; 
				pInteractionCommand.DeployedBiomass = 0.10f;
				pInteractionCommand.Species = SpeciesModel;
				Connection.Instance.Send(JsonUtility.ToJson(pInteractionCommand));
				break;
			case SpeciesPlanetAction.Sample:
				InventoryCommand sampleSpeciesCommand = new InventoryCommand(InventoryCommandType.SAMPLE_FROM_PLANET);
                sampleSpeciesCommand.Species = SpeciesModel;
				sampleSpeciesCommand.Epoch = PlanetModel.Epoch; 
				sampleSpeciesCommand.PlanetId = PlanetModel.ID; 
                sampleSpeciesCommand.Slot = Session.Instance.NextAvailableSlot();
				if (sampleSpeciesCommand.Slot == -1)
				{
				// This is an issue	
					Debug.Log("NO MORE SLOTS FOR YOU!!!!!");
				}
				else
				{
					Connection.Instance.Send(JsonUtility.ToJson(sampleSpeciesCommand));
				}
				break;
			case SpeciesPlanetAction.Research:
				break;
			case SpeciesPlanetAction.Extract:
				pInteractionCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.EXTRACT);
				pInteractionCommand.Epoch = PlanetModel.Epoch; 
				pInteractionCommand.PlanetId = PlanetModel.ID; 
				//pInteractionCommand.ExtractedPercentage = 0.10f;
				pInteractionCommand.Species = SpeciesModel;
				Connection.Instance.Send(JsonUtility.ToJson(pInteractionCommand));
				break;
		}
		Destroy(gameObject);
	}
}
