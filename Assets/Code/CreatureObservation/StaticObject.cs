using UnityEngine;
using System.Collections;

public class StaticObject : MonoBehaviour
{
	//
	public GameObject[] Options;
	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < Options.Length; ++i)
		{
			Options[i].SetActive(false);
		}
		
		int op = Random.Range(0, Options.Length);
		Options[op].SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
