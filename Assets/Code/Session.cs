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
       SpeciesModel model = JsonUtility.FromJson<SpeciesModel>("{\"Name\":\"Cool dudes\",\"Splices\":[{\"Instinct\":\"Nomadic\",\"Size\":\"SMALL\",\"Name\":\"Tropical\",\"InternalName\":\"tropical\",\"Description\":\"Better adapted to hot weather\",\"Traits\":[2,3]},{\"Instinct\":\"Nomadic\",\"Size\":\"MEDIUM\",\"Name\":\"Thermophile\",\"InternalName\":\"thermophile\",\"Description\":\"Love me some hot weather\",\"Traits\":[2,17]},{\"Instinct\":\"Hoarding\",\"Size\":\"SMALL\",\"Name\":\"G-Eater\",\"InternalName\":\"g_eater\",\"Description\":\"Can eat G!\",\"Traits\":[4,5]}]}");
       Species[model.Name] = model;
       OnSessionChangedEvent += () => {};
    }
    // Inventory
    public IDictionary<string, SpeciesModel> Species;
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
}