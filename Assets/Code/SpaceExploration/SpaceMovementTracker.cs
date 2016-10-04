using UnityEngine;
using System.Collections;
using SpaceExploration.Types;

public class SpaceMovementTracker : MonoBehaviour
{
    public GameObject SpaceSphere1;
    public GameObject SpaceSphere2;
    public GameObject HUD;
    
    public Vector2 CurrentPosition { get; private set; }
    float Heading;
    


    // Use this for initialization
    void Start ()
    {
        CurrentPosition = Vector2.zero; 
        Heading = 0;
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
		
		Heading += (4.0f*horImpulse);
		Heading %= 360;
        
        Vector2 forward = new Vector2(Mathf.Cos(Mathf.PI * Heading / 180),
									  Mathf.Sin(Mathf.PI * Heading / 180));

		CurrentPosition += (forward * verImpulse);

		Debug.Log("********************");
    	Debug.Log(forward);						
		Debug.Log(Heading);
		Debug.Log(CurrentPosition);
		Debug.Log("********************");
    }
}