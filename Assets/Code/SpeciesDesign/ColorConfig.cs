using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColorConfig : MonoBehaviour
{
	public Color NomadicColor; 
	public Color HoardingColor;
	public Color PredatoryColor;
	public Color HerdingColor;
	
	public List<Sprite> Emblems;
    public List<Sprite> SpliceBackgrounds;
    public List<Sprite> ChosenSpliceBackgrounds;
    public List<Color> SpliceLabelColor;
    public List<Color> ChosenSpliceLabelColor;

    // Use this for initialization
    void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public Color GetColorFor(Instinct i)
	{
		Debug.Log("Getting color for: " + i);
		switch(i)
		{
			case Instinct.PREDATORY:
				Debug.Log(PredatoryColor);
				return PredatoryColor;
			case Instinct.HERDING:
				Debug.Log(HerdingColor);
				return HerdingColor;
			case Instinct.HOARDING:
				Debug.Log(HoardingColor);
				return HoardingColor;
			case Instinct.NOMADIC:
				Debug.Log(NomadicColor);
				return NomadicColor;
		}
		return Color.black;
	}
	
	public Sprite GetSpriteFor(Instinct i)
	{
		return Emblems[(int)i];
	}

    public Sprite GetBackgroundSpriteForSplice(Instinct i)
    {
        return SpliceBackgrounds[(int)i];
    }

    public Sprite GetBackgroundSpriteForChosenSplice(Instinct i)
    {
        return ChosenSpliceBackgrounds[(int)i];
    }

    public Color GetLabelColorForSplice(Instinct i)
    {
        return SpliceLabelColor[(int)i];
    }

    public Color GetLabelColorForChosenSplice(Instinct i)
    {
        return ChosenSpliceLabelColor[(int)i];
    }
}
