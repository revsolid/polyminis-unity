using UnityEngine;
using System.Collections;

public class OrbitalUI : MonoBehaviour
{
	public Camera SpaceflightCamera;
	public Camera OrbitalCamera;

	// Use this for initialization
	void Start ()
	{ 
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void OnClick()
	{
        OrbitalCamera.gameObject.SetActive(false);
        SpaceflightCamera.gameObject.SetActive(true);
        SpaceflightCamera.enabled = true;
	}
}