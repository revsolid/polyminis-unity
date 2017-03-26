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
    
    public delegate void SaveClicked(SpeciesModel result);
    public static event SaveClicked OnSaveEvent;

    public delegate void OpenSpeciesDesignerAction();
    public static event OpenSpeciesDesignerAction OnOpenSpeciesDesigner;

    SpliceButton.ButtonClicked SpliceButtonClickedHandler;
    DnaHelix.SpliceRemoved SpliceRemovedFromHelixHandler;
    
    SpeciesDesignerModel DesignerModel = new SpeciesDesignerModel();

    void Start()
    {
        Initialize();
        OrbitalUI.OnGoBackToSpaceExScreen += OnExitButtonClicked;
        OrbitalApproachRenderer.OnToOrbitScreen += OnExitButtonClicked;
        InventoryUI.OnOpenInventory += OnExitButtonClicked;
    }

    void OnDestroy()
    {
        SpliceButton.OnClickEvent -= SpliceButtonClickedHandler;
        DnaHelix.OnSpliceRemovedEvent -= SpliceRemovedFromHelixHandler;
        OrbitalUI.OnGoBackToSpaceExScreen -= OnExitButtonClicked;
        OrbitalApproachRenderer.OnToOrbitScreen -= OnExitButtonClicked;
        InventoryUI.OnOpenInventory -= OnExitButtonClicked;

    }

    void OnEnable()
    {
        OnOpenSpeciesDesigner();
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
            SpeciesDesignUI.SColorConfig = this.ColorConfig;
        }

        SpliceButtonClickedHandler = (button) => OnSpliceButtonClicked(button);
        SpliceRemovedFromHelixHandler = (model) => OnSpliceRemovedFromHelix(model);

        SpliceButton.OnClickEvent += SpliceButtonClickedHandler;
        DnaHelix.OnSpliceRemovedEvent += SpliceRemovedFromHelixHandler;
        UpdateAllViews();
    }

    
    // read DesignerModel and update all children
    void UpdateAllViews()
    {
   //     InstinctsTunner.UpdateView(DesignerModel);
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
        OnSaveEvent = null;
    }
    
    public void OnSaveButtonClicked()
    {
        // Validate
        
        string nsName = NameInput.text;
        DesignerModel.CurrentSpecies.SpeciesName = NameInput.text; 
        
        SpeciesModel newModel = new SpeciesModel(DesignerModel.CurrentSpecies);
        newModel.InstinctTuning = InstinctsTunner.ToModel ();
        if (OnSaveEvent != null)
            OnSaveEvent(newModel);
        Debug.Log(JsonUtility.ToJson(DesignerModel.CurrentSpecies));
            
        OnExitButtonClicked();
    }

    public void OpenWithSpecies(SpeciesModel m)
    {
        DesignerModel.LoadSpecies(m);
        UpdateAllViews();

        InstinctTuningModel itmodel = null;
        if(m!= null && m.InstinctTuning != null)
        {
            itmodel = m.InstinctTuning;
        }
        InstinctsTunner.LoadModel(itmodel);

        NameInput.text = DesignerModel.CurrentSpecies.SpeciesName;
        gameObject.SetActive(true);
    }

}



