using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Connection : MonoBehaviour
{
    private WebSocket ws;
    void Awake()
    {
        Connect();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Ping the server :3");
            Ping();
        }
    }

    void Connect()
    {
        ws = new WebSocket("ws://localhost:9002");
        ws.Connect();
    }

    void Ping()
    {
        ws.Send("Hi server!");
    }
}