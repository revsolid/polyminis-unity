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

        UpdateAllViews();
    }
    
    // read DesignerModel and update all children
    void UpdateAllViews()
    {
        InstinctsTunner.UpdateView(DesignerModel);
        Helix.UpdateView(DesignerModel);
        this.UpdateView();
        DnaSequencer.ActivateSelection(DesignerModel.CurrentSpecies);
    }


    void UpdateLayoutGroup(SpeciesDesignerModel model, LayoutGroup group, TraitSize size)
    {
        for (int i = 0; i < group.transform.childCount; i++)
        {
            GameObject button = group.transform.GetChild(i).gameObject;
            SpliceModel alreadyIn = button.GetComponent<SpliceButton>().Model;
            // check against unselected list. if it's not in there anymore then kick it.
            bool isStillIn = false;
            foreach (SpliceModel sm in model.UnselectedSplices)
            {
                if (sm.InternalName == alreadyIn.InternalName)
                {
                    isStillIn = true;
                }
            }
            if (!isStillIn)
            {
                Destroy(button);
            }
        }

        // then check the unselected list to see if any new ones need to be instantiated
        foreach (SpliceModel sm in model.UnselectedSplices)
        {
            bool found = false;
            for (int i = 0; i < group.transform.childCount; i++)
            {
                GameObject button = group.transform.GetChild(i).gameObject;
                SpliceModel alreadyIn = button.GetComponent<SpliceButton>().Model;

                if (alreadyIn.InternalName == sm.InternalName)
                {
                    found = true;
                }
            }

            if (!found && sm.TraitSize == size)
            {
                AddSplice(sm);
            }
        }
    }

    void UpdateView()
    {
        UpdateLayoutGroup(DesignerModel, SmallSplices, TraitSize.SMALL);
        UpdateLayoutGroup(DesignerModel, MedSplices, TraitSize.MEDIUM);
        UpdateLayoutGroup(DesignerModel, LargeSplices, TraitSize.LARGE);
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
            UpdateAllViews();
        }
    }
    
    void OnSpliceRemovedFromHelix(SpliceModel model)
    {
        DesignerModel.DeselectSplice(model);
        UpdateAllViews();
    }
    
    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }
    
    public void OnSaveButtonClicked()
    {
        // Validate
        
        SpeciesModel newModel = new SpeciesModel(DesignerModel.CurrentSpecies);
        Session.Instance.Species[name] = newModel;
        // Serialize Species
        Debug.Log(JsonUtility.ToJson(DesignerModel.CurrentSpecies));
        
        // Send to Server
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
            UpdateAllViews();
            NameInput.text = DesignerModel.CurrentSpecies.Name;
            gameObject.SetActive(true);
        }
        else
        {
            //Error openning the species!
        }
    }

}



