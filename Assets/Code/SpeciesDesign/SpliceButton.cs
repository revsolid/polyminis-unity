using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceButton : MonoBehaviour 
{
    public delegate void ButtonClicked(SpliceButton button);
    public static event ButtonClicked OnClickEvent;

    public Text NameField;
    public Image Background;
    public Image Emblem;

    public SpliceModel Model;


    bool ready = false;
    
    public void Update()
    {
        if (!ready && Model != null)
        {
//            NameField.text = Model.Name;
            ready = true;
            Background.color = SpeciesDesignUI.SColorConfig.GetColorFor(Model.EInstinct);
            Emblem.sprite = SpeciesDesignUI.SColorConfig.GetSpriteFor(Model.EInstinct);
        }
    }
    
    public void OnClick()
    {
        OnClickEvent(this);
    }
}