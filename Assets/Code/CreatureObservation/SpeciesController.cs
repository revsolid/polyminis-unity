using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
class StatEntry
{
    public string SpeciesName;
    public float Value;
}

[Serializable]
class EpochStats
{
    public int Epoch = -1;
    public List<StatEntry> Percentages;
}

class CreatureObservationEvent : BaseEvent
{
    public List<SpeciesStartup> Species;
    public SimulationEnvironment Environment;
    public List<SimulationStep> Steps;
    public EpochStats EpochStats;
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
    public CameraController CameraController;
    public EnvironmentController EnvironmentController;
	public OrganellesCatalog Catalog;
    
    [HideInInspector]public float BestFitness = 0;

    Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
    List<int> IndividualIDs = new List<int>();
    List<SpeciesStep> Steps = new List<SpeciesStep>();
    List<Creature> CreaturesSpawned = new List<Creature>();
    bool IdleCoroutine = true;
    bool QuitCoroutine = false;
    bool Restart = false;
    CreatureObservationEvent PendingSpawn;
    List<CreatureObservationEvent> PendingSteps = new List<CreatureObservationEvent>();
    PlanetModel Planet;
    
    void Start()
    {
    }
    void Awake()
    {
		Connection.Instance.OnMessageEvent += OnServerMessage;
        Planet = PlanetInfoCacher.planetModel;

        if (Planet == null)
        {
            Planet = new PlanetModel();
            Planet.ID = 2011;
            Planet.Epoch = 33;
        }
        GoToEpoch(Planet.ID, Planet.Epoch);
    }
    
    public void GoToEpoch(int ID, int Epoch)
    {
        CreatureObservationCommand loadSimCmd;
        loadSimCmd = new CreatureObservationCommand(ID, Epoch);
        loadSimCmd.Command = "GO_TO_EPOCH";
        Connection.Instance.Send(JsonUtility.ToJson(loadSimCmd));
        
        GetStatistics(ID, Epoch);
    }
    
    void GetStatistics(int ID, int Epoch)
    {
        CreatureObservationCommand getStatsCmd;
        getStatsCmd = new CreatureObservationCommand(ID, Epoch);
        getStatsCmd.Command = "GET_STATS";
        Connection.Instance.Send(JsonUtility.ToJson(getStatsCmd));
    }

    void Poll()
    {
        BaseCommand pollCmd = new BaseCommand();
        pollCmd.Service = "creature_observation";
        pollCmd.Command = "POLL";
        Connection.Instance.Send(JsonUtility.ToJson(pollCmd));
    }
    
    void Step()
    {
       foreach(Creature ind in Individuals.Values) 
       {
           ind.Step();
       }
    }
    
    public void OnRestart()
    {
        CancelInvoke("Poll");
        CancelInvoke("Step");
        GoToEpoch(Planet.ID, Planet.Epoch);
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
                    SpawnCreature(im, i, s.SpeciesName);
                }
            }
            
            EnvironmentController.SetEnvironment(PendingSpawn.Environment);
                        
            PendingSpawn = null;

            foreach(KeyValuePair<int, Creature> entry in Individuals)
            {
                Creature ind = entry.Value; 
                if(ind.Model.Fitness > BestFitness)
                {
                    BestFitness = ind.Model.Fitness;
                }
            }
            // Always cleanup before adding the repeating call 
            // it is ok to do this since CancelInvoke just NOOPs the first time
            CancelInvoke("Poll");
            CancelInvoke("Step");
            InvokeRepeating("Poll", 0.1f, 3.0f); 
            InvokeRepeating("Step", 0.0f, 0.5f);
        }
    }
    
    void SpawnCreature(IndividualModel model, int index = 0, string speciesName="")    
    {
        Creature creature = Instantiate<Creature>(CreaturePrototype);
        creature.SpeciesIndex = index;
        if (string.IsNullOrEmpty(speciesName))
        {
            speciesName = "Species In Planet";
        }
        creature.SetDataFromModel(model, speciesName, Catalog);
        
        Individuals[model.ID] = creature;
        IndividualIDs.Add(model.ID);
        creature.
        SetStartingPosition(model.Physics.StartingPos);
        creature.Controller = this; 
        CreaturesSpawned.Add(creature);
    } 
    
    void AddStep(SpeciesStep ss)
    {
        Steps.Add(ss);
    }
    
    public void OnCreatureClicked(Creature creature)
    {
        CameraController.SetFocusOnCreature(creature);
    }
    
    public void OnServerMessage(string msg)
    {
        BaseEvent base_ev = JsonUtility.FromJson<BaseEvent>(msg);
        if (base_ev.Service == "creature_observation")
        {
            CreatureObservationEvent ev = JsonUtility.FromJson<CreatureObservationEvent>(msg);
            if (ev.EventString == "NEW_EPOCH")
            {
                PendingSpawn = ev;
            }
            else if (ev.EventString == "STEP_POLL")
            {
                PendingSteps.Add(ev);
            }
            else if (ev.EventString == "SIM_DONE")
            {
                CancelInvoke("Poll");
            }
            else if (ev.EventString == "EPOCH_STATS")
            {
                Debug.Log(msg);
            }
        }
    }
    
    void OnDestroy()
    {
        Connection.Instance.OnMessageEvent -= OnServerMessage;
        CancelInvoke("Poll");
        CancelInvoke("Step");
    }
}
