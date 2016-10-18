
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
                // TODO: Change this for the map, or add it, maybe the crhomosome might be useful
                "chromosome":[[0,9,106,173],[0,11,190,218],[0,0,190,239],[0,0,219,173]]
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






//
public class Range<T> 
{
    public T Min { get; private set; }
    public T Max { get; private set; }

    public Range<T>(T min, T max)
    {
        Min = min;
        Max = max;
    }

    public bool Within(T value)
    {
        return Min <= value && value  Max;
    }
}

//
public class PlanetModel
{
    Vector2 SpaceCoords;
    Range Temperature;
    Range Ph;
    IList<SpeciesModel> Species;
}

//
public class StarModel
{
    Vector2 SpaceCoords;
    //TODO: (?)
}

//
public class SystemModel
{
    StarModel Star;
    IList<PlanetModel> Planets;
}

// 
public class UserModel
{
    string Username;
    string Password;  // SUPER SECURE!!

    // TODO: What do they own 
}

//
public enum TraitSize
{
    SMALL,
    MEDIUM,
    LARGE
}

// 
public enum Instinct
{
    HOARDING,
    NOMADIC,
    HERDING,
    PREDATORY
}

//
public class SpliceModel
{
    // Q: Instincts?
    Instinct Instinct;
    TraitSize Size;
    string Name;
    string Description;
}

//
public class SpeciesModel
{
    IList<SpeciesModel> Splices;
    string Name;
    //TODO:
    // Instincts microtuning
}

//
public class OrganelleModel
{

}

//
public class CreatureMorphologyModel
{
  // Morphology of the creature, how to build the 3D representation of this species 

  Vector2 Dimensions;
  IDictionary<Vector2, OrganelleModel> CreatureMap; 
}

//
public class IndividualModel
{
  // A creature in a simulation

  Vector2 SimCoords; 
  CreatureMorphologyModel Morphology;
  int HP;
  Range Temperature;
  Range Ph;
  // TODO: Neural Network Representation (?)
}

//
public class SimulationStep
{

}


//
// DESIGN NOTE: The server sends down actions (i.e. MOVE HORIZONTALLY, ROTATE CCW, EAT), client side
// we should transform those actions into some sort of event stream so actuators can animate / react
public class SimulationAction
{

}