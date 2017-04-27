using CP.ProChart;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedStatsViewUI : MonoBehaviour
{
    public delegate void OnElemHover(int row, int column);
    public event OnElemHover ElementHovered; 
    public delegate void OnElemSelect(int row, int column);
    public event OnElemSelect ElementSelected; 
    
    public SpeciesController SpeciesController;

    public LineChart MainChart;
    DetailedViewModel Model;
    ChartData2D Data;
    
    public Text PlanetName;
    public Text EpochText;
    public Text MaxText; 
    public Text AvgText; 
    public Text MinText; 
    
    public Button GoToEpochButton;
    
    public EpochDetailDataGrid DataGrid; 

    public Text[] Labels;
    public Button EpochLabelButtonPrototype;
    public Transform LabelsAnchor;
    public Transform EpochMarker;
    Button[] EpochButtons = new Button[10];
    int SelectedEpoch;
    
    void Start()
    {
        for(int i = 0; i < EpochButtons.Length; i++)
        {
            EpochButtons[i] = GameObject.Instantiate(EpochLabelButtonPrototype);
            EpochButtons[i].gameObject.SetActive(false);
            EpochButtons[i].transform.parent = LabelsAnchor;
            EpochButtons[i].transform.localPosition = Vector3.zero;
        }
    }
    
    public void SetValues(ref ChartData2D data, ref DetailedViewModel model)
    {
        Debug.Log("Set Values");
        MainChart.SetValues(ref data);
        Model = model;
        
        if (Data != null)
        {
            Data.onDataChangeDelegate -= OnDataChanged;
        }

        Data = data;
        Data.onDataChangeDelegate += OnDataChanged; 
    }
    
    void Update()
    {
        MaxText.text = Model.MaxValue.ToString("####.#%");
        AvgText.text = Model.AvgValue.ToString("####.#%");
        MinText.text = Model.MinValue.ToString("####.#%");
        
        var poss =  MainChart.GetAxisXPositions();
        if (poss == null)
        {
            return;
        }
        int i;
        EpochMarker.gameObject.SetActive(false);
        for(i = 0; i < poss.Length && i < Labels.Length; ++i)
        {
            Labels[i].gameObject.SetActive(true);
            Labels[i].transform.localPosition = new Vector2(poss[i].position.x, Labels[i].transform.localPosition.y);
            Labels[i].text = Model.CurrentStartingEpoch + i + "";
            
     //       EpochButtons[i].gameObject.SetActive(true);
            EpochButtons[i].transform.localPosition = new Vector2(poss[i].position.x, EpochButtons[i].transform.localPosition.y);
            
            if ((Model.CurrentStartingEpoch + i) == Model.CurrentlyDisplayedEpoch)
            {
                EpochMarker.localPosition = new Vector2(poss[i].position.x, EpochMarker.localPosition.y);
                EpochMarker.gameObject.SetActive(true);
            }
        }
        
        for (; i < Labels.Length; ++i)
        {
            Labels[i].gameObject.SetActive(false);
        }
        
        PlanetName.text = Model.PlanetModel.PlanetName;
        EpochText.text = string.Format("Currently Observing: {0}", Model.CurrentlyDisplayedEpoch);
        
        var posy = MainChart.GetAxisYPositions(); 
        if (posy == null)
        {
            return;
        }
        int len = posy.Length; 
        Debug.Log("XXXXX");
        Debug.Log(len);
        int inx = Model.CurrentlyDisplayedEpoch - Model.MaxValueInx;
        if (inx > 0 && inx < posy.Length)
        {
            MaxText.transform.localPosition = new Vector2(MaxText.transform.localPosition.x, posy[inx].position.y);
        }
        inx = Model.CurrentlyDisplayedEpoch - Model.MinValueInx;
        if (inx > 0 && inx < posy.Length)
        {
            MinText.transform.localPosition = new Vector2(MinText.transform.localPosition.x, posy[inx].position.y);
        }
        AvgText.transform.localPosition = new Vector2(AvgText.transform.localPosition.x, (MaxText.transform.localPosition.y +  MinText.transform.localPosition.y)/2.0f);
        for(int k = 0; k < len; k++)
        {
            Debug.Log(posy[k].position.y);
        }
    }
        
    void OnDataChanged()
    {
        Debug.Log("Data Changed");
    }
    
    void OnEnable()
    {
        MainChart.onOverDelegate += OnChartHover;
        MainChart.onSelectDelegate += OnChartSelect;
    }

    void OnDisable()
    {
        MainChart.onOverDelegate -= OnChartHover;
        MainChart.onSelectDelegate -= OnChartSelect;
    }

    void OnChartHover(int row, int column)
    {
        if (ElementHovered != null)
        {
            ElementHovered(row, column);
        }
    }
    
    void OnChartSelect(int row, int column)
    {
        if (ElementSelected != null)
        {
            ElementSelected(row, column);
        }
        
        GoToEpochButton.gameObject.SetActive(true);
        
        int epoch = column + Model.CurrentStartingEpoch;
        int len = Model.EpochsToIndexes[epoch].Count();
        List<string> names = new List<string>(len);
        List<float> values = new List<float>(len);
        List<float> deltas = new List<float>(len);
        for(int i = 0; i < len; i++) 
        {
            int inx = Model.EpochsToIndexes[epoch][i];
            names.Add(Model.IndexToSpeciesName[inx]);
            values.Add(Data[inx, column]);

            if (column > 0)
            {
                deltas.Add(values[i] - Data[inx, column - 1]);
            }
            else
            {
                deltas.Add(0.0f);
            }
        }
        DataGrid.UpdateGrid(names, values, deltas);
        
        SelectedEpoch = epoch;
        GoToEpochButton.GetComponentInChildren<Text>().text = string.Format("{0}", epoch);
    }
    
    public void OnObserveEpoch()
    {
        SpeciesController.GoToEpoch(Model.PlanetModel.ID, SelectedEpoch);
    }
}