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
    
    bool ready;
    
    public void Start()
    {
    }
    
    public void Update()
    {
        if (!ready)
        {
            NameField.text = Model.Name;
            ready = true;
            Background.color = SpeciesDesignUI.SColorConfig.GetColorFor(Model.EInstinct);
        }
    }

    public void OnClick()
    {
        OnClickEvent(this);
    }
}