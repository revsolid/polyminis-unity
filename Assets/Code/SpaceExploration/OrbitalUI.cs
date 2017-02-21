using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OrbitalUI : MonoBehaviour
{
	public Camera SpaceflightCamera;
	public Camera OrbitalCamera;
	public SpeciesDesignUI SpeciesEditor;
    public PlanetCloseupRenderer PlanetRenderer;
    public Slider PhSlider;
    public Slider TempSlider;
    public Text PlanetName;
    public SpeciesPlanetDialog InteractionsDialogPrototype;
    
    public SpeciesCard[] Slots;
    
    PlanetModel Planet;

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
	
	public void OnEditCreatureClicked(SpeciesModel species)
	{
		SpeciesEditor.OpenWithSpecies(species);
	}
    
    public void OnDeployCreatureClicked()
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.CurrentAction = SpeciesPlanetAction.Deploy;
    }
    
    public void OnResearchCreatureClicked(string speciesName)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.CurrentAction = SpeciesPlanetAction.Research;
    }
    
    public void OnExtractCreatureClicked(string speciesName)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.CurrentAction = SpeciesPlanetAction.Extract;
    }
    
    public void OnSampleCreatureClicked(string speciesName)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.CurrentAction = SpeciesPlanetAction.Sample;
    }

    public void OnUIOpened(Planet p)
    {
        PhSlider.value = p.PH.Average() * 100;
        TempSlider.value = p.Temperature.Average() * 100;
        PlanetName.text = p.PlanetName;
        PlanetRenderer.RenderUpdate(p);
        
        for(int i=0; i < p.Model.Species.Count; i++)
        {
            Debug.Log(p.Model.Species[i].SpeciesName);
            Slots[i].Species = p.Model.Species[i];
        }
        Planet = p.Model;
    }

    public void OnSpeciesCatalogClicked()
    {
    }
    
    public void OnObservePlanetClicked()
    {
        SceneManager.LoadScene("creature_observation");
    }
}