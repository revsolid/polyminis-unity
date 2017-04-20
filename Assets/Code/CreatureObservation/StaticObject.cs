using UnityEngine;
using System.Collections;

public class StaticObject : MonoBehaviour
{
	public int Height = 1;
	// Use this for initialization
	void Start ()
	{
		Vector3 newScale = transform.localScale;
		newScale.y = Height + Random.value * 5;
	//	transform.localScale = newScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Material material = GetComponentInChildren<Renderer>().material;
	//	float v = Height * 0.01f; 
	//	material.SetColor("_EmissionColor", new Color(v, v, v));
	}
}
