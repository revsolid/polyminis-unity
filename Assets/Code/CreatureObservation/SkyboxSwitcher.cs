using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour {

	public Material AcidCold;
	public Material AcidHot;
	public Material AcidTemp;

	public Material BasicCold;
	public Material BasicHot;
	public Material BasicTemp;

	public Material NeutralCold;
	public Material NeutralHot;
	public Material NeutralTemp;
	
	// Use this for initialization
	void Start()
	{
		Debug.Log("Starting Switcher");
		PlanetModel p = PlanetInfoCacher.planetModel;

        if (p != null)
        {
			Debug.Log("Planet found with some datas");
			Debug.Log(""+p.Temperature.Average());
			Debug.Log(""+p.Ph.Average());
			bool hot = p.Temperature.Average() > 0.55;
			bool cold = p.Temperature.Average() < 0.45; 
			bool acid = p.Ph.Average() > 0.6;
			bool basic = p.Ph.Average() < 0.4;
			
			
			if (hot)
			{
				if (acid)
				{
					 RenderSettings.skybox = AcidHot;	
				}
				else if (basic)
				{
					 RenderSettings.skybox = BasicHot;	
				}
				else
				{
					RenderSettings.skybox = NeutralHot;	
				}
			}
			else if (cold)
			{
				if (acid)
				{
					 RenderSettings.skybox = AcidCold;	
				}
				else if (basic)
				{
					 RenderSettings.skybox = BasicCold;	
				}
				else
				{
					RenderSettings.skybox = NeutralCold;	
				}
			}
			else
			{
				if (acid)
				{
					 RenderSettings.skybox = AcidTemp;	
				}
				else if (basic)
				{
					 RenderSettings.skybox = BasicTemp;	
				}
				else
				{
					RenderSettings.skybox = NeutralTemp;	
				}
			}
		}
	}
}
