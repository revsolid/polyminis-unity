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
		Planets = new List<Planet>();	
		LastKnownPos = MovementTracker.CurrentPosition;
		
		Planet p1 = GameObject.Instantiate(PlanetPrototype);
		Planets.Add(p1);
		p1.gameObject.transform.localPosition = new Vector3(-140,0,-50);
		p1.DistanceToSpaceShip = 100;
		
		p1 = GameObject.Instantiate(PlanetPrototype);
		Planets.Add(p1);
		p1.gameObject.transform.localPosition = new Vector3(0,0,-50);
		p1.DistanceToSpaceShip = 200;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (LastKnownPos == MovementTracker.CurrentPosition)
		{
			return;
		}
		
		Vector2 delta = LastKnownPos - MovementTracker.CurrentPosition;
		
		foreach (Planet planet in Planets)
		{
			planet.UpdateSpaceshipPosition(MovementTracker.CurrentPosition);
		}
		LastKnownPos = MovementTracker.CurrentPosition;
	}
}