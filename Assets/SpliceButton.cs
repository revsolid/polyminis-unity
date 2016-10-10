using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpliceButton : MonoBehaviour {

    public int size, catagory, id;
    private SpliceUIManager uiManager;
    private Image image;
    private bool enabled = false;
    Color color;

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("SpliceUIManager").GetComponent<SpliceUIManager>();
        image = GetComponent<Image>();
        image.color = uiManager.tabColors[catagory];
        color = uiManager.tabColors[catagory];
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
