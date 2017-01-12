
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DnaSeqRenderer : MonoBehaviour
{
	public TraitModel Model;
    public bool Active = false;
    bool WasActive = false;
    public Image ColorImg;
    Color MyColor; // Derive color from the Organelle ID

	void Start ()
    {
        ColorImg.color = Color.black;
    }

    void Update()
    {
        if (Model != null)
        {
            MyColor = new Color(0.0f, Model.TID / 15.0f + 0.1f, 0.0f);
        }
        else
        {
            return;
        }
        if (Active && !WasActive)
        {
            WasActive = true;
            ColorImg.color = MyColor;
        }
        else if (!Active && WasActive)
        {
            WasActive = false;
            ColorImg.color = Color.black;
        }
    }
}