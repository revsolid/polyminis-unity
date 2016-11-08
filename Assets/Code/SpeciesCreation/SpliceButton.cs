using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceButton : MonoBehaviour 
{
    public Text NameField;
    public delegate void ButtonClicked(SpliceButton button);
    public static event ButtonClicked OnClickEvent;

    SpliceUIManager uiManager;
    Image image;
    
    bool lEnabled = false;
    bool ready = false;
    Color color;

    public SpliceModel Model;
    
    public void Update()
    {
        if (!ready)
        {
            NameField.text = Model.Name;
            ready = true;
        }
    }

    public void OnClick()
    {
        OnClickEvent(this);
    }
}