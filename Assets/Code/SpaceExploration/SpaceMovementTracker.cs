using UnityEngine;
using System.Collections;
using SpaceExploration.Types;

public class SpaceMovementTracker : MonoBehaviour
{
    public GameObject SpaceSphere1;
    public GameObject HUD;
    
    public Vector2 CurrentPosition { get; private set; }
    public float Heading {get; private set; }
    public Vector2 Forward {get; private set; }
    public float TranslationSpeedMult = 0.0f;
    public float RotationSpeedMult = 0.0f;

    
    void Awake()
    {
        Connection.OnMessageEvent += (message) => OnServerMessage(message);
    }

    // Use this for initialization
    void Start ()
    {
        CurrentPosition = Vector2.zero; 
        Heading = 0;

        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.INIT, CurrentPosition);
        Debug.Log(JsonUtility.ToJson(spaceExCommand));
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));//"init", CurrentPosition.x.ToString() + "," + CurrentPosition.y.ToString());
        InvokeRepeating("SendLocation", 0.5f, 0.2f);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float horImpulse = Input.GetAxis ("Horizontal");
        float verImpulse = Input.GetAxis ("Vertical");
        SpaceSphere1.transform.Rotate(new Vector3(-1*verImpulse * 0.01f, horImpulse * 0.1f, 0.0f));
        float tDamp = Mathf.Max(TranslationSpeedMult, 1/15.0f); //TODO: This should be delta time or something derived instead of a static value 
        float rDamp = Mathf.Max(RotationSpeedMult, 0.07f);

        // Rotate the Hud a bit.
        HUD.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1*horImpulse);
        
        Heading += (16.0f*rDamp*horImpulse);
        Heading %= 360;
        
        Forward = new Vector2(Mathf.Cos(Mathf.PI * Heading / 180),
                              Mathf.Sin(Mathf.PI * Heading / 180));


        CurrentPosition += (Forward * verImpulse * tDamp);
    }


    // send current location to server (attempt move)
    void SendLocation()
    {    
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.ATTEMPT_MOVE, CurrentPosition);
        //Debug.Log(JsonUtility.ToJson(spaceExCommand));
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));//("mov", CurrentPosition.x.ToString() + "," + CurrentPosition.y.ToString());
    }

    void OnServerMessage(string message)
    {
        SpaceExplorationEvent spaceExEvent = JsonUtility.FromJson<SpaceExplorationEvent>(message);
        if (spaceExEvent != null)
        {
            switch (spaceExEvent.EventType)
            {
            case SpaceExplorationEventType.KICK_BACK:
                //TODO: Make this do actual stuff
                Debug.Log("KICK_BACK");
                break;
            }
        }
    }

}