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
    {
    }
    
    void Update()
    {
    }
    
    public void Populate(IDictionary<string, SpeciesModel> species)
    {
        Debug.Log("Populating with " + species.Count);
        foreach(KeyValuePair<string, SpeciesModel> kvp in species) 
        {
            if (!Models.Contains(kvp.Value))
            {
                SpeciesCardRenderer speciesCard = GameObject.Instantiate<SpeciesCardRenderer>(SpeciesCardPrototype);
                speciesCard.Model = kvp.Value;
                speciesCard.transform.SetParent(LayoutToUse.transform);
                speciesCard.transform.localPosition = Vector3.zero;
    	        speciesCard.transform.localScale = Vector3.one;
    	        speciesCard.transform.SetAsFirstSibling();
                Models.Add(kvp.Value);
            }
            Debug.Log(kvp.Value.Name);
        }
        Debug.Log("Leaving with: " + Models.Count);
    }
}