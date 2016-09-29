using UnityEngine;
using System.Collections;

public class SpaceMovementTracker : MonoBehaviour
{
	public GameObject SpaceMaterial;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float horImpulse = Input.GetAxis ("Horizontal");
		float verImpulse = Input.GetAxis ("Vertical");

	}
}
