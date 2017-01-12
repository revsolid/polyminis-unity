using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    public SpaceExPlanetRenderer SpaceExRendererPrototype;
    public RadarRenderer RadarRendererPrototype;
    public StarmapRenderer StarmapRendererPrototype;
    public OrbitalApproachRenderer OrbitalApproachRendererPrototype;

    // Non-prototype
    public InOrbitRenderer InOrbitRenderer; 
		
    
    public SpaceMovementTracker MovementTracker;
    public Camera OrbitalCamera;
    public GameObject Starmap;
    Vector2 LastKnownPos;
    public IList<Planet> Planets { get; private set;}
    List<PlanetModel> ToSpawn;
    Camera MainCamera;
    bool DoWarp = false;
    Vector2 WarpDest = Vector2.zero;

//TODO: Planets should have a few Events in them (GotInRadarDistance, GotInOrbitDistance, ...) and 
// other systems should subscribe (or event the PlanetManager itself) that makes some renderers way
// easier to write and avoid having several of them in the scene
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

        orbAppRenderer.SetTargetCamera(OrbitalCamera);
        starmapRenderer.Starmap = this.Starmap;
        starmapRenderer.SetTargetCamera(OrbitalCamera);
        starmapRenderer.transform.parent = this.Starmap.transform;
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
        bool inOrbit = false;
        foreach (Planet planet in Planets)
        {
            planet.UpdateSpaceshipPosition(MovementTracker.CurrentPosition, MovementTracker.Forward);
            if (planet.InOrbitRange())
            {
                InOrbitRenderer.RenderUpdate(planet);
                inOrbit = true;
            }
        }
        if (!inOrbit)
        {
            InOrbitRenderer.gameObject.SetActive(false);
            InOrbitRenderer.Visible = false;
        }
        LastKnownPos = MovementTracker.CurrentPosition;

        // Check if any planet needs to be spawned
        while (ToSpawn.Count > 0)
        {
            SpawnNewPlanet(ToSpawn[0]);
            // allPlanets[ToSpawn[0].id] = p;
            ToSpawn.RemoveAt(0);
        }

        // Switch to orbital view
        if(DoWarp)
        {
            DoWarp = false;
            Camera.main.gameObject.SetActive(false);
            OrbitalCamera.gameObject.SetActive(true);
            OrbitalCamera.enabled = true;
            OrbitalCamera.GetComponentInChildren<OrbitalUI>().OnUIOpened(GetPlanetAt(WarpDest));
        }
    }

    // Create a new planet and add it to manager 
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
            case SpaceExplorationEventType.WARP:
                WarpDest = spaceExEvent.Position;
                DoWarp = true;
                break;
            }
        }
    }

    Planet GetPlanetAt(Vector2 inPos)
    {
        foreach (Planet p in Planets)
        {
            if(p.SpacePosition == inPos)
            {
                Debug.Log("Planet found");
                return p;
            }
        }
        Debug.Log("Planet Not Found");
        return Planets[0];
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