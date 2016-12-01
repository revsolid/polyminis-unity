using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OrbitalUI : MonoBehaviour
{
	public Camera SpaceflightCamera;
	public Camera OrbitalCamera;
	public GameObject SpeciesEditor;
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
	
	public void OnEditCreatureClicked()
	{
		SpeciesEditor.SetActive(true);
	}

    public void OnUIOpened(Planet p)
    {
        Debug.Log("XXXXX");
        Debug.Log(p);
        PhSlider.value = p.PH.Max;
        TempSlider.value = p.Temperature.Max;
        PlanetName.text = p.PlanetName;
        PlanetRenderer.GetComponent<PlanetCloseupRenderer>().RenderUpdate(p);

        //TODO: Add instantiation of Species "Cards" to SpeciesLayout

    }

    public void OnSpeciesCatalogClicked()
    {
        Debug.Log("clicked");
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