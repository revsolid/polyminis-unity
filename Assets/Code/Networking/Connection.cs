using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ConnectionType
{
    LOCAL, 
    EC2
}


public class Connection
{
    public delegate void MessageReceived(string message);
    public static event MessageReceived OnMessageEvent;

    private WebSocket ws;
    private static Connection instance;

    public static Connection Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new Connection();
            }

            return instance;
        }
    }


    public Connection()
    {
        string address;

        switch (ConnectionType.LOCAL)
        {
        case ConnectionType.EC2:
            address = "ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080";
            break;
        default:
            address = "ws://localhost:8080";
            break;
        }
        ws = new WebSocket(address);
        Debug.Log("Initilizing Connection to: " + address);
        ws.OnMessage += (sender, e) => OnMessage(e.Data);
        ws.Connect();
    }


    void OnMessage(string message)
    {
        OnMessageEvent(message);
    }

    public void Send(string content)
    {
        if (ws != null)
        {
            ws.Send(content);
        }
    }
    public void CloseConnection()
    {
        ws.Close();
        instance = null;
    }
}