using UnityEngine;
using System.Collections;

public class Organelle : MonoBehaviour
{
	
	public GameObject Sphere;
	public OrganelleModel OrganelleModel;
	protected int _SpeciesIndex;
	public int SpeciesIndex { get { return _SpeciesIndex; } set { _SpeciesIndex = value; PrepareTexture(); }}

	// Use this for initialization
	protected void Start ()
	{
		PrepareTexture();
	}
	
	protected void PrepareTexture()
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