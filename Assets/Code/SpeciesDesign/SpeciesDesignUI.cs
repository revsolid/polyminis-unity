using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeciesDesignUI : MonoBehaviour
{
	public VerticalLayoutGroup SmallSplices;
	public VerticalLayoutGroup MedSplices;
	public VerticalLayoutGroup LargeSplices;
	public SpliceButton SpliceButtonRendererPrototype;
	public DnaHelix Helix;
	public ColorConfig ColorConfig;
	public static ColorConfig SColorConfig;

	SpeciesModel CurrentSelection;

	// Use this for initialization
	void Start ()
	{
		if (SpeciesDesignUI.SColorConfig == null)
		{
			Debug.Log("Setting static");
			SpeciesDesignUI.SColorConfig = this.ColorConfig;
		}

		SpliceButton.OnClickEvent += (button) => OnSpliceButtonClicked(button);
		DnaHelix.OnSpliceRemovedEvent += (model) => OnSpliceRemovedFromHelix(model);
		foreach(KeyValuePair<string, SpliceModel> entry in Almanac.Instance.AvailableSplices)
		{
			AddSplice(entry.Value);
  		}
		CurrentSelection = new SpeciesModel();
	}
	// Update is called once per frame
	void Update() {}


	void AddSplice(SpliceModel model)
	{
  		SpliceButton sbutton = GameObject.Instantiate(SpliceButtonRendererPrototype);
		sbutton.Model = model;
		switch (model.TraitSize)
		{
			case TraitSize.SMALL: 
			sbutton.transform.SetParent(SmallSplices.transform);
				break;
			case TraitSize.MEDIUM: 
			sbutton.transform.SetParent(MedSplices.transform);
				break;
			case TraitSize.LARGE: 
			sbutton.transform.SetParent(LargeSplices.transform);
				break;
		}
    	sbutton.transform.localPosition = Vector3.zero;
    	sbutton.transform.localScale = Vector3.one;
    	sbutton.transform.SetAsFirstSibling();
	}
	
	bool ValidateSelection(SpliceModel model)
	{
		// 4 Small, 2 Med, 1 Large
		var small = CurrentSelection.Splices.Where( x => x.TraitSize == TraitSize.SMALL).Count();
		var med = CurrentSelection.Splices.Where( x => x.TraitSize == TraitSize.MEDIUM).Count();
		var large = CurrentSelection.Splices.Where( x => x.TraitSize == TraitSize.LARGE).Count(); 

		if (model.TraitSize == TraitSize.SMALL)
			small++;
		if (model.TraitSize == TraitSize.MEDIUM)
			med++;
		if (model.TraitSize == TraitSize.LARGE)
			large++;

		return small <= 4 && med <= 2 && large <=1;
	}

	void OnSpliceButtonClicked(SpliceButton button)
	{
		Debug.Log(button.Model.Name);

		if (ValidateSelection(button.Model))
		{
			CurrentSelection.Splices.Add(button.Model);
			Helix.AddSelectedSplice(button.Model);
			Destroy(button.gameObject);
		}
	}
	
	void OnSpliceRemovedFromHelix(SpliceModel model)
	{
		CurrentSelection.Splices.Remove(model);
		AddSplice(model);
	}
	
	public void OnExitButtonClicked()
	{
		gameObject.SetActive(false);
	}
	
	public void OnSaveButtonClicked()
	{
		// Validate
		
		SpeciesModel newModel = new SpeciesModel();
		newModel.Name = CurrentSelection.Name;
		newModel.Splices = CurrentSelection.Splices; //TODO: Thourough clone
		Session.Instance.Species.Add(newModel);
		// Serialize Species
		Debug.Log(JsonUtility.ToJson(CurrentSelection));
		
		// Send to Server
	}
	
	public void OnNameChanged(InputField input)
	{
		if (input.text.Length <= 0) 
		{
			Debug.Log("No value entered");
			return;
		}
		CurrentSelection.Name = input.text;
	}
	

}
