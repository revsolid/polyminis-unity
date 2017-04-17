using UnityEngine;
using System.Collections;


public class NucleusModel
{
	//
	public NucleusModel(int i){}
}


public class Nucleus : MonoBehaviour
{

	public NucleusModel NucleusModel;
	int _SpeciesIndex;
	public int SpeciesIndex { get { return _SpeciesIndex; } set { _SpeciesIndex = value; PrepareTexture(); }}
	
	// Use this for initialization
	void Start ()
	{
		PrepareTexture();
	}
	
	void PrepareTexture()
	{
	    Renderer rend = GetComponentInChildren<Renderer>();
		// TMP
		Color c = new Color();
		c[SpeciesIndex % 3] = 0.8f;
		c.a = 0.4f;
		if (rend != null)
	        rend.material.SetColor("_EmissionColor", c);
	}
}
