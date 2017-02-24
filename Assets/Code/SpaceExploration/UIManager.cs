using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject StarMap;
    public InventoryUI Inventory;
    bool StarMapToggle = false;

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

    public void OnSpeciesEditorClick()
    {
        Inventory.ShowInMode(InventoryMode.NORMAL);
    }

    // takes space position and convert it to 3D position on starmap
    public static Vector3 ToStarmapPos(Vector2 spacePos)
    {
        float shrinkFactor = 5.0f;

        return (new Vector3(-spacePos.x / shrinkFactor, spacePos.y / shrinkFactor, 0.0f));
    }


}
