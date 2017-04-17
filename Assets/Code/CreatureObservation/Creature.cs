using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
	public Organelle OrganellePrototype;
	
	// TODO - This should be a proper data-driven table	
	public Actuator OrganellePrototype2;
	

	public Nucleus NucleusPrototype;
	public CreatureMover Mover;
	public int SpeciesIndex = 0;
	public int ID;
	public bool Alive;
    public GameObject foodSource;
	public string DebugText;
	
	public string SpeciesName { get; private set; }
	public SpeciesController Controller;
	public ControlStep LastControlStep;
	
    public delegate void CreatureClicked(Creature creature);
    public static event CreatureClicked OnCreatureClickedEvent;
	
	public IndividualModel Model;
	
	private List<IndividualStep> Steps;
	private int StepsToIgnore = 0;
	private int ExecutedSteps = 0;
	
	void Awake()
	{
		OnCreatureClickedEvent = null;
	}
	
	public void AddStep(IndividualStep step)
	{
		if (Steps == null)
		{
			Steps = new List<IndividualStep>();
			SetStartingPosition(step.Physics.Position);
			SetStartingOrientation(step.Physics.EOrientation);
		}
		Steps.Add(step);
	}
	
	public void Step()
	{
		if (Steps == null || Steps.Count == 0)
		{
			return;
		}
		IndividualStep step = Steps[0];
		Steps.RemoveAt(0);
		
		if (Mover != null)
		{
			if (StepsToIgnore == 0)
			{
				Mover.AddStep(step.Physics);
				StepsToIgnore = Mathf.Max(0, 4 - (int)Model.Speed);
			}
			else
			{
				StepsToIgnore -= 1;
			}
		}

        Alive = step.Alive;
        LastControlStep = step.Control; 
		ExecutedSteps += 1;
		
		DebugText = string.Format("Executing Step {0}/{1} ", ExecutedSteps, Steps.Count);
		
		DebugText += string.Format("\nPosition: {0}", step.Physics.Position);
		DebugText += string.Format("\nAlive: {0}", step.Alive);
		DebugText += string.Format("\nIgnoring: {0}", StepsToIgnore);
		DebugText += string.Format("\n\nFitness Stats:");
		DebugText += string.Format("\nNomadic: {0}",  Model.EvaluationStats.Nomadic);
		DebugText += string.Format("\nPredatory: {0}",  Model.EvaluationStats.Predatory);
		DebugText += string.Format("\nHerding: {0}",  Model.EvaluationStats.Herding);
		DebugText += string.Format("\nHoarding: {0}",  Model.EvaluationStats.Hoarding);
		
		DebugText += Mover.DebugText;
	}
	
	public void SetDataFromModel(IndividualModel model, string speciesName="Species in Planet")
	{
		SpeciesName = speciesName;
		ID = model.ID;		

		foreach(OrganelleModel organelle in model.Morphology.Body)
		{
			Vector2 delta = organelle.Coord;	
			
			if (delta == new Vector2(0, 0))
			{
				Nucleus n = GameObject.Instantiate(NucleusPrototype);
				n.transform.SetParent(transform);
				n.transform.localPosition += new Vector3(1.25f, 0.0f, 1.25f);
				n.NucleusModel = new NucleusModel(0);
				n.SpeciesIndex = SpeciesIndex;
			}
			else
			{
				Organelle o = 5 <= organelle.Trait.TID  && organelle.Trait.TID <= 8 ? GameObject.Instantiate(OrganellePrototype2) : GameObject.Instantiate(OrganellePrototype);
				o.transform.SetParent(transform);
				delta *= 2.5f;
				o.transform.localPosition += new Vector3(delta.x + 1.25f, 0.0f, delta.y + 1.25f);
				o.SpeciesIndex = SpeciesIndex;
				o.OrganelleModel = organelle;
			}
		}
		Mover.SetDataFromModel(model);
		Model = model;
	}
	
	public void SetStartingPosition(Vector2 v)
	{
		Mover.SetInitialPosition(v);
	}
	public void SetStartingOrientation(MovementDirection or)
	{
		Mover.SetInitialOrientation(or);
	}
	
	public void OnMouseDown()
	{
		if (OnCreatureClickedEvent != null)
		{
			OnCreatureClickedEvent(this);
		}
		Controller.OnCreatureClicked(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
