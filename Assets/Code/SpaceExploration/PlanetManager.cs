using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
	
	public Planet PlanetPrototype;
	public SpaceMovementTracker MovementTracker;
	Vector2 LastKnownPos;
	IList<Planet> Planets;

	// Use this for initialization
	void Start ()
	{	
		//TODO: Normalize the Vector3 vs Vector2 situation before it is catastrophic
		Planets = new List<Planet>();	
		LastKnownPos = MovementTracker.CurrentPosition;
		
		Planet p1 = GameObject.Instantiate(PlanetPrototype);
		Planets.Add(p1);
		p1.gameObject.transform.parent = gameObject.transform;
		p1.gameObject.transform.localPosition = new Vector3(-0,0,100);
		p1.SpacePosition = new Vector2(100, 0);
		
		p1 = GameObject.Instantiate(PlanetPrototype);
		Planets.Add(p1);
		p1.gameObject.transform.parent = gameObject.transform;
		p1.gameObject.transform.localPosition = new Vector3(0,0, -100);
		p1.SpacePosition = new Vector2(-100, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//	TODO: This should be using Time.deltaTime instead of raw values as frame rate fucks up the pacing entirely
		transform.localEulerAngles = new Vector3(0.0f, -1*MovementTracker.Heading, 0.0f);
		foreach (Planet planet in Planets)
		{
			planet.UpdateSpaceshipPosition(MovementTracker.CurrentPosition);
		}
		LastKnownPos = MovementTracker.CurrentPosition;
	}
}