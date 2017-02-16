using UnityEngine;
using System.Collections;

public class SpeciesCard : MonoBehaviour
{

    public OrbitalUI OrbitalUI;
    public GameObject EmptyState;
    public GameObject FilledState;
    
    bool Empty = true; 
    
    SpeciesModel Species;
    
    
	// Use this for initialization
	void Start ()
    {
        FilledState.SetActive(false);
        EmptyState.SetActive(true);
	}
    
    public void OnClicked()
    {
        if (!Empty)
        {
            OrbitalUI.OnEditCreatureClicked("Cool dudes");
        }
        else
        {
        }
    }
}
