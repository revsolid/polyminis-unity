using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdStorage : MonoBehaviour
{
	bool Hidden = true;
	public GameObject Open;
	public Button Toggler;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void ToggleHidden()
	{
		Hidden = !Hidden; 
		Toggler.enabled = Hidden;
		Toggler.gameObject.SetActive(Hidden);
		Open.SetActive(!Hidden);
	}
}
