using UnityEngine;
using System.Collections;

using SpaceExploration.Types;

public enum PlanetViewType
{
    SpaceExView,
    RadarView
}
public class Planet : MonoBehaviour
{

    public PlanetViewType ViewType;

    //TODO: This should come from the server
    public Vector2 SpacePosition;
	
    public float DistanceToSpaceship = 1.0f;
	Vector2 PreviousSpaceshipPosition;

    public void UpdateSpaceshipPosition(Vector2 newPosition)
    {
		DistanceToSpaceship = (SpacePosition - newPosition).magnitude;
		Vector2 delta = PreviousSpaceshipPosition - newPosition;
		RenderUpdate(delta);
		PreviousSpaceshipPosition = newPosition;
    }
    // Use this for initialization
    void Start ()
    {
    }
    
    // Update is called once per frame
    void Update ()
    {
    }
    
    void RenderUpdate(Vector2 delta)
    {
// TODO: If this function gets too unwieldly then we should change this System
//       to an Interface based system instead of a simple swtich / case
//       this mixes view and model and is supposed to be bad :(
        switch (ViewType)
        {
//
        case PlanetViewType.SpaceExView:
            //TODO: If distance is less than or greater than something do something
			
			//TODO: THIS IS TERRIBLE
			transform.localPosition += new Vector3(delta.y, 0.0f, delta.x);
            transform.localScale = (Vector3.one * (200 / (2*DistanceToSpaceship)));
            break;

//
        case PlanetViewType.RadarView:
            transform.localScale = Vector3.one * 5.0f;
            break;
        }
    }
}