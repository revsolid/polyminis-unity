using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ExperimentManager : MonoBehaviour {

//TODO: This is hacky AF
//	public List<Button> Buttons;
//	public List<string> ExperimentNames; 
	public SpeciesController Controller;
	
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void OnLoadExp(string expname)
	{
		Controller.LoadExperiment(expname);
	}
}
