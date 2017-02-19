using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

//TODO: Maybe we move the connection and other Singleton-like things over here
public class Session : Singleton<Session> 
{
    public delegate void SessionChanged();
    public static event SessionChanged OnSessionChangedEvent;

    protected Session()
    {
       Species = new Dictionary<string, SpeciesModel>();
       SpeciesNames = new List<string> ();
       SpeciesModel model = JsonUtility.FromJson<SpeciesModel>("{\"Name\":\"Cool dudes\",\"Splices\":[{\"Instinct\":\"Nomadic\",\"Size\":\"SMALL\",\"Name\":\"Tropical\",\"InternalName\":\"tropical\",\"Description\":\"Better adapted to hot weather\",\"Traits\":[2,3]},{\"Instinct\":\"Nomadic\",\"Size\":\"MEDIUM\",\"Name\":\"Thermophile\",\"InternalName\":\"thermophile\",\"Description\":\"Love me some hot weather\",\"Traits\":[2,17]},{\"Instinct\":\"Hoarding\",\"Size\":\"SMALL\",\"Name\":\"G-Eater\",\"InternalName\":\"g_eater\",\"Description\":\"Can eat G!\",\"Traits\":[4,5]}]}");
       AddSpecies(model);
       OnSessionChangedEvent += () => {};
    }
    // Inventory
    // Use AddSpecies to add species so it gets saved in the name list
    public IDictionary<string, SpeciesModel> Species { get; private set;}
    public List<string> SpeciesNames { get; private set; }
    private string _userName;
    public string UserName
    {
        get
        {
            return _userName;
        }
        set
        {
            _userName = value;
            OnSessionChangedEvent();
        }
    }
    public Vector2 LastKnownPosition = Vector2.zero;
    private float _biomass = 100.0f;
    public float Biomass
    {
        get
        {
            return _biomass;
        }
        set
        {
            _biomass = value;
            OnSessionChangedEvent();
        }
    }    
    
    public virtual void OnDestroy()
    {
        base.OnDestroy();
        Connection.Instance.CloseConnection();
        Debug.Log("Session Terminated");
    }

    public void AddSpecies(SpeciesModel inSpecies)
    {
        Species [inSpecies.Name] = inSpecies; 
        bool found = false;
        foreach (string name in SpeciesNames)
        {
            if (name == inSpecies.Name)
            {
                found = true;
            }
        }
        if (!found)
        {
            SpeciesNames.Add (inSpecies.Name);
        }
    }

    // construct a list of species to be sent to server
    public List<SpeciesModel> ListOfSpecies()
    {
        List<SpeciesModel> retVal = new List<SpeciesModel> ();

        foreach (string name in SpeciesNames)
        {
            retVal.Add (Species [name]);
            Debug.Log (Species [name].Name);
        }

        return retVal;
    }

}