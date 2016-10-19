using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

struct PlanetData
{
    public float x;
    public float y;
    public int id;
}

public class ExplorationConnection : MonoBehaviour
{
    public GameObject ship;
    public GameObject planet;
    public float locationUpdateInterval = 1.0f;

    List<PlanetData> toSpawn;
    List<PlanetData> toDepawn;

    List<GameObject> allPlanets;


    private WebSocket ws;
    void Awake()
    {
        toSpawn = new List<PlanetData>();
        allPlanets = new List<GameObject>();
    }

    void Start()
    {
        InitClient();

        InvokeRepeating("SendLocation", 0.5f, locationUpdateInterval);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("<requestplanets>[]");
        }

        while(toSpawn.Count > 0)
        {
            GameObject p = (GameObject)Instantiate(planet, new Vector3(toSpawn[0].x, 0, toSpawn[0].y), Quaternion.identity);
           // allPlanets[toSpawn[0].id] = p;
            toSpawn.RemoveAt(0);
        }
    }

    void InitClient()
    {
        //ws = new WebSocket("ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080");
        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += (sender, e) => OnMessage(e.Data);
        Debug.Log("Attempting Connect...");
        ws.Connect();

        string message = "<init>[";
        message += ship.transform.position.x.ToString();
        message += ",";
        message += ship.transform.position.z.ToString();
        message += "]";

        ws.Send(message);
    }

    void OnMessage(string message)
    {
        Debug.Log("GameServer: " + message);
        string flag = MsgFlag(message);
        if ( flag.Equals("spawn"))
        {

            string[] splitString = MsgLoad(message).Split(new string[] { "," }, StringSplitOptions.None);
            PlanetData pd;

            pd.x = float.Parse(splitString[0]);
            pd.y = float.Parse(splitString[1]);
            pd.id = int.Parse(splitString[2]);
            toSpawn.Add(pd);
        }
        else if(flag.Equals("place-holder"))
        {

        }

    }

    void SendLocation()
    {
        string message = "<mov>[";
        message += ship.transform.position.x.ToString();
        message += ",";
        message += ship.transform.position.z.ToString();
        message += "]";

        ws.Send(message);
    }

    void UpdatePlanets()
    {

    }

    string MsgFlag(string message)
    {
        string[] splitString = message.Split(new string[] { ">[" }, StringSplitOptions.None);
        string flag = splitString[0].Substring(1, splitString[0].Length - 1);
        return flag;
    }

    string MsgLoad(string message)
    {
        string[] splitString = message.Split(new string[] { ">[" }, StringSplitOptions.None);
        string load = splitString[1].Substring(0, splitString[1].Length - 1);
        Debug.Log(load);
        return load;
    }
}