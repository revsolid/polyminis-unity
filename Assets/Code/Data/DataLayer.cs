
// This classes are just the translation layer between JSON and C# 
// BIG TODO: UUID
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
    public string PlanetName; 
    public int Epoch;
    public int ID;

    public PlanetModel() //TODO: Default constructor... assigns arbitratry values
    {
 //       PlanetName = "Some Planet";
 //       SpaceCoords = new Vector2(100,0);
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
    GET_INVENTORY
}

[Serializable]
public class InventoryCommand : BaseCommand
{
    InventoryCommandType CommandType;
    public int Slot;
    public SpeciesModel Species;
    
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
    ResearchDone
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

   public string InventoryType;
   public SpeciesModel Value;
   public SpeciesModel Species 
   {
       get
       {
           return Value;
       }
   }
   ResearchModel Research;
   
   public string GetName()
   {
       if (InvType == EInventoryType.SpeciesSeed)
       {
           return Value.SpeciesName;
       }
       else
       {
           return Research.PlanetEpoch;
       }
   }
}

[Serializable]
public class ResearchModel
{
    public string PlanetEpoch;
}

// PLANET INTERACTIONS
[Serializable]
public enum PlanetInteractionCommandType
{
    EXTRACT,
    DEPLOY,
    EDIT_IN_PLANET
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
    
    public float ExtractedPopulation = 0.0f;
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
    INTERACTION_RESULT
}
[Serializable]
public class PlanetInteractionEvent : BaseEvent
{
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
    public int[] Traits;
}

//
[Serializable]
public class SpeciesModel
{
    public string SpeciesName;
    public List<SpliceModel> Splices = new List<SpliceModel>();
    public object InstinctTuning = new object();
    public object GAConfiguration = new object();
    public string CreatorName;
    public string OriginalSpeciesName;
    public string PlanetEpoch;

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
        InstinctTuning = new object();
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
public class IndividualModel
{
  // A creature in a simulation

  public int  ID;
  public CreatureMorphologyModel Morphology;
  public int HP;
  public IndividualPhysics Physics;
  public Range Temperature;
  public Range Ph;
  // TODO: Neural Network Representation (?)
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
public class PhysicsWorldModel
{
    public List<StaticObjectModel> StaticObjects;
}
[Serializable]
public class SimulationEnvironment
{
    public PhysicsWorldModel PhysicsWorld;
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
    public string Name;
    public List<IndividualModel> Individuals;
}
[Serializable]
public class SpeciesStep
{
    public string Name; 
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