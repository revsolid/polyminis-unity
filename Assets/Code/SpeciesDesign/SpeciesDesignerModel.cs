using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesDesignerModel
{
    // these take internal names as keys
    public Dictionary<string, SpliceModel> UnselectedSplices { get; private set; }
    public Dictionary<string, SpliceModel> SelectedSplices { get; private set; }

    public SpeciesModel CurrentSpecies { get; private set; }
    
    public SpeciesDesignerModel()
    {
        UnselectedSplices = new Dictionary<string, SpliceModel>();
        SelectedSplices = new Dictionary<string, SpliceModel>();
        CurrentSpecies = new SpeciesModel();
    }
    
    public void Initialize ()
    {
        foreach (KeyValuePair<string, SpliceModel> entry in Almanac.Instance.AvailableSplices)
        {
            AddNewSplice(entry.Value);
        }
    }

    public void LoadSpecies(SpeciesModel species)
    {
        Clear();
        Initialize();
        List<SpliceModel> toUnselect = new List<SpliceModel>();
        
        if (species == null)
        {
            species = new SpeciesModel();
        }
        else
        {
            // Copy the species so we can operate on it and not change anything
            // until Save is pressed
            species = new SpeciesModel(species);
        }
        
        foreach(SpliceModel sm in species.Splices)
        {
            foreach(KeyValuePair<string, SpliceModel> unselected in UnselectedSplices)
            {
                // TODO: replace with ID
                if(sm.InternalName == unselected.Value.InternalName)
                {
                    toUnselect.Add(unselected.Value);
                }
            }
        }

        foreach(SpliceModel sm in toUnselect)
        {
            SelectSplice(sm);
        }
        CurrentSpecies = species;
    }

    public void AddNewSplice(SpliceModel model)
    {
        UnselectedSplices.Add(model.InternalName, model);
    }

    // from the splice collection (not helix)
    public void SelectSplice(SpliceModel model)
    {
        SelectedSplices.Add(model.InternalName, model);
        UnselectedSplices.Remove(model.InternalName);
        CurrentSpecies.Splices.Clear();
        foreach(KeyValuePair<string, SpliceModel> modelPair in SelectedSplices)
        {
            CurrentSpecies.Splices.Add(modelPair.Value);
        }
    }


    public void DeselectSplice(SpliceModel model)
    {
        UnselectedSplices.Add(model.InternalName, model);
        SelectedSplices.Remove(model.InternalName);
        CurrentSpecies.Splices.Clear();
        foreach (KeyValuePair<string, SpliceModel> modelPair in SelectedSplices)
        {
            CurrentSpecies.Splices.Add(modelPair.Value);
        }
    }

    // nuke the entire model
    public void Clear()
    {
        UnselectedSplices.Clear();
        SelectedSplices.Clear();
    }

}
