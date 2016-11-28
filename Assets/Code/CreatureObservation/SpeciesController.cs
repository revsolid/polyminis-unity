using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesController : MonoBehaviour {

	Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
	List<SpeciesStep> Steps= new List<SpeciesStep>();
	public Creature CreaturePrototype;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		while (Steps.Count > 0)
		{
			SpeciesStep ss = Steps[0];
			Steps.RemoveAt(0);
			foreach (IndividualStep i_s in  ss.Individuals)
			{
				Creature ind = Individuals[i_s.ID];
				if (ind != null)
				{
					ind.AddStep(i_s);
				}
			}
		}
	}
	
	void SpawnCreatures(IndividualModel model)	
	{
		Creature creature = Instantiate<Creature>(CreaturePrototype);
		creature.SetDataFromModel(model);
		Individuals[model.ID] = creature;
	}
}
