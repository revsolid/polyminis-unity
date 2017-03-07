using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesController : MonoBehaviour
{
	public Creature CreaturePrototype;
	public StaticObject ObjectPrototype;

	public FollowerCamera DetailViewCamera;
	public DetailedViewUI DetailViewUI;

	Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
	List<SpeciesStep> Steps = new List<SpeciesStep>();
	List<Creature> CreaturesSpawned = new List<Creature>();
	bool IdleCoroutine = true;
	bool QuitCoroutine = false;
	bool Restart = false;
	// Use this for initialization
	
	void Awake()
	{
	}
	void Start()
	{
		LoadExperiment("demo_0_1");
		InvokeRepeating("Poll", 1.0f, 3.0f);

		Creature.OnCreatureClickedEvent += OnCreatureClicked;
        Connection.OnMessageEvent += (message) => OnServerMessage(message);
	}

	void Poll()
	{
		BaseCommand dummyCmd = new BaseCommand();
        dummyCmd.Service = "creature_observation";
        dummyCmd.Command = "POLL";
        Connection.Instance.Send(JsonUtility.ToJson(dummyCmd));
		Debug.Log("XX");
	}

	public void LoadExperiment(string expname)
	{
		string species_file = expname + "_species";
		string steps_file = expname + "_steps";
		Individuals = new Dictionary<int, Creature>();
		Steps = new List<SpeciesStep>();
		
		foreach(Creature c in CreaturesSpawned)	
		{
			Destroy(c.gameObject);
		}
		CreaturesSpawned.Clear();

		TextAsset species_json = Resources.Load<TextAsset>("StaticData/" + species_file );
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
		
		TextAsset steps_json = Resources.Load<TextAsset>("StaticData/" + steps_file);
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
		
		TextAsset env_json = Resources.Load<TextAsset>("StaticData/scenario_master");
		Scenario env = JsonUtility.FromJson<Scenario>(env_json.text);
		
		Debug.Log(env.Environment.PhysicsWorld.StaticObjects);
		
		foreach(StaticObjectModel som in env.Environment.PhysicsWorld.StaticObjects)
		{
			//TODO: This def. doesn't go here
			SpawnStatic(som);
		}
		
		
		foreach(SimulationStep ss in sim.Steps)
		{
			foreach(SpeciesStep s in ss.Species)
			{
				AddStep(s);
			}
		}
		
		IdleCoroutine = true;
		Creature.OnCreatureClickedEvent += OnCreatureClicked;
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
			if (QuitCoroutine)
			{
				yield break;
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
		CreaturesSpawned.Add(creature);
	}
	
	void SpawnStatic(StaticObjectModel model)
	{
		StaticObject obj = Instantiate<StaticObject>(ObjectPrototype);
		obj.gameObject.transform.position = CreatureMover.SimulationPositionToScenePosition(model.Position);
		obj.gameObject.transform.position += new Vector3(-1.25f, 0.0f, -1.25f);
		obj.gameObject.transform.localScale = CreatureMover.SimulationScaleToSceneScale(model.Dimensions);
	}
	
	void AddStep(SpeciesStep ss)
	{
		Steps.Add(ss);
	}
	
	void OnCreatureClicked(Creature creature)
	{
		DetailViewCamera.gameObject.SetActive(true);
		DetailViewCamera.Target = creature.transform;
		DetailViewUI.ToDetail = creature;
	}
	
	void OnServerMessage(string msg)
	{
		Debug.Log("Got a message! ");
		BaseEvent ev = JsonUtility.FromJson<BaseEvent>(msg);
		
		{
			Debug.Log("Got a message! " + ev.Service);
		}
	}
}
