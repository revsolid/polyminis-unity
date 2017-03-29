using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject StarMap;
    public InventoryUI Inventory;
    bool StarMapToggle = false;
    bool InventoryToggle = false;

    // Use this for initialization
    void Start ()
    {
    }
    
    // Update is called once per frame
    void Update ()
    {
    }

    public void OnStarmapClick()
    {
        StarMapToggle = !StarMapToggle;

        StarMap.SetActive(StarMapToggle);
    }

    public void OnCloseStarmapClick()
    {
        StarMapToggle = false;
        StarMap.SetActive(StarMapToggle);
    }

    public void OnSpeciesEditorClick()
    {
        InventoryToggle = !Inventory.gameObject.active; 
        
        if (InventoryToggle)
            Inventory.ShowInMode(InventoryMode.NORMAL);
        else
            Inventory.Dismiss();
    }

    // takes space position and convert it to 3D position on starmap
    public static Vector3 ToStarmapPos(Vector2 spacePos)
    {
        float shrinkFactor = 10.0f;

        return (new Vector3(-spacePos.x / shrinkFactor, spacePos.y / shrinkFactor, 0.0f));
    }


}