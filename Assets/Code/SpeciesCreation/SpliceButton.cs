using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceButton : MonoBehaviour 
{

    public int size, category, id;
    private SpliceUIManager uiManager;
    private Image image;
    private bool lEnabled = false;
    bool ready = false;
    Color color;
    
    //TODO: REALLY TEMPORARY
    static String[] SpliceNames = new String[] { "Predator", "Gourmet-A", "Tropical", "Hot'n'Cold", "Sensitive", "Mobile", "T Moncher", "Omnivorous-A", "Cryophile", "Thermophile", "Empath", "Safe", "Warrior", "Extremophile", "Xtreme-Extremophile", "Omni-Omnivorous-B", "Heyoka" };
    static int NextAvailableName = 0;

    public void Update()
    {
        if (!ready)
        {
            uiManager = GameObject.FindGameObjectWithTag("SpliceEditor").GetComponent<SpliceUIManager>();
            if (uiManager == null)
                return;
            
            image = GetComponent<Image>();
            category = uiManager.currentCategory;
            image.color = uiManager.tabColors[category];
            color = uiManager.tabColors[category];
            
            //TODO: SUPER TEMPORARY
            Text text = GetComponentInChildren<Text>();
            text.text = SpliceNames[NextAvailableName];
            NextAvailableName = (NextAvailableName + 1) % SpliceNames.Length;
            Debug.Log(text.text);
            ready = true;
        }
        
    }
    public void clicked()
    {
        
        try
        {
            if (!lEnabled)
            {
                uiManager.counters[size].setSelected(this);
                uiManager.updateBars(this, true);
            }
            else
            {
                uiManager.counters[size].deselect(this);
                uiManager.updateBars(this, false);
            }
        }
        catch (splicesFullException e)
        {
            return;
        }
        
        lEnabled = !lEnabled;
    }

    public class splicesFullException : Exception { };

    public Color getColor()
    {
        return color;
    }
}
