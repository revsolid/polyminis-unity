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
    public InventoryUI Inventory;
    
    public SpeciesCard[] Slots;
    
    PlanetModel Planet;
    string SpeciesToOpen;
    bool HasHookedCallbacks = false;
    SpeciesModel ShowSpeciesNextUpdate = null;

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
        //TODO: This is ugly
        if (!HasHookedCallbacks)
        {
            global::Planet.OnPlanetModelChanged += OnPlanetChanged;
            HasHookedCallbacks = true;
        }
        
        if (ShowSpeciesNextUpdate != null)
        {
            SpeciesDesignUI.OnSaveEvent += (resultingModel) => {
                Debug.Log(JsonUtility.ToJson(resultingModel));
                PlanetInteractionCommand editInPlanetCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.EDIT_IN_PLANET);
                editInPlanetCommand.Epoch = Planet.Epoch; 
                editInPlanetCommand.PlanetId = Planet.ID; 
                editInPlanetCommand.Species = resultingModel;
                Connection.Instance.Send(JsonUtility.ToJson(editInPlanetCommand));
            };
            SpeciesEditor.OpenWithSpecies(ShowSpeciesNextUpdate);
            ShowSpeciesNextUpdate = null;
        }
    }
    
    public void OnBackClicked()
    {
        OrbitalCamera.gameObject.SetActive(false);
        SpaceflightCamera.gameObject.SetActive(true);
        SpaceflightCamera.enabled = true;
    }
    
    public void OnEditCreatureClicked(SpeciesModel species)
    {
        
        PlanetInteractionCommand getEditInfoCommand = new PlanetInteractionCommand(PlanetInteractionCommandType.GET_TO_EDIT_IN_PLANET);
        getEditInfoCommand.Epoch = Planet.Epoch; 
        getEditInfoCommand.PlanetId = Planet.ID; 
        getEditInfoCommand.Species = species;
        Connection.Instance.Send(JsonUtility.ToJson(getEditInfoCommand));
        
        SpeciesToOpen = species.SpeciesName;
    }
    
    public void OnDeployCreatureClicked()
    {
        // Start Deploy Flow - First show the Inventory in Selection Mode
        InventoryUI.OnEntrySelected += (entryModel, slot) =>
        {
            // If something was selected, then ask for Biomass for Deployment 
            SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
            Debug.Log(entryModel.Value.SpeciesName);
            dialog.SpeciesModel = entryModel.Species;
            dialog.PlanetModel = Planet;
            dialog.CurrentAction = SpeciesPlanetAction.Deploy;
        };
        Inventory.ShowInMode(InventoryMode.SELECTION);
    }
    
    public void OnResearchCreatureClicked(SpeciesModel species)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.SpeciesModel = species;
        dialog.CurrentAction = SpeciesPlanetAction.Research;
    }
    
    public void OnExtractCreatureClicked(SpeciesModel species)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.SpeciesModel = species;
        dialog.CurrentAction = SpeciesPlanetAction.Extract;
    }
    
    public void OnSampleCreatureClicked(SpeciesModel species)
    {
        SpeciesPlanetDialog dialog = Instantiate(InteractionsDialogPrototype);
        dialog.PlanetModel = Planet;
        dialog.SpeciesModel = species;
        dialog.CurrentAction = SpeciesPlanetAction.Sample;
    }

    public void OnUIOpened(Planet p)
    {
        Debug.Log("On UI Opened");
        LoadFromPlanet(p);
    }
    
    public void OnObservePlanetClicked()
    {
        BaseCommand dummyCmd = new BaseCommand();
        dummyCmd.Service = "creature_observation";
        dummyCmd.Command = "GO_TO_EPOCH";
        Connection.Instance.Send(JsonUtility.ToJson(dummyCmd));
        SceneManager.LoadScene("creature_observation");
    }
    
    public void OnPlanetChanged(Planet inPlanet)
    {
        PlanetModel p = inPlanet.Model;
        if (p.ID == Planet.ID)
        {
            Planet = p;
            LoadFromPlanet(inPlanet);
            
            if (!string.IsNullOrEmpty(SpeciesToOpen))
            {
                SpeciesModel m = null;
                for (int i = 0; i < p.Species.Count; ++i)
                {
                    if (p.Species[i].SpeciesName == SpeciesToOpen)
                    {
                        m = p.Species[i];
                        break;
                    }
                }
                
                SpeciesToOpen = "";
                if (m != null)
                {
                    ShowSpeciesNextUpdate = m;
                }
                else
                {
                    Debug.LogError("OnPlanetChanged tried to open species: " + SpeciesToOpen + " but couldn't find it");
                }
            }
        }
    }
    
    void LoadFromPlanet(Planet planet)
    {
        PlanetModel p = planet.Model;
        PhSlider.value = planet.PH.Average() * 100;
        TempSlider.value = planet.Temperature.Average() * 100;
        PlanetName.text = p.PlanetName;
        PlanetRenderer.RenderUpdate(planet);
        
        for(int i=0; i < p.Species.Count; i++)
        {
            Debug.Log(p.Species[i].SpeciesName);
            Slots[i].Species = p.Species[i];
        }
        for(int i=p.Species.Count; i < Slots.Length; i++)
        {
            Slots[i].Species = null;
        }
        Planet = p;
    }
}
