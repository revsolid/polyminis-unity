using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{

	public float DistanceToSpaceShip = 1.0f;
	float Scale = 20.0f;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		float horImpulse = Input.GetAxis ("Horizontal");
		float verImpulse = Input.GetAxis ("Vertical");

		transform.RotateAround(Camera.main.transform.position, Vector3.up, -4 * horImpulse);
		transform.localScale = (Vector3.one * (20.0f * (100 / DistanceToSpaceShip)));

		DistanceToSpaceShip -= verImpulse;
	}
}
