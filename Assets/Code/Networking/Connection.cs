using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;


public class Connection
{
    public delegate void MessageReceived(string message);
    public static event MessageReceived OnMessageEvent;

    private static WebSocket ws;
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
        // the url to sisnett's Amazon EC2 linux box
        //TODO: Make this configurable
        //ws = new WebSocket("ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080");
        ws = new WebSocket("ws://localhost:8080");
        Debug.Log("Initilizing");
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
}