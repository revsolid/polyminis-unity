using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarFiller : MonoBehaviour {

	public GameObject[] slivers;
	public int Value;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i = 0; i < slivers.Length; i++)	
		{
			slivers[i].SetActive(false);
		}
		for(int i = 0; i < Value && i < slivers.Length; i++)	
		{
			slivers[i].SetActive(true);
		}
	}
}
