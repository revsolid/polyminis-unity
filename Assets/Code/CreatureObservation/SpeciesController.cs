using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class CreatureObservationEvent : BaseEvent
{
    public List<SpeciesStartup> Species;
    public SimulationEnvironment Environment;
    public List<SimulationStep> Steps;
}

class CreatureObservationCommand : BaseCommand
{
    public int PlanetId;
    public int Epoch;
    // Type
    public CreatureObservationCommand(int pid, int epoch)
    {
        Service = "creature_observation";
        PlanetId = pid; 
        Epoch = epoch;
    }
}

public class SpeciesController : MonoBehaviour
{
    public Creature CreaturePrototype;
    public StaticObject ObjectPrototype;
    public GameObject foodModel;
    public FollowerCamera DetailViewCamera;
    public DetailedViewUI DetailViewUI;

    Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
    List<SpeciesStep> Steps = new List<SpeciesStep>();
    List<Creature> CreaturesSpawned = new List<Creature>();
    bool IdleCoroutine = true;
    bool QuitCoroutine = false;
    bool Restart = false;
    CreatureObservationEvent PendingSpawn;
    List<CreatureObservationEvent> PendingSteps = new List<CreatureObservationEvent>();
    
    void Awake()
    {
    }
    void Start()
    {
        LoadExperiment("demo_46_1");
        //InvokeRepeating("Poll", 1.0f, 5.0f);
		Connection.Instance.OnMessageEvent += OnServerMessage;
 //       InvokeRepeating("Poll", 0.1f, 3.0f);
    }

    void Poll()
    {
        BaseCommand dummyCmd = new BaseCommand();
        dummyCmd.Service = "creature_observation";
        dummyCmd.Command = "POLL";
        Connection.Instance.Send(JsonUtility.ToJson(dummyCmd));
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
            Debug.Log(s.SpeciesName);
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
        
        if (PendingSteps.Count > 0)
        {
            if (PendingSteps[0].Steps.Count == 0)
            {
                PendingSteps.RemoveAt(0);
            }
            else
            {
                SimulationStep ss = PendingSteps[0].Steps[0];
				PendingSteps[0].Steps.RemoveAt(0);
                foreach(SpeciesStep s in ss.Species)
                {
                    AddStep(s);
                }
            }
        }
        
        if (PendingSpawn != null)
        {
            foreach(Creature c in CreaturesSpawned)    
            {
                Destroy(c.gameObject);
            }
            CreaturesSpawned.Clear();

            for(int i = 0; i < PendingSpawn.Species.Count; i++)
            {
                SpeciesStartup s = PendingSpawn.Species[i];
                foreach(IndividualModel im in s.Individuals)
                {
                    SpawnCreature(im, i);
                }
            }
            
            SimulationEnvironment env = PendingSpawn.Environment;  
            Debug.Log(env.PhysicsWorld.StaticObjects);
            
            foreach(StaticObjectModel som in env.PhysicsWorld.StaticObjects)
            {
                //TODO: This def. doesn't go here
                SpawnStatic(som);
            }
            
            PendingSpawn = null;
            InvokeRepeating("Poll", 0.1f, 3.0f); 
        }
    }
    
    void SpawnCreature(IndividualModel model, int index = 0)    
    {
        Creature creature = Instantiate<Creature>(CreaturePrototype);
        creature.SpeciesIndex = index;
        creature.SetDataFromModel(model);
        Individuals[model.ID] = creature;
        creature.SetStartingPosition(model.Physics.StartingPos);
        creature.Controller = this; 
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
    
    public void OnCreatureClicked(Creature creature)
    {
        DetailViewCamera.gameObject.SetActive(true);
        DetailViewCamera.Target = creature.transform;
        DetailViewUI.ToDetail = creature;
    }
    
    public void OnServerMessage(string msg)
    {
		Debug.Log("XXXXXXXXX");
        CreatureObservationEvent ev = JsonUtility.FromJson<CreatureObservationEvent>(msg);
        
        if (ev.Service == "creature_observation")
        {
			Debug.Log("SPECIES CONTROLLER - OSMsg");
            if (ev.EventString == "NEW_EPOCH")
            {
                PendingSpawn = ev;
            }
            else if (ev.EventString == "STEP_POLL")
            {
                PendingSteps.Add(ev);
            }
        }
    }
}
