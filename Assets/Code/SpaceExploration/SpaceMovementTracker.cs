using UnityEngine;
using System.Collections;
using SpaceExploration.Types;

public class SpaceMovementTracker : MonoBehaviour
{
    public GameObject SpaceSphere1;
    public GameObject SpaceSphere2;
    public GameObject HUD;
    
    public Vector2 CurrentPosition { get; private set; }
    public float Heading {get; private set; }
    public Vector2 Forward {get; private set; }
    
    public float HackVel = 0.0f;
    
    void Awake()
    {
        Connection.PlanetManagerMsg += (message) => OnServerMessage(message);
    }

    // Use this for initialization
    void Start ()
    {
        CurrentPosition = Vector2.zero; 
        Heading = 0;

        Connection.Instance.Send("init", CurrentPosition.x.ToString() + "," + CurrentPosition.y.ToString());
        InvokeRepeating("SendLocation", 0.5f, 0.2f);
    }

    // Update is called once per frame
    void Update ()
    {
        float horImpulse = Input.GetAxis ("Horizontal");
        float verImpulse = Input.GetAxis ("Vertical");
        SpaceSphere1.transform.Rotate(new Vector3(-1*verImpulse * 0.01f, horImpulse * 0.1f, 0.0f));
        SpaceSphere2.transform.Rotate(new Vector3(verImpulse * 0.1f, horImpulse * 0.01f, 0.0f));
        float damp = Mathf.Max(HackVel, 1/15.0f); //TODO: This should be delta time or something derived instead of a static value 

        // Rotate the Hud a bit.
        HUD.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1*horImpulse);
        
        Heading += (16.0f*damp*horImpulse);
        Heading %= 360;
        
        Forward = new Vector2(Mathf.Cos(Mathf.PI * Heading / 180),
                              Mathf.Sin(Mathf.PI * Heading / 180));

        CurrentPosition += (Forward * verImpulse * damp);

    }


	// send current location to server (attempt move)
	void SendLocation()
	{	
        Connection.Instance.Send("mov", 
            CurrentPosition.x.ToString() + "," + CurrentPosition.y.ToString());
	}

    void OnServerMessage(string message)
    {
        if (Connection.MsgTag(message).Equals("kickback"))
        {
            CurrentPosition = Connection.MsgLoc(message);
        }
    }

}