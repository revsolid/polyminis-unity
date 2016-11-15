using UnityEngine;
using System.Collections;

public class ColorConfig : MonoBehaviour
{
	public Color NomadicColor; 
	public Color HoardingColor;
	public Color PredatoryColor;
	public Color HerdingColor;

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
}
