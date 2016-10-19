using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;

struct PlanetData
{
	public float x;
	public float y;
	public int id;
}


public class Connection
{
	//TODO: add different events
	public delegate void MessageReceived();
	public static event MessageReceived Planetupdate;

	private WebSocket ws;
	private List<PlanetData> toSpawn;
	 
	// TODO: change this to a connection singleton
	public static WebSocket Socket
	{
		get 
		{
			if (ws == null)
			{
				ws = new WebSocket("ws://ec2-54-70-6-182.us-west-2.compute.amazonaws.com:8080");
				ws.OnMessage += (sender, e) => OnMessage(e.Data);
				ws.Connect();
			}
			return ws;
		}
	}

	void OnMessage(string message)
	{
		Debug.Log("GameServer: " + message);
		string tag = MsgTag(message);
		if (tag.Equals("spawn"))
		{

			string[] splitString = MsgLoad(message).Split(new string[] { "," }, StringSplitOptions.None);
			PlanetData pd;

			pd.x = float.Parse(splitString[0]);
			pd.y = float.Parse(splitString[1]);
			pd.id = int.Parse(splitString[2]);
			toSpawn.Add(pd);
		}
		else if(tag.Equals("place-holder"))
		{

		}

	}


	void UpdatePlanets()
	{

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
		string load = splitString[1].Substring(0, splitString[1].Length - 1);
		Debug.Log(load);
		return load;
	}
}