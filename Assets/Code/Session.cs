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
       OnSessionChangedEvent += () => {};
       Connection.OnMessageEvent += (message) => { OnMessageReceived(message); };
    }
    // Inventory
    public List<InventoryEntry> InventoryEntries;
    
    public int Slots;
    public int NextAvailableSlot()
    {
        if (InventoryEntries.Count < Slots)
        {
            return InventoryEntries.Count;
        }
        else
        {
            return -1;
        }
    }
   
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
    
    void OnMessageReceived(string message)
    {
        InventoryServiceEvent invEvent = JsonUtility.FromJson<InventoryServiceEvent>(message);
        
        if (invEvent != null)
        {
            if (invEvent.Service == "inventory")
            {
                if (invEvent.InventoryEventType == InventoryEventType.InventoryUpdate) 
                {
                    Debug.Log(message);
                    InventoryEntries = invEvent.InventoryEntries;
                    
                    foreach(var ie in InventoryEntries)
                    {
                        Debug.Log(ie.InventoryType);
                        Debug.Log(ie.Value);
                        OnSessionChangedEvent();
                    }
                }
            }
        }
            
        
    }
}