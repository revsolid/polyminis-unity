using UnityEngine;
using System.Collections;

public class SpeciesCard : MonoBehaviour
{

    public OrbitalUI OrbitalUI;
    public GameObject EmptyState;
    public SpeciesSlot FilledState;
    public SpeciesModel Species = null;
    
    bool Empty = true; 
    
    
    
    // Use this for initialization
    void Start ()
    {
        FilledState.gameObject.SetActive(false);
        EmptyState.SetActive(true);
    }
    
    void Update()
    {
        if (Species != null)
        {
            FilledState.gameObject.SetActive(true);
            FilledState.Species = Species;
            EmptyState.SetActive(false);
            Empty = false;
        }
        
        if (Species == null && !Empty)
        {
            FilledState.gameObject.SetActive(false);
            EmptyState.SetActive(true);
            Empty = true;
        }
    }
    
    public void OnClicked()
    {
        if (!Empty)
        {
            if (Species.CreatorName == Session.Instance.UserName)
            {
                OrbitalUI.OnEditCreatureClicked(Species);
            }
            else
            {
                OrbitalUI.OnResearchCreatureClicked(Species.SpeciesName);
            }
        }
        else
        {
            OrbitalUI.OnDeployCreatureClicked();
        }
    }
    
    public void OnExtractButton()
    {
        OrbitalUI.OnExtractCreatureClicked(Species);
    }
    public void OnSampleButton()
    {
        OrbitalUI.OnSampleCreatureClicked(Species);
    }
}
