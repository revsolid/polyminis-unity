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
    public InputField NameInput;
	public static ColorConfig SColorConfig;

	SpeciesModel CurrentSelection;

	// Use this for initialization
	void Start()	
	{
        Initialize();
	}

    // Previously named Reset but I think Initialize fits the purpose better
    void Initialize() 
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

    //Clear contents and give you a window that looks new
    void ResetWindow()
    {


        // "click" all splices inside the helix
        while (Helix.SmallSplices.transform.childCount > 0)
        {
            Transform child = Helix.SmallSplices.transform.GetChild(0);
            child.transform.parent = null;
            Helix.ClickSpliceRenderer(child.gameObject.GetComponent<SpliceDnaHelixRenderer>());
        }
        while (Helix.MedSplices.transform.childCount > 0)
        {
            Transform child = Helix.MedSplices.transform.GetChild(0);
            child.transform.parent = null;
            Helix.ClickSpliceRenderer(child.gameObject.GetComponent<SpliceDnaHelixRenderer>());
        }
        while (Helix.LargeSplices.transform.childCount > 0)
        {
            Transform child = Helix.LargeSplices.transform.GetChild(0);
            child.transform.parent = null;
            Helix.ClickSpliceRenderer(child.gameObject.GetComponent<SpliceDnaHelixRenderer>());
        }

        // If CurrenSelection is null then it obviously is already new
        CurrentSelection = new SpeciesModel();
        DnaSequencer.ActivateSelection(CurrentSelection);


    }

    void ResetLayoutGroup(LayoutGroup lg)
    {
        var children = new List<GameObject>();
        foreach (Transform child in lg.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
	}
    
	// Update is called once per frame
	void Update() {}

    // so that buttons have different names
    string SpliceButtonName(SpliceModel model)
    {
        return "SpliceButtonRenderer-" + model.InternalName;
    }

	void AddSplice(SpliceModel model)
	{
  		SpliceButton sbutton = GameObject.Instantiate(SpliceButtonRendererPrototype);
        sbutton.gameObject.name = SpliceButtonName(model);
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
        ClickSpliceButton(button);
    }

    void ClickSpliceButton(SpliceButton button)
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
        CurrentSelection.Name = NameInput.text;
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

        if(m != null)
        {
            if (m == CurrentSelection)
            {
                //do nothing...it's loaded already
            }
            else
            {
                CurrentSelection = m;

                // wipe it clean!
                ResetWindow();

                // set name field
                NameInput.text = name;

                // and then "click" the responding splices new stuff
                foreach (SpliceModel sm in m.Splices)
                {
                    for (int i = 0; i < SmallSplices.transform.childCount; i++)
                    {
                        if (SmallSplices.transform.GetChild(i).gameObject.name.Equals(SpliceButtonName(sm)))
                        {
                            ClickSpliceButton(SmallSplices.transform.GetChild(i).GetComponent<SpliceButton>());
                        }
                    }
                    for (int i = 0; i < MedSplices.transform.childCount; i++)
                    {
                        if (MedSplices.transform.GetChild(i).gameObject.name.Equals(SpliceButtonName(sm)))
                        {
                            ClickSpliceButton(MedSplices.transform.GetChild(i).GetComponent<SpliceButton>());
                        }
                    }
                    for (int i = 0; i < LargeSplices.transform.childCount; i++)
                    {
                        if (LargeSplices.transform.GetChild(i).gameObject.name.Equals(SpliceButtonName(sm)))
                        {
                            ClickSpliceButton(LargeSplices.transform.GetChild(i).GetComponent<SpliceButton>());
                        }
                    }
                }
            }
            gameObject.SetActive(true);
        }
        else
        {
            //Error openning the species!
        }
    }


}
