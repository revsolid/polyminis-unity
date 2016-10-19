using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    public SpaceExPlanetRenderer SpaceExRendererPrototype;
	public OrbitalApproachRenderer OrbitalApproachRendererPrototype;
	public RadarRenderer RadarRendererPrototype;
	
	public SpaceMovementTracker MovementTracker;
	public Camera OrbitalCamera;
	Vector2 LastKnownPos;
	IList<Planet> Planets;

	// Use this for initialization
	void Start ()
	{	
		//TODO: Normalize the Vector3 vs Vector2 situation before it is catastrophic
		Planets = new List<Planet>();	
		LastKnownPos = MovementTracker.CurrentPosition;
		
		Planets.Add(CreatePlanetAt(new Vector2( 100, 0)));
	//	Planets.Add(CreatePlanetAt(new Vector2(-100, 0)));
	}
	
	Planet CreatePlanetAt(Vector2 position)
	{
		Planet p1 = new Planet(position);
		SpaceExPlanetRenderer spaceExRenderer = GameObject.Instantiate(SpaceExRendererPrototype);
		OrbitalApproachRenderer orbAppRenderer = GameObject.Instantiate(OrbitalApproachRendererPrototype);
		RadarRenderer radarRenderer = GameObject.Instantiate(RadarRendererPrototype);
		
		orbAppRenderer.SetTargetCamera(OrbitalCamera);
		spaceExRenderer.gameObject.transform.parent = gameObject.transform;
		spaceExRenderer.gameObject.transform.localPosition = new Vector3(-1*position.y,0,position.x);
		p1.Renderers.Add(spaceExRenderer);
		p1.Renderers.Add(orbAppRenderer);
		p1.Renderers.Add(radarRenderer);
		return p1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//	TODO: This should be using Time.deltaTime instead of raw values as frame rate fucks up the pacing entirely
		transform.localEulerAngles = new Vector3(0.0f, -1*MovementTracker.Heading, 0.0f);
		foreach (Planet planet in Planets)
		{
			planet.UpdateSpaceshipPosition(MovementTracker.CurrentPosition, MovementTracker.Forward);
		}
		LastKnownPos = MovementTracker.CurrentPosition;
	}

	// send current location to server (attempt move)
	void SendLocation()
	{
		Connection.Socket.Send ();
	}
}
