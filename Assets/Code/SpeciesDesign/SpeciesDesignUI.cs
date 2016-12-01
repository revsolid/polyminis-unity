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
	public DnaSeq DnaSequencer;
	public InstinctsTunner InstinctsTunner;
	public ColorConfig ColorConfig;
	public static ColorConfig SColorConfig;

	SpeciesModel CurrentSelection;

	// Use this for initialization
	void Start()	
	{
		Reset();
	}

	void Reset()
	{
		gameObject.SetActive(false);
		Helix.Reset();
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
		InstinctsTunner.Ready();
		CurrentSelection = new SpeciesModel();
	}
	
	void ResetLayoutGroup(LayoutGroup lg)
    {
        var children = new List<GameObject>();
        foreach (Transform child in lg.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
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
    //	sbutton.transform.localPosition = Vector3.zero;
    //	sbutton.transform.localScale = Vector3.one;
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
		if (SelectSplice(button.Model))
			Destroy(button.gameObject);
	}
	
	bool SelectSplice(SpliceModel model)
	{
		if (ValidateSelection(model))
		{
			CurrentSelection.Splices.Add(model);
			Helix.AddSelectedSplice(model);
			DnaSequencer.ActivateSelection(CurrentSelection);
			InstinctsTunner.AddSplice(model.EInstinct);
			return true;
		}
		return false;
	}
	
	void OnSpliceRemovedFromHelix(SpliceModel model)
	{
		CurrentSelection.Splices.Remove(model);
		AddSplice(model);
		DnaSequencer.ActivateSelection(CurrentSelection);
		InstinctsTunner.RemoveSplice(model.EInstinct);
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
		Session.Instance.Species[name] = newModel;
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
	
	public void OpenWithSpecies(string name)
	{
		SpeciesModel m = Session.Instance.Species[name];
		if (m == CurrentSelection)
		{
			gameObject.SetActive(true);
		}
		Debug.Log(m);
		Debug.Log("XXX");
		
		if (m != null)
		{
			// Reset
			if (CurrentSelection != null)
			{
				foreach(SpliceModel sm in CurrentSelection.Splices)	
				{
					AddSplice(sm);
					DnaSequencer.ActivateSelection(CurrentSelection);
					InstinctsTunner.RemoveSplice(sm.EInstinct);
				}
			}
			else
			{
				CurrentSelection = new SpeciesModel();
			}
			foreach(SpliceModel sm in m.Splices)	
			{
				Helix.AddSelectedSplice(sm);
				DnaSequencer.ActivateSelection(CurrentSelection);
				InstinctsTunner.AddSplice(sm.EInstinct);
			}
			CurrentSelection = m;
			gameObject.SetActive(true);
		}
	}

}
