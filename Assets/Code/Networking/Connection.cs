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
    private static int KK;
    
    public static Connection Instance
    {
        get 
        {
            if (instance == null)
            {
                Debug.Log("Creating New Instance of Connection");
                instance = new Connection();
            }
            return instance;
        }
    }
    public string Address { get; private set; }


    public Connection()
    {
        // the url to sisnett's Amazon EC2 linux box
        //TODO: Make this configurable
        Address = "ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080";
        //Address = "ws://localhost:8080";
        ws = new WebSocket(Address);
        Debug.Log("Initilizing Connection to: " + Address);
        ws.OnMessage += (sender, e) => OnMessage(e.Data);
        ws.Connect();
         
    }


    void OnMessage(string message)
    {
        Debug.Log("MESSAGE! "+KK);
        OnMessageEvent(message);
        Debug.Log(this);
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
        OnMessageEvent = null;
        Debug.Log("CLOSING CONNECTION!");
    }
}
