using CP.ProChart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedViewModel
{
    public float MinValue = 100000;
    public float MaxValue = -100000;
    public float AvgValue { get {
        return (MinValue + MaxValue) / 2.0f;
    }}
    public int MinValueInx = -1;
    public int MaxValueInx = -1;
    
    public int CurrentStartingEpoch;
    public Dictionary<int, List<int>> EpochsToIndexes; 
    public Dictionary<int, string> IndexToSpeciesName;
    public PlanetModel PlanetModel;
    public int CurrentlyDisplayedEpoch;
}

public class SpeciesStatistics : MonoBehaviour
{
    
    public LineChart ChartSummary;
    public DetailedStatsViewUI DetailedView;
	public Text PlanetName;
	public Text PlanetNameShadow;
    
    ChartData2D SpeciesPercentageData = new ChartData2D();
    DetailedViewModel DetailedViewModel = new DetailedViewModel();
    Dictionary<string, int> SpeciesNameToIndex = new Dictionary<string, int>();
    Dictionary<int, string> IndexToSpeciesName = new Dictionary<int, string>();
    Dictionary<int, List<int>> EpochsToIndexes = new Dictionary<int, List<int>>();  
    int NextIndex = 0;
    
    int CurrentStartingEpoch = -1;
    int CurrentEndingEpoch = -1;
    
    PlanetModel Planet = null;
    
    public void Forward()
    {
        CreatureObservationCommand getStatsCmd;
        getStatsCmd = new CreatureObservationCommand(Planet.ID, CurrentEndingEpoch + 1);
        getStatsCmd.Command = "GET_STATS";
        Connection.Instance.Send(JsonUtility.ToJson(getStatsCmd));
    }
    
    public void Back()
    {
        CreatureObservationCommand getStatsCmd;
        getStatsCmd = new CreatureObservationCommand(Planet.ID, CurrentStartingEpoch - 1);
        getStatsCmd.Command = "GET_STATS";
        Connection.Instance.Send(JsonUtility.ToJson(getStatsCmd));
    }
    
    void Awake()
    {
    }
    void Start()
    {
        Connection.Instance.OnMessageEvent += OnServerMessage;
        SpeciesController.OnEpochLoaded += (int planetId, int epoch) => {
            DetailedViewModel.CurrentlyDisplayedEpoch = epoch;
        };
        Planet = PlanetInfoCacher.planetModel;
        if (Planet == null)
        {
            Planet = new PlanetModel();
            Planet.ID = 2011;
            Planet.Epoch = 33;
            Planet.PlanetName = "Il Nome";
        }
        DetailedViewModel.PlanetModel = Planet;
		PlanetName.text = Planet.PlanetName;
		PlanetNameShadow.text = Planet.PlanetName;

        ChartSummary.SetValues(ref SpeciesPercentageData);
        DetailedView.SetValues(ref SpeciesPercentageData, ref DetailedViewModel);
    }
    
    void OnEnable()
    {
        DetailedView.ElementHovered += OnChartElemHover;
        DetailedView.ElementSelected += OnChartElemSelect;
    }

    void OnDisable()
    {
        DetailedView.ElementHovered -= OnChartElemHover;
        DetailedView.ElementSelected -= OnChartElemSelect;
    }
    
    void OnDestroy()
    {
        if (Connection.Instance != null)
        {
            Connection.Instance.OnMessageEvent -= OnServerMessage;
        }
    }
    
    void OnChartElemHover(int row, int column)
    {
        //
    }
    void OnChartElemSelect(int row, int column)
    {
        //
    }
    
    void OnServerMessage(string msg)
    {
        BaseEvent base_ev = JsonUtility.FromJson<BaseEvent>(msg);
        // Avoid parsing creature_observation events that aren't EPOCH_STATS because those
        // can be real fat
        if (base_ev.Service == "creature_observation" && base_ev.EventString == "EPOCH_STATS")
        {
            CreatureObservationEvent ev = JsonUtility.FromJson<CreatureObservationEvent>(msg);
            
            if (ev.EpochStats.Epoch < 0)
            {
                return;
            }
            
            if (CurrentStartingEpoch == -1)
            {
                // Request the next surrounding 10 gens [Epoch-5, Epoch+5]
                CurrentStartingEpoch = (int) Mathf.Max(1.0f, ev.EpochStats.Epoch - 5);
                for (int i = 0; i < 10; ++i)                 
                {
                    CreatureObservationCommand getStatsCmd;
                    getStatsCmd = new CreatureObservationCommand(Planet.ID, CurrentStartingEpoch + i);
                    getStatsCmd.Command = "GET_STATS";
                    Connection.Instance.Send(JsonUtility.ToJson(getStatsCmd));
                }
            }
            else if (ev.EpochStats.Epoch < CurrentStartingEpoch && ev.EpochStats.Epoch > 0)
            {
                // "Shift" the chart data to the right 
                ChartData2D shiftedData = new ChartData2D();
                for(int i = 0; i < SpeciesPercentageData.Rows; i++) 
                {
                    for(int j = 0; j < SpeciesPercentageData.Columns - 1; j++) 
                    {
                        // Keep the same Indexes (Row), shift Epochs(Columns)
                        shiftedData[i,j+1] = SpeciesPercentageData[i,j];
                    }
                }
                SpeciesPercentageData = shiftedData; 
                CurrentStartingEpoch = ev.EpochStats.Epoch;
                DetailedView.SetValues(ref SpeciesPercentageData, ref DetailedViewModel);
                CurrentEndingEpoch -= 1;
            }
            
            EpochsToIndexes[ev.EpochStats.Epoch] = new List<int>();
            foreach(StatEntry perc in ev.EpochStats.Percentages)
            {
                
                int inx = -1;
                if (!SpeciesNameToIndex.TryGetValue(perc.SpeciesName, out inx))
                {
                    SpeciesNameToIndex[perc.SpeciesName] = NextIndex++;
                    inx = SpeciesNameToIndex[perc.SpeciesName];
                }
                IndexToSpeciesName[inx] = perc.SpeciesName;
                EpochsToIndexes[ev.EpochStats.Epoch].Add(inx);
                
                if (ev.EpochStats.Epoch - CurrentStartingEpoch >= 0)
                {
                    SpeciesPercentageData[inx, (int)Mathf.Max(0.0f, ev.EpochStats.Epoch - CurrentStartingEpoch)] = perc.Value;
                
                    if (perc.Value > DetailedViewModel.MaxValue)
                    {
                        DetailedViewModel.MaxValue = perc.Value;
                        DetailedViewModel.MaxValueInx = ev.EpochStats.Epoch;
                    }
                    if (perc.Value < DetailedViewModel.MinValue)
                    {
                        DetailedViewModel.MinValue = perc.Value;
                        DetailedViewModel.MinValueInx = ev.EpochStats.Epoch;
                    }
                }
            }
            if (ev.EpochStats.Epoch > CurrentEndingEpoch)
            {
                CurrentEndingEpoch = ev.EpochStats.Epoch;
                // Shift Left
                if (SpeciesPercentageData.Columns > 10)
                {
                    ChartData2D shiftedData = new ChartData2D();
                    for(int i = 0; i < SpeciesPercentageData.Rows; i++) 
                    {
                        for(int j = 1; j < SpeciesPercentageData.Columns; j++) 
                        {
                            // Keep the same Indexes (Row), shift Epochs(Columns)
                            shiftedData[i,j-1] = SpeciesPercentageData[i,j];
                        }
                    }
                    SpeciesPercentageData = shiftedData; 
                    DetailedView.SetValues(ref SpeciesPercentageData, ref DetailedViewModel);
                    CurrentStartingEpoch += 1;
                }
            }
            DetailedViewModel.CurrentStartingEpoch = (int)Mathf.Max(1.0f, CurrentStartingEpoch);
            DetailedViewModel.EpochsToIndexes = EpochsToIndexes;
            DetailedViewModel.IndexToSpeciesName = IndexToSpeciesName;
            
        }
    }
}