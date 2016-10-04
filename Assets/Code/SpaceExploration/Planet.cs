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
	public float DistanceToSpaceShip = 1.0f;

	//TODO: This should come from the server
	Vector2 SpacePosition;

	public void UpdateSpaceshipPosition(Vector2 newPosition)
	{
		DistanceToSpaceShip = (SpacePosition - newPosition).magnitude;
	}
	// Use this for initialization
	void Start ()
	{
		SpacePosition = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{

		float horImpulse = Input.GetAxis ("Horizontal");
		float verImpulse = Input.GetAxis ("Vertical");
		
		RenderUpdate(horImpulse, verImpulse);
	}
	
	void RenderUpdate(float horImpulse, float verImpulse)
	{
// TODO: If this function gets too unwieldly then we should change this System
//       to an Interface based system instead of a simple swtich / case
//       this mixes view and model and is supposed to be bad :(
		switch (ViewType)
		{
//
		case PlanetViewType.SpaceExView:
			//TODO: If distance is less than or greater than something do something
			transform.RotateAround(Camera.main.transform.position, Vector3.up, -4 * horImpulse);
			transform.localScale = (Vector3.one * (200 / DistanceToSpaceShip)));
			break;

//
		case PlanetViewType.RadarView:
			transform.localScale = Vector3.one * 5.0f;
			break;
		}
	}
}