using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject StarMap;
	bool StarMapToggle = false;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		StarMap.SetActive(StarMapToggle);
	}

	public void OnButtonClick()
	{
		StarMapToggle = !StarMapToggle;
	}
}
