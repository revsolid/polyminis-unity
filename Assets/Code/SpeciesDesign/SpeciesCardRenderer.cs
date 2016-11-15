using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeciesCardRenderer : MonoBehaviour
{
    public delegate void RendererClicked(SpeciesCardRenderer button);
    public static event RendererClicked OnClickEvent; 

    public SpeciesModel Model;
    public Text NameText;
    bool Ready = false;
    
    void Start() {}
    void Update()
    {
        if (!Ready)
        {
            NameText.text = Model.Name;
            Ready = true;
        }

    }
}