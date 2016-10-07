using UnityEngine;
using WebSocketSharp;

public class Connection : MonoBehaviour
{
    void Awake()
    {
        using (var ws = new WebSocket ("ws://echo.websocket.org"))
        {
            Debug.Log("XXXXX");
            ws.Connect();
        }
    }
}