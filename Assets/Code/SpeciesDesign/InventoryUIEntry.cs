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
    void Awake(){}
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

    public void OnEnable()
    {
        // if it is being researched, query server of current epoch, 
        if (InvEntry != null && InvEntry.Value.BeingResearched)
        {
            InventoryCommand queryEpochComman = new InventoryCommand(InventoryCommandType.GET_GLOBAL_EPOCH);
            Connection.Instance.Send(JsonUtility.ToJson(queryEpochComman));
            Slider slider = this.transform.FindChild ("Progress").gameObject.GetComponent<Slider> ();
            slider.gameObject.SetActive (true);

        }
    }
        

    public void UpdateProgressBar(int EpochNow)
    {
        if (InvEntry != null && InvEntry.Value.BeingResearched)
        {
            Slider slider = this.transform.FindChild ("Progress").gameObject.GetComponent<Slider> ();
            slider.gameObject.SetActive (true);
            float value = (EpochNow - InvEntry.Value.EpochStarted) / (InvEntry.Value.EpochDone - InvEntry.Value.EpochStarted);

            if (value < 0) value = 0;
            else if (value > 1.0f) value = 1.0f;

            slider.value = value;
        }
    }

}