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
        if (Species != null && Species.Name != "" && Empty)
        {
            FilledState.gameObject.SetActive(true);
            FilledState.Species = Species;
            EmptyState.SetActive(false);
            Empty = false;
        }
    }
    
    public void OnClicked()
    {
        if (!Empty)
        {
            if (Species.Creator == Session.Instance.UserName)
            {
                OrbitalUI.OnEditCreatureClicked(Species.Name);
            }
            else
            {
                OrbitalUI.OnResearchCreatureClicked(Species.Name);
            }
        }
        else
        {
            OrbitalUI.OnDeployCreatureClicked();
        }
    }
    
    public void OnExtractButton()
    {
        OrbitalUI.OnExtractCreatureClicked(Species.Name);
    }
    public void OnSampleButton()
    {
        OrbitalUI.OnSampleCreatureClicked(Species.Name);
    }
}
