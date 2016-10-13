using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject StarMap;
	public GameObject SpeciesEditor;
	bool StarMapToggle = false;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void OnStarmapClick()
	{
		StarMapToggle = !StarMapToggle;
		StarMap.SetActive(StarMapToggle);
	}
	
	public void OnSpeciesEditorClick()
	{
		SpeciesEditor.SetActive(true);
	}
	
}
