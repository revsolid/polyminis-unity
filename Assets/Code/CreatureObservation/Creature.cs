﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
	public Organelle OrganellePrototype;
	
	// TODO - This should be a proper data-driven table	
	public Organelle OrganellePrototype2;
	

	public Nucleus NucleusPrototype;
	public CreatureMover Mover;
	public int SpeciesIndex = 0;
	public int ID;
	public bool Alive;
	
	public Text DebugText;
	
    public delegate void CreatureClicked(Creature creature);
    public static event CreatureClicked OnCreatureClickedEvent;
	
	void Awake()
	{
	}
	
	public void AddStep(IndividualStep step)
	{
		if (Mover != null)
		{
			Mover.AddStep(step.Physics);
		}
		Alive = step.Alive;
	}
	
	public void SetDataFromModel(IndividualModel model)
	{
		ID = model.ID;		

		foreach(OrganelleModel organelle in model.Morphology.Body)
		{
			Vector2 delta = organelle.Coord;	
			
			if (delta == new Vector2(0, 0))
			{
				Nucleus n = GameObject.Instantiate(NucleusPrototype);
				n.transform.SetParent(transform);
				n.NucleusModel = new NucleusModel(0);
			}
			else
			{
				Organelle o = organelle.Trait.TID <= 5 ? GameObject.Instantiate(OrganellePrototype2) : GameObject.Instantiate(OrganellePrototype);
				o.transform.SetParent(transform);
				delta *= 2.5f;
				o.transform.localPosition += new Vector3(delta.x, 0.0f, delta.y);
				o.SpeciesIndex = SpeciesIndex;
				o.OrganelleModel = organelle;
			}
		}
		Mover.SetDataFromModel(model);
		DebugText.text = "ID: " + ID + "\n" + "Remaining Steps: "+Mover.GetRemainingSteps();
		
	}
	
	public void SetStartingPosition(Vector2 v)
	{
		Mover.SetInitialPosition(v);
	}
	
	public void OnMouseDown()
	{
		Debug.Log("Click");
		if (OnCreatureClickedEvent != null)
		{
			OnCreatureClickedEvent(this);
			Connection.OnMessageEvent += (msg) => { Debug.Log("I can't even"); };
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		DebugText.text = "ID: " + ID + "\n" + "Remaining Steps: "+Mover.GetRemainingSteps();
	}
}
