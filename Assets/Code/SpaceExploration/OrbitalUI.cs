using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using CP.ProChart;

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
    public PieChart PopulationChart;
    public PieChart ResourceChart;
    public GameObject TextPopup;
    public int ChartSize;
    public SpeciesCard[] Slots;
    
    PlanetModel Planet;
    string SpeciesToOpen;
    bool HasHookedCallbacks = false;
    SpeciesModel ShowSpeciesNextUpdate = null;

    private ChartData1D PopulationData, ResourceData;
    private Dictionary<int, SpeciesModel> Species;
    private Dictionary<int,string> ResourceNames;

    private string PopupString;

    public delegate void BackToSpaceAction();
    public static event BackToSpaceAction OnGoBackToSpaceExScreen;

    // Use this for initialization
    void Awake()
    {
        
    }

    void OnEnable()
    {
        PopulationChart.onOverDelegate += OnPopulationChartHover;
        ResourceChart.onOverDelegate += OnMaterialChartHover;
    }

    void OnDisable()
    {
        PopulationChart.onOverDelegate -= OnPopulationChartHover;
        ResourceChart.onOverDelegate -= OnMaterialChartHover;
    }
    
    void OnDestroy()
    {
        if (HasHookedCallbacks)
        {
            global::Planet.OnPlanetModelChanged -= OnPlanetChanged;
            HasHookedCallbacks = false;
        }
    }
    void Start ()
    {
        PopupString = "";
    }
    
    // Update is called once per frame
    void Update ()
    {
        //TODO: This is ugly
        TextPopup.GetComponent<Text>().text = PopupString;

        if(PopulationChart.ChartSize < ChartSize)
        {
            const int speed = 5;
            PopulationChart.ChartSize += speed;
            
            ResourceChart.ChartSize += speed;
            if (PopulationChart.ChartSize > ChartSize) PopulationChart.ChartSize = ChartSize;
            if (ResourceChart.ChartSize > ChartSize) ResourceChart.ChartSize = ChartSize;
        }
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
            SpeciesEditor.OpenWithSpecies(ShowSpeciesNextUpdate, true);
            ShowSpeciesNextUpdate = null;
        }
    }
    
    public void OnBackClicked()
    {
        if(OnGoBackToSpaceExScreen != null)
        {
            OnGoBackToSpaceExScreen();
        }
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
        PlanetInfoCacher.planetModel = Planet;
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
        //PhSlider.value = planet.PH.Average() * 100;
        //TempSlider.value = planet.Temperature.Average() * 100;
        if (p != Planet)
        {
            PlanetName.text = p.PlanetName;
            PlanetRenderer.RenderUpdate(planet);
        }


        PopulationData = new ChartData1D();
        ResourceData = new ChartData1D();
        Species = new Dictionary<int, SpeciesModel>();
        ResourceNames = new Dictionary<int, string>();

        //Populate species chart widget
        PopulationData.Clear();
        PopulationData.Resize(1, p.Species.Count);
        Species.Clear();

        int j = 0;
        foreach(SpeciesModel sm in p.Species)
        {
            print(j);
            PopulationData[j] = sm.Percentage;
            Species.Add(j, sm);
            j++;
        }

        PopulationChart.SetValues(ref PopulationData);

        //  TODO:
        // Hijacking Resource Chart for Temperature
        // and heavily... all your resources are belong to us
        ResourceData.Clear();
        ResourceData.Resize(1, 3);
        ResourceNames.Clear();

        if (p.Temperature.Max == 0)
        {
            p.Temperature.Max = 1;
        }
        float scMin =  p.Temperature.Min / p.Temperature.Max;
        float scAverage =  p.Temperature.Average() / p.Temperature.Max;


        Debug.Log(scMin + " XXX " + scAverage + " XXX " + p.Temperature.Max);
        ResourceData[0] = scMin;
        ResourceNames.Add(0, "Low:\n"+ (p.Temperature.Min * 273).ToString("0.0")+" K");
        ResourceData[1] = scAverage - scMin;
        ResourceNames.Add(1, "Average:\n" + ((p.Temperature.Min +  p.Temperature.Max) * 273 / 2).ToString("0.0") + " K");
        ResourceData[2] = 1 - scAverage;
        ResourceNames.Add(2, "High:\n"+ (p.Temperature.Max * 273).ToString("0.0") +" K");

        PopulationChart.ChartSize = 0;
        ResourceChart.ChartSize = 0;

        ResourceChart.SetValues(ref ResourceData);

        for (int i=0; i < p.Species.Count; i++)
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

    void OnPopulationChartHover(int column)
    {
        Text t = TextPopup.GetComponentInChildren<Text>();

        if (column == -1)
        {
            PopupString = "";
            return;
        }
        
        PopupString = Species[column].Percentage.ToString("0.00")+ "% | " + Species[column].SpeciesName +"\n( " + Species[column].CreatorName + " )";
        t.color = PopulationChart.GetColor(column);
    }

    void OnMaterialChartHover(int column)
    {
        //TextPopup.SetActive(true);
        Text t = TextPopup.GetComponentInChildren<Text>();
        if (column == -1)
        {
            
            //PopupString = "";
            return;

        }

        
        PopupString = ResourceNames[column];
        t.color = ResourceChart.GetColor(column);
    }
}
