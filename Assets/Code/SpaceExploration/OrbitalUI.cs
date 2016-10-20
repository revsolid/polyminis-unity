using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrbitalUI : MonoBehaviour
{
	public Camera SpaceflightCamera;
	public Camera OrbitalCamera;
	public GameObject SpeciesEditor;
    public Slider PhSlider;
    public Slider TempSlider;
    public Text PlanetName;
    public VerticalLayoutGroup SpeciesLayoutGroup;
    public GameObject SpeciesCatalouge;

	// Use this for initialization
	void Start ()
	{ 
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void OnBackClicked()
	{
        OrbitalCamera.gameObject.SetActive(false);
        SpaceflightCamera.gameObject.SetActive(true);
        SpaceflightCamera.enabled = true;
	}
	
	public void OnEditCreatureClicked()
	{
		SpeciesEditor.SetActive(true);
	}

    public void OnUIOpened(Planet p)
    {
        PhSlider.value = p.PH.Max;
        TempSlider.value = p.Temperature.Max;
        PlanetName.text = p.PlanetName;

        //TODO: Add instantiation of Species "Cards" to SpeciesLayout

    }

    public void OnSpeciesCatalougeClicked()
    {
        SpeciesCatalouge.SetActive(!SpeciesCatalouge.activeInHierarchy);
    }
    
    public void OnObservePlanetClicked()
    {
        Application.LoadLevel("creature_observation");
    }
}