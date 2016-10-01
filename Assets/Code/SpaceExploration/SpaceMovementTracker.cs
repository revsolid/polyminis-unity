using UnityEngine;
using System.Collections;

public class SpaceMovementTracker : MonoBehaviour
{
	public GameObject SpaceSphere1;
	public GameObject SpaceSphere2;
	public GameObject HUD;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float horImpulse = Input.GetAxis ("Horizontal");
		float verImpulse = Input.GetAxis ("Vertical");
		SpaceSphere1.transform.Rotate(new Vector3(-1*verImpulse * 0.01f, horImpulse * 0.1f, 0.0f));
		SpaceSphere2.transform.Rotate(new Vector3(verImpulse * 0.1f, -1*horImpulse * 0.01f, 0.0f));

		// Rotate the Hud a bit.
		HUD.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1*horImpulse);

	}
}
