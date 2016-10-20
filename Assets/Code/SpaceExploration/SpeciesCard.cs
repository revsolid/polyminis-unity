using UnityEngine;
using System.Collections;

public class SpeciesCard : MonoBehaviour {

    GameObject OrbitalUI;
	// Use this for initialization
	void Start () {
        OrbitalUI = GameObject.FindGameObjectWithTag("OrbitalUI");
	}

    public void OnClicked()
    {
        OrbitalUI.GetComponent<OrbitalUI>().OnEditCreatureClicked();
    }
}
