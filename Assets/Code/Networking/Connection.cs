using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;


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
        // the url to sisnett's Amazon EC2 linux box
        //TODO: Make this configurable
        //ws = new WebSocket("ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080");
        string address = "ws://localhost:8080";
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
            Debug.Log(content);
            ws.Send(content);
        }
    }
    public void CloseConnection()
    {
        ws.Close();
        instance = null;
    }
}