﻿using UnityEngine;
using System.Collections;

//TMP
public class OrganelleModel
{
	public int OrganelleId;	
	
	public OrganelleModel(int id)
	{
		OrganelleId = id;
	}
}

public class Organelle : MonoBehaviour
{
	
	public GameObject Sphere;
	public OrganelleModel OrganelleModel;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Renderer rend = Sphere.GetComponent<Renderer>();
		// TMP
        rend.material.SetColor("_Color", new Color(OrganelleModel.OrganelleId / 255.0f, 0, 0));
	}
}