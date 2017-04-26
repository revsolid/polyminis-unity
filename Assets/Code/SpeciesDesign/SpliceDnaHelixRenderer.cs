using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceDnaHelixRenderer : MonoBehaviour 
{
    public delegate void Clicked(SpliceDnaHelixRenderer renderer);
    public static event Clicked OnClickEvent;
    
    public Text NameField;
    public Image Background;
        
    public SpliceModel Model;
    
    bool ready = false;
    
    public void Start()
    {
    }
    
    public void Update()
    {
        if (!ready && Model != null)
        {
            NameField.text = Model.Name;
            Debug.Log(SpeciesDesignUI.SColorConfig);
            NameField.color = SpeciesDesignUI.SColorConfig.GetLabelColorForSplice(Model.EInstinct);
            Background.sprite = SpeciesDesignUI.SColorConfig.GetBackgroundSpriteForChosenSplice(Model.EInstinct);
            //Emblem.sprite = SpeciesDesignUI.SColorConfig.GetBackgroundSpriteForSplice(Model.EInstinct);
            this.gameObject.name = "SpliceHelixRenderer-" + Model.InternalName;

            ready = true;
        }
    }

    public void OnClick()
    {
        OnClickEvent(this);
    }
}