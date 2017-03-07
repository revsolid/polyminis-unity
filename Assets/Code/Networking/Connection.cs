using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class Connection : Singleton<Connection>
{
    public delegate void MessageReceived(string message);
    public event MessageReceived OnMessageEvent;

    private WebSocket ws;
    
    //private static Connection instance;
    
    /*public static Connection Instance
    {
        get 
        {
            if (instance == null)
            {
                Debug.Log("Creating New Instance of Connection");
                instance = new Connection();
                instance.lilK = KK;
                KK += 1;

            }
            return instance;
        }
    }*/
    public string Address { get; private set; }


    public Connection()
    {
        // the url to sisnett's Amazon EC2 linux box
        //TODO: Make this configurable
        //Address = "ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080";
        Address = "ws://localhost:8080";
        ws = new WebSocket(Address);
        Debug.Log("Initilizing Connection to: " + Address);
        ws.OnMessage += (sender, e) => OnMessage(e.Data);
        ws.Connect();
        OnMessageEvent += (msg) => {};
    }


    void OnMessage(string message)
    {
        Debug.Log("MESSAGE! " + message.Length);
        OnMessageEvent(message);
    }

    public void Send(string content)
    {
        Debug.Log("about to sending...");
        if (ws != null)
        {
            Debug.Log("sending...");
            ws.Send(content);
        }
    }
    public void CloseConnection()
    {
        ws.Close();
  //      instance = null;
  //      OnMessageEvent = null;
        Debug.Log("CLOSING CONNECTION!");
    }
}
