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
	Instinct _dominantInstinct = Instinct.NOMADIC;
	public Instinct DominantInstinct { get { return _dominantInstinct; } set { _dominantInstinct = value; UpdateHead(); } }
	
	int _SpeciesIndex;
	public int SpeciesIndex { get { return _SpeciesIndex; } set { _SpeciesIndex = value; PrepareTexture(); }}
	
	
	//	TODO: This could be a custom editor... but anyway
	public GameObject[] Heads;
	public Instinct[] InstinctsToHeads;
	
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
	        rend.material.SetColor("_Color2", c);
	}
	
	void UpdateHead()
	{
		for(int i = 0; i < InstinctsToHeads.Length; ++i)
		{
			Heads[i].SetActive(false);
			if (DominantInstinct == InstinctsToHeads[i])	
			{
				Heads[i].SetActive(true);
			}
		}
	}
}
