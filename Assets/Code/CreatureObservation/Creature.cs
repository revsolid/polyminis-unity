using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
	public Organelle OrganellePrototype;
	
	Dictionary<Vector2, int> OrganelleMap;
	
	void Awake()
	{
		OrganelleMap = new Dictionary <Vector2, int> ()
		{
			{ new Vector2(0,0), 114 },
			{ new Vector2(1,0), 201 },
			{ new Vector2(1,1), 85 },
			{ new Vector2(2,0), 182 },
		};
	}
	
	void Start ()
	{
		foreach(KeyValuePair<Vector2, int> entry in OrganelleMap)
		{
			Vector2 delta = entry.Key;	
			int colorOfset = entry.Value;
			
			Organelle o = GameObject.Instantiate(OrganellePrototype);
			o.transform.parent = transform;
			delta *= 2.5f;
			o.transform.localPosition += new Vector3(delta.x, 0.0f, delta.y);
			o.OrganelleModel = new OrganelleModel(colorOfset);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
