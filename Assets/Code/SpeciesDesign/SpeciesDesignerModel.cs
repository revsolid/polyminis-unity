using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesDesignerModel
{
    public List<SpliceModel> UnselectedSplices { get; private set; }
    public List<SpliceModel> SelectedSplices { get; private set; }

    public SpeciesModel CurrentSpecies { get; private set; }
    
    public SpeciesDesignerModel()
    {
        UnselectedSplices = new List<SpliceModel>();
        SelectedSplices = new List<SpliceModel>();
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
            foreach(SpliceModel unselected in UnselectedSplices)
            {
                // TODO: replace with ID
                if(sm.InternalName == unselected.InternalName)
                {
                    toUnselect.Add(unselected);
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
        UnselectedSplices.Add(model);
    }

    // from the splice collection (not helix)
    public void SelectSplice(SpliceModel model)
    {
        SelectedSplices.Add(model);
        UnselectedSplices.Remove(model);
        CurrentSpecies.Splices = SelectedSplices;
    }


    public void DeselectSplice(SpliceModel model)
    {
        UnselectedSplices.Add(model);
        SelectedSplices.Remove(model);
        CurrentSpecies.Splices = SelectedSplices;
    }

    // nuke the entire model
    public void Clear()
    {
        UnselectedSplices.Clear();
        SelectedSplices.Clear();
    }

}
