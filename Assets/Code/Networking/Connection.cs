using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;


public class Connection
{
	public delegate void MessageReceived(string message);
    public static event MessageReceived PlanetManagerMsg;
    public static event MessageReceived MovementTrackMsg;

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
        ws = new WebSocket("ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080");
        //ws = new WebSocket("ws://localhost:8080");
        Debug.Log("Initilizing");
        ws.OnMessage += (sender, e) => OnMessage(e.Data);
        ws.Connect();
    }

    void OnMessage(string message)
	{
		//Debug.Log("GameServer: " + message);
		string tag = MsgTag(message);
		if (tag.Equals("spawn"))
		{
            PlanetManagerMsg(message);
		}
		else if(tag.Equals("kickback"))
		{
            MovementTrackMsg(message);
		}

	}

    public void Send(string tag, string content)
    {
        if(ws != null)
        {
            ws.Send("<" + tag + ">[" + content + "]");
        }
    }

	public static string MsgTag(string message)
	{
		string[] splitString = message.Split(new string[] { ">[" }, StringSplitOptions.None);
		string flag = splitString[0].Substring(1, splitString[0].Length - 1);
		return flag;
	}

	public static string MsgLoad(string message)
	{
		string[] splitString = message.Split(new string[] { ">[" }, StringSplitOptions.None);
		string retVal = splitString[1].Substring(0, splitString[1].Length - 1);
		return retVal;
	}

    // only for messages in form of <tag>[float x, float y]
    public static Vector2 MsgLoc(string message)
    {
        string[] splitString = MsgLoad(message).Split(new string[] { "," }, StringSplitOptions.None);
        Vector2 retVal = new Vector2(float.Parse(splitString[0]), float.Parse(splitString[1]));

        return retVal;
    }

}