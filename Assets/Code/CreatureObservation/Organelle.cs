using UnityEngine;
using System.Collections;

public class Organelle : MonoBehaviour
{
	
	public GameObject Sphere;
	public OrganelleModel OrganelleModel;
	public int SpeciesIndex = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Renderer rend = Sphere.GetComponent<Renderer>();
		// TMP
		Color c = new Color();
		c[SpeciesIndex % 3] = OrganelleModel.Trait.TID / 255.0f;
		c.a = 0.4f;
		if (rend != null)
	        rend.material.SetColor("_Color", c);
	}
}