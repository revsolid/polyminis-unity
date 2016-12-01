
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
public class Range<T> where T: IComparable
{
    public T Min { get; private set; }
    public T Max { get; private set; }

    public Range(T min, T max)
    {
        Min = min;
        Max = max;
    }

    public bool Within(T value)
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
    public Range<float> Temperature;
    public Range<float> Ph;
    public List<SpeciesModel> Species;
    public string Name;
    public int ID;

    public PlanetModel() //TODO: Default constructor... assigns arbitratry values
    {
        Name = "Some Planet";
        SpaceCoords = new Vector2(100,0);
        Temperature = new Range<float>(0.0f, 1.0f);
        Ph = new Range<float>(1.3f, 4.2f);
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
    WARP
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
    WARP
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
}


// 
public class UserModel
{
    string Username;
    string Password;  // SUPER SECURE!!

    // TODO: What do they own 
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
    public string Name;
    public List<SpliceModel> Splices = new List<SpliceModel>();
    public object InstinctTuning = new object();
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
  public Range<float> Temperature;
  public Range<float> Ph;
  // TODO: Neural Network Representation (?)
}

//
[Serializable]
public enum ActionDirection
{
    HORIZONTAL,
    VERTIAL,
    ROTATION
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
    public ActionDirection Direction;
    public float Impulse;
}
[Serializable]
public class PhysicsStep
{
    public int Orientation;
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
    public PhysicsStep Physics;
//    public ControlStep Control;
}
[Serializable]
public class SimulationStartup
{
    public List<SpeciesStartup> Species;
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