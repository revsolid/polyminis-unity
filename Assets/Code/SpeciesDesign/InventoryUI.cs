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
    
    bool Reload = false;

    void Start ()
    {
        ShowInMode(InventoryMode.NORMAL);
        Session.OnSessionChangedEvent += () => { Reload = true; };
    }
    void Awake () {}
    
    void Update() 
    {
        if (Reload == true)
        {
            LoadFromSession();
            Reload = false;
        }
    }

    void ShowInMode(InventoryMode mode)
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

        // Always Re-get from the Session as it has the latest and greatest        
        LoadFromSession();
    }
    void LoadFromSession()
    {
        var children = new List<GameObject>();
        foreach (Transform child in EntriesLayoutGroup.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int fullEntries = Session.Instance.InventoryEntries.Count;
        for(int i = 0; i < fullEntries; i++)
        {
            InventoryUIEntry uiEntry = Instantiate(EntryPrototype);
            uiEntry.InvEntry = Session.Instance.InventoryEntries[i];
            uiEntry.SlotNum = i;
            uiEntry.transform.SetParent(EntriesLayoutGroup.transform);
        }
        
        // Add Empty Slots
        int emptySlots = Session.Instance.Slots - Session.Instance.InventoryEntries.Count;
        
        for(int i = 0; i < emptySlots; ++i)
        {
            InventoryUIEntry uiEntry = Instantiate(EntryPrototype);
            uiEntry.InvEntry = null;
            uiEntry.SlotNum = i + fullEntries;
            uiEntry.transform.SetParent(EntriesLayoutGroup.transform);
        }

        gameObject.SetActive(true);
    } 
    
    void HandleEdit(InventoryEntry currentModel, int slot)
    {
        if (currentModel == null)
        {
            Debug.Log("New Species");
        }
        else
        {
            Debug.Log("Editing: " + currentModel);
            
            SpeciesDesignUI.OnSaveEvent += (resultingModel) => {
                Debug.Log(JsonUtility.ToJson(resultingModel));
                    InventoryCommand saveSpeciesCommand = new InventoryCommand(InventoryCommandType.UPDATE_SPECIES);
                    saveSpeciesCommand.Species = resultingModel;
                    Connection.Instance.Send(JsonUtility.ToJson(saveSpeciesCommand));
            };
            SpeciesDesigner.OpenWithSpecies(currentModel.Species);
        }
    }
    
    void HandleSelection(InventoryEntry currentModel, int slot)
    {
            Debug.Log("Selecting: " + currentModel);
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
    }
}