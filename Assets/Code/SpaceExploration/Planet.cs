using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SpaceExploration.Types;

public class Planet
{
    public IList<IPlanetRenderer> Renderers;
    public float DistanceToSpaceship { get; private set; }
    public Vector2 LastDelta { get; private set; }
    public Vector2 Azimuth { get; private set; }
    public float RelativeAngle { get; private set; }
    public int ID { get; set; }

    //TODO: This should come from the server
    Vector2 SpacePosition;
    Vector2 PreviousSpaceshipPosition;
    public Range<float> Temperature { get; private set; }
    public Range<float> PH {get; private set;}
    public string PlanetName {get; private set;}
    private PlanetModel Model;
    
    

    public Planet(PlanetModel pm)
    {
        Model = pm;
        SpacePosition = Model.SpaceCoords;
        Renderers = new List<IPlanetRenderer>();
        ID = Model.ID;

        //TODO: This should come from the server
        Temperature = Model.Temperature;
        PH = Model.Ph;
        PlanetName = "Test Planet";
    }
    
    public void UpdateSpaceshipPosition(Vector2 newPosition, Vector2 newForward)
    {
        Azimuth  = (SpacePosition - newPosition);
        DistanceToSpaceship = Azimuth.magnitude;
        LastDelta = PreviousSpaceshipPosition - newPosition;
        
        RelativeAngle = Vector2.Angle(Azimuth, newForward);
        Vector3 cross = Vector3.Cross(Azimuth, newForward);
        if (cross.z > 0)
        {
            RelativeAngle *= -1; 
        }
        
        foreach (IPlanetRenderer renderer in Renderers)
        {
            renderer.RenderUpdate(this);
        }
        PreviousSpaceshipPosition = newPosition;
    }
}
