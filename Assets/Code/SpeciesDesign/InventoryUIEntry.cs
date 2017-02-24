using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIEntry : MonoBehaviour
{
    InventoryEntry _invEntry;
    public delegate void EntryClicked(InventoryEntry model, int slotNum);
    public static event EntryClicked OnClickEvent;
    [HideInInspector]
    public InventoryEntry InvEntry;
    public Text EntryName;
    public int SlotNum;    
    public InventoryMode Mode;
    
    void Start () {}
    void Awake () {}
    void Update()
    {
        if (InvEntry != null)
        {
            EntryName.text =  InvEntry.GetName();
        }
        else if (Mode == InventoryMode.NORMAL)
        {
            EntryName.text = "Create new Species";
        }
	else
	{
	    gameObject.SetActive(false);
	}
    }
    
    public void OnClickEntry() 
    {
        Debug.Log("Clickety");
        if (OnClickEvent != null)
            OnClickEvent(InvEntry, SlotNum);
    }
}
