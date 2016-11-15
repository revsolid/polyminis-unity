using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeciesCatalog : MonoBehaviour
{
    public SpeciesCardRenderer SpeciesCardPrototype; 
    public LayoutGroup LayoutToUse;
    IList<SpeciesModel> Models = new List<SpeciesModel>();
    
    void Start()
    {}
    
    void Update()
    {
    }
    
    public void Populate(IList<SpeciesModel> species)
    {
        Debug.Log("Populating with " + species.Count);
        foreach(SpeciesModel s in species) 
        {
            if (!Models.Contains(s))
            {
                SpeciesCardRenderer speciesCard = GameObject.Instantiate<SpeciesCardRenderer>(SpeciesCardPrototype);
                speciesCard.Model = s;
                speciesCard.transform.SetParent(LayoutToUse.transform);
                speciesCard.transform.localPosition = Vector3.zero;
    	        speciesCard.transform.localScale = Vector3.one;
    	        speciesCard.transform.SetAsFirstSibling();
                Models.Add(s);
            }
            Debug.Log(s.Name);
        }
        Debug.Log("Leaving with: " + Models.Count);
    }
}