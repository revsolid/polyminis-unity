using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public enum InventoryMode
{
    SELECTION,
    NORMAL
}

public class InventoryUI : MonoBehaviour
{
    public InventoryUIEntry EntryPrototype;
    public LayoutGroup EntriesLayoutGroup;
    public SpeciesDesignUI SpeciesDesigner;
    
    InventoryMode CurrentMode;
    
    public delegate void EntrySelected(InventoryEntry model, int slotNum);
    public static event EntrySelected OnEntrySelected;

    bool Reload = false;
    int Epoch = -1;

    void Start ()
    {
        gameObject.SetActive(false);
        Session.OnSessionChangedEvent += () => { Reload = true; };
        OrbitalUI.OnGoBackToSpaceExScreen += Dismiss;
        OrbitalApproachRenderer.OnToOrbitScreen += Dismiss;
    }

    void OnDestroy()
    {
        OrbitalUI.OnGoBackToSpaceExScreen -= Dismiss;
        OrbitalApproachRenderer.OnToOrbitScreen -= Dismiss;
    }

    void Awake () 
    {
    }
    
    void Update() 
    {
        if (Reload == true)
        {
            LoadFromSession();
            Reload = false;
        }

        if (Epoch != -1)
        {
            foreach (InventoryUIEntry entry in this.gameObject.GetComponentsInChildren<InventoryUIEntry> ())
            {
                entry.UpdateProgressBar (Epoch);
            }
        }
    }

    public void ShowInMode(InventoryMode mode)
    {
        CurrentMode = mode;
        if (CurrentMode == InventoryMode.NORMAL)
        {
            InventoryUIEntry.OnClickEvent +=  HandleEdit;
        }
        else
        {
            InventoryUIEntry.OnClickEvent += HandleSelection;
        }
        InventoryUIEntry.OnDeleteEvent += HandleDelete;

        // Always Re-get from the Session as it has the latest and greatest        
        LoadFromSession();
        gameObject.SetActive(true);
    }
    void LoadFromSession()
    {
        Connection.Instance.OnMessageEvent -= OnServerMessage;
        var children = new List<GameObject>();
        foreach (Transform child in EntriesLayoutGroup.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int fullEntries = Session.Instance.InventoryEntries.Count;
        for(int i = 0; i < fullEntries; i++)
        {
            InventoryUIEntry uiEntry = Instantiate(EntryPrototype);
            uiEntry.InvEntry = Session.Instance.InventoryEntries[i];
            uiEntry.Mode = CurrentMode;
            uiEntry.transform.SetParent(EntriesLayoutGroup.transform);
        }
        
        // Add Empty Slots
        int emptySlots = Session.Instance.Slots - Session.Instance.InventoryEntries.Count;
        
        for(int i = 0; i < emptySlots; ++i)
        {
            InventoryUIEntry uiEntry = Instantiate(EntryPrototype);
            uiEntry.InvEntry = null;
            uiEntry.Mode = CurrentMode;
            uiEntry.transform.SetParent(EntriesLayoutGroup.transform);
        }
        Connection.Instance.OnMessageEvent += OnServerMessage;
    } 
    
    void HandleEdit(InventoryEntry currentModel)
    {
        InventoryCommandType cmdType;
        SpeciesModel sm = null;
        int slot;
        if (currentModel == null)
        {
            Debug.Log("New Species");
            cmdType = InventoryCommandType.NEW_SPECIES;
            slot = Session.Instance.NextAvailableSlot();
        }
        else
        {
            Debug.Log("Editing: " + currentModel);
            cmdType = InventoryCommandType.UPDATE_SPECIES;
            sm = currentModel.Species;
            slot = currentModel.Slot;
        }
        
        SpeciesDesignUI.OnSaveEvent += (resultingModel) => 
        {
            //Debug.Log(JsonUtility.ToJson(resultingModel));
            InventoryCommand saveSpeciesCommand = new InventoryCommand(cmdType);
            saveSpeciesCommand.Species = resultingModel;
            saveSpeciesCommand.Slot = slot;
            Debug.Log(saveSpeciesCommand);
            Connection.Instance.Send(JsonUtility.ToJson(saveSpeciesCommand));
        };
        SpeciesDesigner.OpenWithSpecies(sm);
    }
    
    void HandleSelection(InventoryEntry currentModel)
    {
        Debug.Log("Selecting: " + currentModel);
        OnEntrySelected(currentModel, currentModel.Slot);
        Dismiss();
    }
    
    void HandleDelete(InventoryEntry currentModel)
    {
        Debug.Log("Deleting: " + currentModel);
        InventoryCommand deleteSpeciesCommand = new InventoryCommand(InventoryCommandType.DELETE_ENTRY);
        deleteSpeciesCommand.Slot = currentModel.Slot;
        Connection.Instance.Send(JsonUtility.ToJson(deleteSpeciesCommand));
    }

    public void Dismiss()
    {
        gameObject.SetActive(false);
        if (CurrentMode == InventoryMode.NORMAL)
        {
            InventoryUIEntry.OnClickEvent -=  HandleEdit;
        }
        else
        {
            InventoryUIEntry.OnClickEvent -= HandleSelection;
        }
        InventoryUIEntry.OnDeleteEvent -= HandleDelete;
        OnEntrySelected = null;
    }

    void OnServerMessage(string message)
    {
        EpochEvent evt = JsonUtility.FromJson<EpochEvent>(message);
        if (evt != null)
        {
            switch (evt.EpochEventType)
            {
            case EpochEventType.ReceiveGlobalEpoch:
                Debug.Log ("Received Global epoch");
                Debug.Log ("Epoch: " + evt.Epoch.ToString ());
                Epoch = evt.Epoch;
                break;
            }
        }
    }


}