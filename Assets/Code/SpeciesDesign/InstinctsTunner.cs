using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InstinctsTunner : MonoBehaviour
{
	public LayoutGroup Hoarding;
	public LayoutGroup Herding;
	public LayoutGroup Predatory;
	public LayoutGroup Nomadic;
	public GameObject MarkerPrototype;
	
	public int MinLevel = 2;
	public int MaxLevel = 8;
	
	private Dictionary<Instinct, int> Levels;
	private Dictionary<Instinct, LayoutGroup> LayoutMap;
	
	// Use this for initialization
	public void Ready()
	{
		LayoutMap = new Dictionary<Instinct, LayoutGroup>();
		LayoutMap[Instinct.HOARDING] = Hoarding;
		LayoutMap[Instinct.HERDING] = Herding;
		LayoutMap[Instinct.PREDATORY] = Predatory;
		LayoutMap[Instinct.NOMADIC] = Nomadic;

		Levels = new Dictionary<Instinct, int>();
		foreach(Instinct i in Enum.GetValues(typeof(Instinct)))
		{
			// Add 2 per instinct as default
			Levels[i] = 2;
			AddSplice(i);
			AddSplice(i);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public void AddSplice(Instinct instinct)
	{
		GameObject marker = Instantiate<GameObject>(MarkerPrototype);
		Color color = SpeciesDesignUI.SColorConfig.GetColorFor(instinct);
		
		Image img = marker.GetComponentInChildren<Image>();
		img.color = 2 * color * Levels[instinct] / MaxLevel;
		Levels[instinct] += 1;
		marker.transform.parent = LayoutMap[instinct].transform;
		marker.transform.localPosition = Vector3.zero;
        marker.transform.localScale = Vector3.one;
        marker.transform.SetAsLastSibling();
	}
	
	public void RemoveSplice(Instinct instinct)
	{
		Levels[instinct] -= 1;
		int childCount = LayoutMap[instinct].transform.childCount;
		Destroy(LayoutMap[instinct].transform.GetChild(childCount - 1).gameObject);
	}
	
	public void OnUp(Instinct i)
	{}
}
