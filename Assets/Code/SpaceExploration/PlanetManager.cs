using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    public SpaceExPlanetRenderer SpaceExRendererPrototype;
    public OrbitalApproachRenderer OrbitalApproachRendererPrototype;
    public RadarRenderer RadarRendererPrototype;
    public StarmapRenderer StarmapRendererPrototype;
    
    public SpaceMovementTracker MovementTracker;
    public Camera OrbitalCamera;
    public GameObject Starmap;
    Vector2 LastKnownPos;
    public IList<Planet> Planets { get; private set;}
    List<PlanetModel> ToSpawn;

    void Awake()
    {
        Connection.OnMessageEvent += (message) => OnServerMessage(message);
    }

    // Use this for initialization
    void Start ()
    {    
        Planets = new List<Planet>();
        ToSpawn = new List<PlanetModel>();
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
        StarmapRenderer starmapRenderer = GameObject.Instantiate(StarmapRendererPrototype);
        starmapRenderer.Starmap = this.Starmap;

        orbAppRenderer.SetTargetCamera(OrbitalCamera);
        spaceExRenderer.gameObject.transform.parent = gameObject.transform;
        p1.Renderers.Add(spaceExRenderer);
        p1.Renderers.Add(orbAppRenderer);
        p1.Renderers.Add(radarRenderer);
        p1.Renderers.Add (starmapRenderer);
        return p1;
    }
    
    // Update is called once per frame
    void Update ()
    {
        //    TODO: This should be using Time.deltaTime instead of raw values as frame rate fucks up the pacing entirely
        transform.localEulerAngles = new Vector3(0.0f, -1*MovementTracker.Heading, 0.0f);
        foreach (Planet planet in Planets)
        {
            planet.UpdateSpaceshipPosition(MovementTracker.CurrentPosition, MovementTracker.Forward);
        }
        LastKnownPos = MovementTracker.CurrentPosition;

        // Check if any planet needs to be spawned
        while (ToSpawn.Count > 0)
        {
            SpawnNewPlanet(ToSpawn[0]);
            // allPlanets[ToSpawn[0].id] = p;
            ToSpawn.RemoveAt(0);
        }
    }

    // Create a new planet and add it to planet
    void SpawnNewPlanet(PlanetModel p)
    {
        Planets.Add(CreatePlanetAt(p.SpaceCoords, p.ID));
    }

    void OnServerMessage(string message)
    {
        SpaceExplorationEvent spaceExEvent = JsonUtility.FromJson<SpaceExplorationEvent>(message);
        if (spaceExEvent != null)
        {
            switch (spaceExEvent.EventType)
            {
            case SpaceExplorationEventType.SPAWN_PLANETS:
                for (var i = 0; i < spaceExEvent.Planets.Length; i++)
                {
                    PlanetModel newPlanet = spaceExEvent.Planets[i];
                    if (!HasSpawnedPlanet(newPlanet.ID))
                    {
                        ToSpawn.Add(newPlanet);
                    }
                }
                break;
            }
        }
    }

    bool HasSpawnedPlanet(int inID)
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