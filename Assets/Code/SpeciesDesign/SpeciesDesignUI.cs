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

    SpeciesDesignerModel DesignerModel;

    void Start()
    {
        DesignerModel = new SpeciesDesignerModel();
        Initialize();
    }

    // Previously named Reset but I think Initialize fits the purpose better
    void Initialize() 
    {
        gameObject.SetActive(false);
        DesignerModel.Initialize();
        InstinctsTunner.Initialize();
        Helix.Initialize();
        DnaSequencer.ActivateSelection(DesignerModel.CurrentSpecies);

        if (SpeciesDesignUI.SColorConfig == null)
        {
            Debug.Log("Setting static");
            SpeciesDesignUI.SColorConfig = this.ColorConfig;
        }
        SpliceButton.OnClickEvent += (button) => OnSpliceButtonClicked(button);
        DnaHelix.OnSpliceRemovedEvent += (model) => OnSpliceRemovedFromHelix(model);

        UpdateView();
    }
    
    // read DesignerModel and update all children
    void UpdateView()
    {
        InstinctsTunner.UpdateView(DesignerModel);
        Helix.UpdateView(DesignerModel);
        UpdateLayoutGroups();
        DnaSequencer.ActivateSelection(DesignerModel.CurrentSpecies);
    }

    void UpdateLayoutGroups()
    {
        ResetLayoutGroups();
        foreach(SpliceModel sm in DesignerModel.UnselectedSplices)
        {
            AddSplice(sm);
        }
    }

    void ResetLayoutGroup(LayoutGroup lg)
    {
        var children = new List<GameObject>();
        foreach (Transform child in lg.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    void ResetLayoutGroups()
    {
        ResetLayoutGroup(SmallSplices);
        ResetLayoutGroup(MedSplices);
        ResetLayoutGroup(LargeSplices);
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
        //    sbutton.transform.localPosition = VecVector3tor3.zero;
        //    sbutton.transform.localScale = .one;
        sbutton.transform.SetAsFirstSibling();
    }

    // so that buttons have different names
    string SpliceButtonName(SpliceModel model)
    {
        return "SpliceButtonRenderer-" + model.InternalName;
    }


    bool ValidateSelection(SpliceModel model)
    {
        // 4 Small, 2 Med, 1 Large
        var small = DesignerModel.CurrentSpecies.Splices.Where( x => x.TraitSize == TraitSize.SMALL).Count();
        var med = DesignerModel.CurrentSpecies.Splices.Where( x => x.TraitSize == TraitSize.MEDIUM).Count();
        var large = DesignerModel.CurrentSpecies.Splices.Where( x => x.TraitSize == TraitSize.LARGE).Count(); 

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
        SelectSplice(button.Model);
    }

    void SelectSplice(SpliceModel model)
    {
        if (ValidateSelection(model))
        {
            DesignerModel.SelectSplice(model);
            UpdateView();
        }
    }
    
    void OnSpliceRemovedFromHelix(SpliceModel model)
    {
        DesignerModel.DeselectSplice(model);
        UpdateView();
    }
    
    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }
    
    public void OnSaveButtonClicked()
    {
        // Validate
        
        /*
        SpeciesModel newModel = new SpeciesModel();
        CurrentSelection.Name = NameInput.text;
        newModel.Name = CurrentSelection.Name;
        newModel.Splices = CurrentSelection.Splices; //TODO: Thourough clone
        Session.Instance.Species[name] = newModel;
        // Serialize Species
        Debug.Log(JsonUtility.ToJson(CurrentSelection));
        
        // Send to Server
        */
    }

    // note: this always loads the selected species, since the one  
    // stored in designer model is(should be) a COPY not a REFERENCE.
    // so if you select a species on planet -> modify it in species designer -> select the species again:
    // your modification all get wiped.
    public void OpenWithSpecies(string name)
    {
        SpeciesModel m = Session.Instance.Species[name];

        if (m != null)
        {
            DesignerModel.LoadSpecies(m);
            UpdateView();
            gameObject.SetActive(true);
        }
        else
        {
            //Error openning the species!
        }
    }

}



