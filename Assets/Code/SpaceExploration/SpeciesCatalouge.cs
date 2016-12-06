using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeciesCatalouge : MonoBehaviour {

	VerticalLayoutGroup SpeciesGroup;

	void Start () {
	    
	}
	
	
	void Update () {

	}

    public void OnCLoseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
