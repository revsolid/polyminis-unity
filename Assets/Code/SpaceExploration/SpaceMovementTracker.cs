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
    public Camera SpaceFlightCamera;
    
    bool HasMoved = true;
    float LastImpulse = 0.0f;

    
    void Awake()
    {
        Connection.Instance.OnMessageEvent += OnServerMessage;
    }

    // Use this for initialization
    void Start ()
    {
        CurrentPosition = Session.Instance.LastKnownPosition;
        Heading = 0;

        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.INIT, CurrentPosition);
        Debug.Log(JsonUtility.ToJson(spaceExCommand));
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));//"init", CurrentPosition.x.ToString() + "," + CurrentPosition.y.ToString());
        InvokeRepeating("SendLocationIR", 0.0f, 1.0f);
        InvokeRepeating("SaveLocation", 10.0f, 10.0f);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float horImpulse = ControlHelper.GetHorizontalAxis();
        float verImpulse = ControlHelper.GetVerticalAxis();

        SpaceSphere1.transform.Rotate(new Vector3(-1*verImpulse * 0.01f, horImpulse * 0.1f, 0.0f));
        SpaceFlightCamera.transform.localEulerAngles = new Vector3(0.0f, Heading, 0.0f);
        float tDamp = Mathf.Max(TranslationSpeedMult, 1/15.0f); //TODO: This should be delta time or something derived instead of a static value 
        float rDamp = Mathf.Max(RotationSpeedMult, 0.001f);

        if (horImpulse == 0.0f && verImpulse == 0.0f)
        {
            HasMoved = false;
        }
        else
        {
            HasMoved = true;
        }
        // Rotate the Hud a bit.
        
        if (horImpulse != 0.0f && verImpulse == 0.0f) 
        {
            verImpulse = 0.1f;
        }

        HUD.transform.eulerAngles = new Vector3(0.0f, 0.0f, -1*horImpulse);
        
        Heading += (16.0f*rDamp*horImpulse);
        Heading %= 360;
        
        Forward = new Vector2(Mathf.Cos(Mathf.PI * Heading / 180),
                              Mathf.Sin(Mathf.PI * Heading / 180));


        CurrentPosition += (Forward * verImpulse * tDamp);
        
        if (LastImpulse != verImpulse)
        {
            Debug.Log(LastImpulse);
            LastImpulse = Mathf.Min(Mathf.Max(verImpulse, LastImpulse - 0.005f), LastImpulse + 0.005f);
            AkSoundEngine.SetRTPCValue("Spaceship_Speed", LastImpulse * 85, gameObject);
        }
    }


    // SendLocation Invoke Repeating
    void SendLocationIR()
    {
        SendLocation(false);
    }
    
    // send current location to server (attempt move)
    void SendLocation(bool force)
    {   
        if (!HasMoved && !force) 
            return;
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.ATTEMPT_MOVE, CurrentPosition);
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
    }
    
    void SaveLocation()
    {    
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.SAVE_POSITION, CurrentPosition);
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
    }


    void Warp (Vector2 dest)
    {
        CurrentPosition = dest;
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
            case SpaceExplorationEventType.WARP:
                Debug.Log("Warp to <" + spaceExEvent.Position.x.ToString() + ", " + spaceExEvent.Position.y.ToString() + ">");
                Warp(spaceExEvent.Position);
                break;
            }
        }
    }
    
    void OnDestroy()
    {
        Connection.Instance.OnMessageEvent -= OnServerMessage;
    }
}