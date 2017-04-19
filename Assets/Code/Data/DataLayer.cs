
// This classes are just the translation layer between JSON and C# 
// BIG TODO: REFACTOR THIS IN SEVERAL FILES (or at least organize inside file)
/*
// Current Step JSON blob with minimal serialization from server
{
    "species":
   [{
        "name":"Species 1",
        "population":
        [{
            "id":0,
            "morphology":
            {
                "chromosome":[[0,9,106,173],[0,11,190,218],[0,0,190,239],[0,0,219,173]],
                "body":
                { 
                    "(0, 0)":{"organelle_id":106},
                    "(1, 0)":{"organelle_id":190},
                    "(1, 1)":{"organelle_id":190},
                    "(2, 0)":{"organelle_id":219}
                } 
            },
            //TODO: Physics doesn't really work, needs to be abstracted away into events (SEE DESIGN NOTE)
            "physics":
            {
                "collisions":[],
                "dimensions":[3.0,2.0],
                "id":0,
                "position":[2.0,0.0]
            }
        }]
    }],
    "step":1
}
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//
[Serializable]
public class Range
{
    public float Min;
    public float Max;

    public Range(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public bool Within(float value)
    {
        return (Min.CompareTo(value) != -1 && Max.CompareTo(value) != 1);
    }
    
    public float Average()
    {
        float result = 0.0f;
        float? min = Min as float?;
        if(min != null) 
        {
            result += min.Value;
        }
        float? max = Max as float?;
        if(max != null) 
        {
            result += max.Value;
        }
        if (max != null && min != null)
        {
            result /= 2.0f;
        }
        return result; 
    }
}

//
[Serializable]
public class PlanetModel
{
    public Vector2 SpaceCoords;
    public Range Temperature; // = new Range(0.0f, 1.0f);
    public Range Ph; // = new Range(1.3f, 4.2f);
    public List<SpeciesModel> Species;
    public List<MaterialEntry> Materials;
    public string PlanetName; 
    public int Epoch;
    public int ID;

    public PlanetModel() //TODO: Default constructor... assigns arbitratry values
    {
        //       PlanetName = "Some Planet";
        //       SpaceCoords = new Vector2(100,0);

        Materials = new List<MaterialEntry>();
        MaterialEntry newMat = new MaterialEntry("A", 0.3f);
        Materials.Add(newMat);
        newMat = new MaterialEntry("T", 0.2f);        
        newMat = new MaterialEntry("G", 0.4f);        
        Materials.Add(newMat);
        newMat = new MaterialEntry("U", 0.1f);        
        Materials.Add(newMat);

    }
}

[Serializable]
public class MaterialEntry
{
    public string Material;
    public float Percentage;

    public MaterialEntry(string name , float percent)
    {
        Material = name;
        Percentage = percent;
    }
}

//
[Serializable]
public class StarModel
{
    Vector2 SpaceCoords;
    //TODO: (?)
}

//
[Serializable]
public class SystemModel
{
    StarModel Star;
    List<PlanetModel> Planets;
}

[Serializable]
public class BaseCommand
{
    public string Service;
    public string Command;
}


[Serializable]
public enum SpaceExplorationCommandType
{
    INIT,
    ATTEMPT_MOVE,
    ATTEMPT_WARP,
    CALC_WARP_COST,
    SAVE_POSITION
}
[Serializable]
public class SpaceExplorationCommand : BaseCommand
{
    SpaceExplorationCommandType CommandType;
    public Vector2 Position;
    
    public SpaceExplorationCommand(SpaceExplorationCommandType commandType, Vector2 position)
    {
        CommandType = commandType; 
        Position = position;
        Command = CommandType.ToString();
        Service = "space_exploration";
    }
}

[Serializable]
public class BaseEvent
{
    public string Service;   
    public string EventString;
}
[Serializable]
public enum SpaceExplorationEventType
{
    SPAWN_PLANETS,
    KICK_BACK,
    WARP,
    WARP_COST_RESULT
}
[Serializable]
public class SpaceExplorationEvent : BaseEvent
{
   public SpaceExplorationEventType EventType
   {
       get
       {
           return (SpaceExplorationEventType) Enum.Parse(typeof(SpaceExplorationEventType), EventString); 
       }
   }
   public PlanetModel[] Planets;
   public Vector2 Position;
   public float WarpCost;
   public int NewBiomassAvailable;
}


// USER
[Serializable]
public class UserModel
{
    public string UserName;
    string Password;  // SUPER SECURE!!
}

[Serializable]
public class UserServiceCommand : BaseCommand
{
    public string UserName;
    public UserServiceCommand(string username)
    {
        Service = "user";
        UserName = username;
    }
}

[Serializable]
public class UserServiceEvent : BaseEvent
{
    public bool Result = false;
    public string UserName;
    public Vector2 LastKnownPosition;
    public int     InventorySlots;
    public int     Biomass;
}


// INVENTORY
[Serializable]
public enum InventoryCommandType
{
    RESEARCH,
    NEW_SPECIES,
    UPDATE_SPECIES,
    SAMPLE_FROM_PLANET,
    GET_INVENTORY,
    DELETE_ENTRY,
    GET_GLOBAL_EPOCH
}

[Serializable]
public class InventoryCommand : BaseCommand
{
    InventoryCommandType CommandType;
    public int Slot;
    public SpeciesModel Species;
    public int PlanetId;
    public int Epoch;
    
    public InventoryCommand(InventoryCommandType commandType)
    {
        CommandType = commandType; 
        Command = CommandType.ToString();
        Service = "inventory";
    }
}
[Serializable]
public enum InventoryEventType
{
    InventoryUpdate,
    ResearchDone,
    ReceiveGlobalEpoch
}
[Serializable]
public class InventoryServiceEvent : BaseEvent
{
    public List<InventoryEntry> InventoryEntries;
    public InventoryEventType InventoryEventType
    {
        get
        {
           return (InventoryEventType) Enum.Parse(typeof(InventoryEventType), EventString); 
        }
    }
}

[Serializable]
public enum EpochEventType
{
    ReceiveGlobalEpoch
}
[Serializable]
public class EpochEvent : BaseEvent
{
    public int Epoch;
    public EpochEventType EpochEventType
    {
        get
        {
            return (EpochEventType) Enum.Parse(typeof(EpochEventType), EventString); 
        }
    }
}

[Serializable]
public enum EInventoryType
{
    Research,
    SpeciesSeed
}

[Serializable]
public class InventoryEntry
{
    EInventoryType InvType
    {
        get
        {
            return (EInventoryType) Enum.Parse(typeof(EInventoryType), InventoryType);
        }
    }

    public int Slot;
    public string InventoryType;
    public SpeciesModel Value;
    public SpeciesModel Species 
    {
        get
        {
            return Value;
        }
    }
   
    public string GetName()
    {
        string ret = Value.SpeciesName;
        if (InvType == EInventoryType.Research)
        {
            ret += "[R]";
        }
        return ret;
   }
}

// PLANET INTERACTIONS
[Serializable]
public enum PlanetInteractionCommandType
{
    EXTRACT,
    DEPLOY,
    EDIT_IN_PLANET,
    GET_TO_EDIT_IN_PLANET
}

[Serializable]
public class PlanetInteractionCommand : BaseCommand
{
    PlanetInteractionCommandType CommandType;
    public int Slot;
    public int Epoch;
    public int PlanetId;
    public SpeciesModel Species;
    //public string SpeciesName;
    
    public float ExtractedPercentage = 0.0f;
    public float DeployedBiomass     = 0.0f;
    
    public PlanetInteractionCommand(PlanetInteractionCommandType commandType)
    {
        CommandType = commandType; 
        Command = CommandType.ToString();
        Service = "orbital_interactions";
    }
}

[Serializable]
public enum PlanetInteractionEventType
{
    INTERACTION_RESULT,
    GET_EDIT_RESULT
}
[Serializable]
public class PlanetInteractionEvent : BaseEvent
{
    public PlanetInteractionEventType EventType
    {
        get
        {
            return (PlanetInteractionEventType) Enum.Parse(typeof(PlanetInteractionEventType), EventString.ToUpper());
        }
    }
    public PlanetModel UpdatedPlanet;
    public SpeciesModel Species;
    public int NewBiomassAvailable;
    public int InPlanet;
}


//
[Serializable]
public enum TraitSize
{
    SMALL,
    MEDIUM,
    LARGE
}

// 
[Serializable]
public enum Instinct
{
    NOMADIC = 0,
    HOARDING,
    PREDATORY,
    HERDING
}

//
[Serializable]
public class SpliceModel
{
    // Q: Instincts?
    public string Instinct;
    public Instinct EInstinct
    {
        get
        {
            return (Instinct) Enum.Parse(typeof(Instinct), Instinct.ToUpper());
        }
    }
    public string Size;
    public TraitSize TraitSize
    {
       get
       {
           return (TraitSize) Enum.Parse(typeof(TraitSize), Size); 
       }
    }

    public string Name;
    public string InternalName;
    public string Description;
    public List<string> Traits;
}

//
[Serializable]
public class InstinctTuningModel
{
    public int HoardingLvl = 0;
    public int PredatoryLvl = 0;
    public int HerdingLvl = 0;
    public int NomadicLvl = 0;
    public int HoardingMaxLvl = 0;
    public int PredatoryMaxLvl = 0;
    public int HerdingMaxLvl = 0;
    public int NomadicMaxLvl = 0;
}

//
[Serializable]
public class SpeciesModel
{
    public string SpeciesName;
    public List<SpliceModel> Splices = new List<SpliceModel>();
    public InstinctTuningModel InstinctTuning = new InstinctTuningModel();
    public object GAConfiguration = new object();
    public string CreatorName;
    public string OriginalSpeciesName;
    public string PlanetEpoch;
    public float Percentage;
    
    // Research information
    public bool BeingResearched = false;
    public int EpochStarted = -1;
    public int EpochDone = -1;

    public SpeciesModel()
    {

    }
    public SpeciesModel(SpeciesModel toCopy)
    {
        SpeciesName = toCopy.SpeciesName;
        OriginalSpeciesName = toCopy.OriginalSpeciesName;
        CreatorName = toCopy.CreatorName;
        PlanetEpoch = toCopy.PlanetEpoch;
        Splices = new List<SpliceModel>();
        foreach(SpliceModel sm in toCopy.Splices)
        {
            Splices.Add(sm);
        }
        InstinctTuning = toCopy.InstinctTuning;
        BeingResearched = toCopy.BeingResearched;
        EpochStarted = toCopy.EpochStarted;
        EpochDone = toCopy.EpochDone;
    }
}

public enum TraitTier
{
    BasicTier,   
    TierI,
    TierII,
    TierIII
}

//

[Serializable]
public class TraitModel
{
    public int TID;
    public string TraitTier;
    public string InternalName = "";
    TraitTier Tier;
}


[Serializable]
public class OrganelleModel
{
    public TraitModel Trait;
    public Vector2 Coord;
}

//
[Serializable]
public class CreatureMorphologyModel
{
  // Morphology of the creature, how to build the 3D representation of this species 
    public List<OrganelleModel> Body; 
}


//
[Serializable]
public class IndividualPhysics
{
  public Vector2 StartingPos; 
  public Vector2 Position;
  public Vector2 Dimensions; 
}

//
[Serializable]
public class EvaluationStatistics 
{
    public float Nomadic; 
    public float Herding; 
    public float Hoarding; 
    public float Predatory; 
}

//
[Serializable]
public class IndividualModel
{
  // A creature in a simulation

    public int  ID;
    public CreatureMorphologyModel Morphology;
    public int HP;
    public IndividualPhysics Physics;
    public Range Temperature;
    public Range Ph;
    public EvaluationStatistics EvaluationStats;
    public float Fitness;
    public float Speed; 

    public ControlStartUp Control;
  
}

//
[Serializable]
public enum ActionDirection
{
    HORIZONTAL,
    VERTICAL,
    ROTATION
}
[Serializable]
public enum MovementDirection
{
    UP,
    LEFT,
    DOWN,
    RIGHT,
}
[Serializable]
public class CollisionModel
{
    public int ID_1;
    public int ID_2;
}
[Serializable]
public class PhysicsAction
{
    public string direction = "";
    public ActionDirection EDirection 
    {
        get
        {
            if (direction == "")
                return ActionDirection.HORIZONTAL;
            return (ActionDirection) Enum.Parse(typeof(ActionDirection), direction.ToUpper());
        }
    }
    public float impulse;
}
[Serializable]
public class PhysicsStep
{
    public string Orientation;
    public MovementDirection  EOrientation
    {
        get
        {
            return (MovementDirection) Enum.Parse(typeof(MovementDirection), Orientation.ToUpper());
        }
    }
    public Vector2 Position;
    public List<CollisionModel> Collisions;
    public PhysicsAction LastAction;
}
[Serializable]
public class ControlStep
{
    public List<float> Inputs;
    public List<float> Outputs;
    public List<float> Hidden;
}


[Serializable]
public class NNLayer
{
    public List<float> Coefficients;
    public List<float> Biases;
}

[Serializable]
public class ControlStartUp
{
    public NNLayer HiddenToOutput;
    public NNLayer InToHidden;
}

[Serializable]
public class IndividualStep
{
    public int ID;
    public bool Alive;
    public PhysicsStep Physics;
    public ControlStep Control;
}

[Serializable]
public class StaticObjectModel
{
    public Vector2 Position;
    public Vector2 Dimensions;
}
[Serializable]
public class WorldObjectModel
{
    public Vector2 Position;
    public Vector2 Dimensions;
    public float Temperature;
    public float Ph;
}

[Serializable]
public class PhysicsWorldModel
{
    public List<StaticObjectModel> StaticObjects;
}
[Serializable]
public class SimulationEnvironment
{
    public PhysicsWorldModel PhysicsWorld;
    public List<WorldObjectModel> WorldObjects;
}
[Serializable]
public class Scenario
{
    public SimulationEnvironment Environment;
}
[Serializable]
public class SimulationStartup
{
    public List<SpeciesStartup> Species;
    public SimulationEnvironment Environment;
}
[Serializable]
public class SpeciesStartup
{
    public string SpeciesName;
    public List<IndividualModel> Individuals;
}
[Serializable]
public class SpeciesStep
{
    public string SpeciesName; 
    public List<IndividualStep> Individuals;
}
[Serializable]
public class SimulationStep
{
    public List<SpeciesStep> Species;
}
[Serializable]
public class Simulation
{
    public List<SimulationStep> Steps;
}