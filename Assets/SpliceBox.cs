using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpliceBox : MonoBehaviour
{

    Image img;
    SpliceButton b;
	// Use this for initialization
	void Start ()
	{
        img = gameObject.GetComponent<Image>();
	}

    public void assign(SpliceButton button)
    {
        if (button != null)
        {
            b = button;
            img.color = button.getColor();
        }
        else
        {
            free();
        }
        
    }

    public void free()
    {
        b = null;
        img.color = Color.white;
    }

    public SpliceButton getButton()
    {
        return b;
    }
}
