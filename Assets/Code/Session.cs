using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

//TODO: Maybe we move the connection and other Singleton-like things over here
public class Session : Singleton<Session> 
{
    protected Session()
    {
       Species = new Dictionary<string, SpeciesModel>();
       SpeciesModel model = JsonUtility.FromJson<SpeciesModel>("{\"Name\":\"Cool dudes\",\"Splices\":[{\"Instinct\":\"Nomadic\",\"Size\":\"SMALL\",\"Name\":\"Tropical\",\"InternalName\":\"tropical\",\"Description\":\"Better adapted to hot weather\",\"Traits\":[2,3]},{\"Instinct\":\"Nomadic\",\"Size\":\"MEDIUM\",\"Name\":\"Thermophile\",\"InternalName\":\"thermophile\",\"Description\":\"Love me some hot weather\",\"Traits\":[2,17]},{\"Instinct\":\"Hoarding\",\"Size\":\"SMALL\",\"Name\":\"G-Eater\",\"InternalName\":\"g_eater\",\"Description\":\"Can eat G!\",\"Traits\":[4,5]}]}");
       Species[model.Name] = model;
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
            Debug.Log("Setting UserName");
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            if (objects.Length > 0)
            {
                PlayerID pid = objects[0].GetComponent<PlayerID>();
                if (pid != null)
                    pid.PlayerName.text = UserName;
            }
            else
            {
                Debug.Log("But didn't find the thingy :(");
            }
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
            
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            if (objects.Length > 0)
            {
                PlayerID pid = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerID>();
                if (pid != null)
                    pid.Biomass.text = Biomass + "";
            }
        }
    }    
    
    public virtual void OnDestroy()
    {
        base.OnDestroy();
        Connection.Instance.CloseConnection();
        Debug.Log("Session Terminated");
    }
}