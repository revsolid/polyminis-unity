using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIEntry : MonoBehaviour
{
    public delegate void EntryClicked(InventoryEntry model);
    public static event EntryClicked OnClickEvent;
    public static event EntryClicked OnDeleteEvent;


    [HideInInspector]
    public InventoryEntry InvEntry;
    public Text EntryName;
    public InventoryMode Mode;
    public Button DeleteButton;
    
    void Start () {}
    void Awake () {}
    void Update()
    {
        if (InvEntry != null)
        {
            if (Mode == InventoryMode.SELECTION && InvEntry.Species.BeingResearched)
            {
                gameObject.SetActive(false);
            }
            EntryName.text =  InvEntry.GetName();
            DeleteButton.gameObject.SetActive(Mode != InventoryMode.SELECTION);
        }
        else if (Mode == InventoryMode.NORMAL)
        {
            EntryName.text = "Create new Species";
            DeleteButton.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    public void OnClickEntry() 
    {
        if (InvEntry != null && InvEntry.Value.BeingResearched)
        {
            Debug.Log("Research is unclickable");
            return;
        }
        Debug.Log("Clickety");
        if (OnClickEvent != null)
            OnClickEvent(InvEntry);
    }
    
    public void OnDeleteEntry()
    {
        Debug.Log("Deletety");
        if (OnDeleteEvent != null)
            OnDeleteEvent(InvEntry);
    }
}