using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesController : MonoBehaviour
{

	Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
	List<SpeciesStep> Steps = new List<SpeciesStep>();
	public Creature CreaturePrototype;
	bool IdleCoroutine = true;
	// Use this for initialization
	void Start ()
	{
		TextAsset species_json = Resources.Load<TextAsset>("StaticData/species_final_2");
		SimulationStartup sim_startup = JsonUtility.FromJson<SimulationStartup>(species_json.text);
		
		for(int i = 0; i < sim_startup.Species.Count; i++)
		{
			SpeciesStartup s = sim_startup.Species[i];
			Debug.Log(s.Name);
			foreach(IndividualModel im in s.Individuals)
			{
				SpawnCreature(im, i);
			}
		}
		
		TextAsset steps_json = Resources.Load<TextAsset>("StaticData/steps_final_2");
		Simulation sim = JsonUtility.FromJson<Simulation>(steps_json.text);
		
		foreach(SpeciesStep ss in sim.Steps[0].Species)
		{
			foreach(IndividualStep i_s in ss.Individuals)
			{
				Creature ind = null;
				Individuals.TryGetValue(i_s.ID, out ind);
				if (ind != null)
				{
					ind.SetStartingPosition(i_s.Physics.Position);
				}
			}
		}
		
		foreach(SimulationStep ss in sim.Steps)
		{
			foreach(SpeciesStep s in ss.Species)
			{
				AddStep(s);
			}
		}
	}
	
	IEnumerator PassStepToCreatures(SpeciesStep ss, Dictionary<int, Creature> inds)
	{
		yield return null;
		foreach (IndividualStep i_s in  ss.Individuals)
		{
			Creature ind = null;
			inds.TryGetValue(i_s.ID, out ind);
			if (ind != null)
			{
				ind.AddStep(i_s);
			}
			else
			{
			}
		}
		IdleCoroutine = true;
		yield break;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Steps.Count > 0 && IdleCoroutine)
		{
			SpeciesStep ss = Steps[0];
			Steps.RemoveAt(0);
			IdleCoroutine = false;
			StartCoroutine(PassStepToCreatures(ss, Individuals));
		}
	}
	
	void SpawnCreature(IndividualModel model, int index = 0)	
	{
		Creature creature = Instantiate<Creature>(CreaturePrototype);
		creature.SpeciesIndex = index;
		creature.SetDataFromModel(model);
		Individuals[model.ID] = creature;
	}
	
	void AddStep(SpeciesStep ss)
	{
		Steps.Add(ss);
	}
}
