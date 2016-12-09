using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesDesignerModel
{
    public List<SpliceModel> UnselectedSplices { get; private set; }
    public List<SpliceModel> SelectedSplices { get; private set; }

    // TODO: implement this as a "copy", instead of referencing the actual species
    public SpeciesModel CurrentSpecies { get; private set; }
    
    public void Initialize ()
    {
        UnselectedSplices = new List<SpliceModel>();
        SelectedSplices = new List<SpliceModel>();
        CurrentSpecies = new SpeciesModel();

        foreach (KeyValuePair<string, SpliceModel> entry in Almanac.Instance.AvailableSplices)
        {
            AddNewSplice(entry.Value);
        }
    }

    public void LoadSpecies(SpeciesModel species)
    {
        // load from almanac...maybe there's a better way?
        Clear();
        Initialize();
        List<SpliceModel> toUnselect = new List<SpliceModel>();
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

        // TODO: clone
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
    }


    public void DeselectSplice(SpliceModel model)
    {
        UnselectedSplices.Add(model);
        SelectedSplices.Remove(model);
    }

    // nuke the entire model
    public void Clear()
    {
        UnselectedSplices.Clear();
        SelectedSplices.Clear();
    }

}
