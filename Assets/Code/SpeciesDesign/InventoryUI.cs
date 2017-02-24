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

    void Start ()
    {
        gameObject.SetActive(false);
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

        // Always Re-get from the Session as it has the latest and greatest        
        LoadFromSession();
        gameObject.SetActive(true);
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
            uiEntry.Mode = CurrentMode;
            uiEntry.gameObject.SetActive(true);
        }
        
        // Add Empty Slots
        int emptySlots = Session.Instance.Slots - Session.Instance.InventoryEntries.Count;
        
        for(int i = 0; i < emptySlots; ++i)
        {
            InventoryUIEntry uiEntry = Instantiate(EntryPrototype);
            uiEntry.InvEntry = null;
            uiEntry.SlotNum = i + fullEntries;
            uiEntry.transform.SetParent(EntriesLayoutGroup.transform);
            uiEntry.Mode = CurrentMode;
            uiEntry.gameObject.SetActive(true);
        }
    } 
    
    void HandleEdit(InventoryEntry currentModel, int slot)
    {
        InventoryCommandType cmdType;
        SpeciesModel sm = null;
        if (currentModel == null)
        {
            Debug.Log("New Species");
            cmdType = InventoryCommandType.NEW_SPECIES;
        }
        else
        {
            Debug.Log("Editing: " + currentModel);
            cmdType = InventoryCommandType.UPDATE_SPECIES;
            sm = currentModel.Species;
        }
        
        SpeciesDesignUI.OnSaveEvent += (resultingModel) => {
            Debug.Log(JsonUtility.ToJson(resultingModel));
                InventoryCommand saveSpeciesCommand = new InventoryCommand(cmdType);
                saveSpeciesCommand.Species = resultingModel;
                saveSpeciesCommand.Slot = slot;
                Connection.Instance.Send(JsonUtility.ToJson(saveSpeciesCommand));
        };
        SpeciesDesigner.OpenWithSpecies(sm);
    }
    
    void HandleSelection(InventoryEntry currentModel, int slot)
    {
            Debug.Log("Selecting: " + currentModel);
            OnEntrySelected(currentModel, slot);
            Dismiss();
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
        OnEntrySelected = null;
    }
}
