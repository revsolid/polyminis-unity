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

    public SpliceModel Model;


    bool ready = false;
    
    public void Update()
    {
        if (!ready && Model != null)
        {
            NameField.text = Model.Name;
            Debug.Log(SpeciesDesignUI.SColorConfig);
            NameField.color = SpeciesDesignUI.SColorConfig.GetLabelColorForSplice(Model.EInstinct);
            Background.sprite = SpeciesDesignUI.SColorConfig.GetBackgroundSpriteForSplice(Model.EInstinct);
            //Emblem.sprite = SpeciesDesignUI.SColorConfig.GetBackgroundSpriteForSplice(Model.EInstinct);
            this.gameObject.name = "SpliceButtonRenderer-" + Model.InternalName;

            ready = true;
        }
    }
    
    public void OnClick()
    {
        OnClickEvent(this);
    }
}