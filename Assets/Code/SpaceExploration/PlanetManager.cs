using UnityEngine;
using System;
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
    List<PlanetModel> toSpawn;

    void Awake()
    {
        Connection.PlanetManagerMsg += (message) => OnServerMessage(message);
    }

    // Use this for initialization
    void Start ()
	{	
		//TODO: Normalize the Vector3 vs Vector2 situation before it is catastrophic
		Planets = new List<Planet>();
        toSpawn = new List<PlanetModel>();
		LastKnownPos = MovementTracker.CurrentPosition;

	}
	
    // This shouldn't be called directly
	Planet CreatePlanetAt(Vector2 position, int inID)
	{
        PlanetModel pm = new PlanetModel();
        pm.SpaceCoords = position;
        pm.ID = inID;
		Planet p1 = new Planet(pm);
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

        // Check if any planet needs to be spawned
        while (toSpawn.Count > 0)
        {
            SpawnNewPlanet(toSpawn[0]);
            // allPlanets[toSpawn[0].id] = p;
            toSpawn.RemoveAt(0);
        }
    }

	// Create a new planet and add it to planet
	void SpawnNewPlanet(PlanetModel p)
	{
        Planets.Add(CreatePlanetAt(p.SpaceCoords, p.ID));
    }

    void OnServerMessage(string message)
    {
        if(Connection.MsgTag(message).Equals("spawn"))
        {
            string[] splitString = Connection.MsgLoad(message).Split(new string[] { "," }, StringSplitOptions.None);
            if (!hasSpawnedPlanet(int.Parse(splitString[2])))
            {
                PlanetModel newPlanet = new PlanetModel();
                newPlanet.SpaceCoords = new Vector2(float.Parse(splitString[0]), float.Parse(splitString[1]));
                newPlanet.ID = int.Parse(splitString[2]);
                toSpawn.Add(newPlanet);
            }
        }
    }

    bool hasSpawnedPlanet(int inID)
    {
        foreach(Planet p in Planets)
        {
            if(p.ID == inID)
            {
                return true;
            }
        }
        return false;
    }
}
