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

    //TODO: This should come from the server
    Vector2 SpacePosition;
    Vector2 PreviousSpaceshipPosition;
    
    public Planet(Vector2 spacePosition)
    {
        SpacePosition = spacePosition;
        Renderers = new List<IPlanetRenderer>();
    }
    
    public void UpdateSpaceshipPosition(Vector2 newPosition, Vector2 newForward)
    {
        Azimuth  = (SpacePosition - newPosition);
        DistanceToSpaceship = Azimuth.magnitude;
        LastDelta = PreviousSpaceshipPosition - newPosition;
        Debug.Log("XXXXXXX");
        Debug.Log("SHip Position: " +  newPosition);
        Debug.Log("Planet Position: "+SpacePosition);
        
        RelativeAngle = Vector2.Angle(Azimuth, newForward);
        Debug.Log("Azimuth: "+ Azimuth);
        Debug.Log("Relative Angle: "+RelativeAngle);
        Debug.Log("Right: "+Vector2.right);
        
        foreach (IPlanetRenderer renderer in Renderers)
        {
            renderer.RenderUpdate(this);
        }
        PreviousSpaceshipPosition = newPosition;
    }
}
