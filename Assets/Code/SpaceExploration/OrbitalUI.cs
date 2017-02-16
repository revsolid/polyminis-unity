using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OrbitalUI : MonoBehaviour
{
	public Camera SpaceflightCamera;
	public Camera OrbitalCamera;
	public SpeciesDesignUI SpeciesEditor;
    public GameObject PlanetRenderer;
    public Slider PhSlider;
    public Slider TempSlider;
    public Text PlanetName;
    public VerticalLayoutGroup SpeciesLayoutGroup;
    public SpeciesCatalog SpeciesCatalog;

	// Use this for initialization
    void Awake()
    {
        
    }
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
	
	public void OnEditCreatureClicked(string speciesname)
	{
		SpeciesEditor.OpenWithSpecies(speciesname);
	}

    public void OnUIOpened(Planet p)
    {
        PhSlider.value = p.PH.Average() * 100;
        TempSlider.value = p.Temperature.Average() * 100;
        PlanetName.text = p.PlanetName;
        PlanetRenderer.GetComponent<PlanetCloseupRenderer>().RenderUpdate(p);
        
        Debug.Log(">>>>>>" + p.PlanetName);

        //TODO: Add instantiation of Species "Cards" to SpeciesLayout

    }

    public void OnSpeciesCatalogClicked()
    {
        bool toggleOn = !SpeciesCatalog.gameObject.activeInHierarchy;
        SpeciesCatalog.gameObject.SetActive(toggleOn);
        
        if (toggleOn)
        {
            SpeciesCatalog.Populate(Session.Instance.Species);
        }
    }
    
    public void OnObservePlanetClicked()
    {
        SceneManager.LoadScene("creature_observation");
    }
}