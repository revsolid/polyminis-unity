using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNode : MonoBehaviour
{
    public int id;
    public Color NNColor1;
    public Color NNColor2;
    
    float value;
    Text valueLabel;
    Image image; 
    
    public void SetValue(float inVal)
    {
        value = inVal;
    }
    // Use this for initialization
    void Start ()
    {
        id = -1;
        value = 0;
        valueLabel = transform.FindChild("Label").gameObject.GetComponent<Text>();
        if(!valueLabel)
        {
            Debug.Log("Warning: node has no value label");
        }

        image = transform.FindChild("Background").gameObject.GetComponent<Image>();
        if (!image)
        {
            Debug.Log("Warning: node has no bg image");
        }

    }

    void SetColor()
    {
        //float clampedVal = value > 1.0f ? 1.0f : value;
        //clampedVal = clampedVal < 0 ? 0 : clampedVal;
        //image.color = new Color(clampedVal, clampedVal, clampedVal, 1.0f);
        image.color = Color.Lerp(NNColor1, NNColor2, Mathf.Clamp(value, 0.0f, 1.0f));
        //image.color = new Color(colorSpaceCoord.x, colorSpaceCoord.y, colorSpaceCoord.z, 1.0f);
    }
    
    // Update is called once per frame
    void Update ()
    {
        valueLabel.text = value.ToString();
        SetColor();
    }
}
