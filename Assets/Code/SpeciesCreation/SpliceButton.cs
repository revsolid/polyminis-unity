using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceButton : MonoBehaviour 
{

    public int size, category, id;
    private SpliceUIManager uiManager;
    private Image image;
    private bool enabled = false;
    Color color;

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("SpliceEditor").GetComponent<SpliceUIManager>();
        image = GetComponent<Image>();
        category = uiManager.currentCategory;
        image.color = uiManager.tabColors[category];
        color = uiManager.tabColors[category];
    }
    public void clicked()
    {
        
        try
        {
            if (!enabled)
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
        
        enabled = !enabled;
    }

    public class splicesFullException : Exception { };

    public Color getColor()
    {
        return color;
    }
}
