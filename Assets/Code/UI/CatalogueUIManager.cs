using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CatalogueUIManager : MonoBehaviour {
    public GameObject MySpeciesPanel;
    public GameObject AllSpeciesPanel;
    public Button AllToggle;
    public Button MyToggle;
    
	// Use this for initialization
	void Start () {
        AllToggle.GetComponent<Button>();
        MyToggle.GetComponent<Button>();
        MySpeciesPanel.SetActive(false);
        AllSpeciesPanel.SetActive(true);
        AllToggle.onClick.AddListener(SeeAll);
        MyToggle.onClick.AddListener(SeeMy);
      
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SeeAll()
    {
        MySpeciesPanel.SetActive(false);
        AllSpeciesPanel.SetActive(true);
        Debug.Log("Viewing All Species");
    }
    void SeeMy()
    {
        MySpeciesPanel.SetActive(true);
        AllSpeciesPanel.SetActive(false);
        Debug.Log("Viewing My Species");
    }

    
}
