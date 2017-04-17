using UnityEngine;
using CP.ProChart;
using System.Collections;
using System.Collections.Generic;

public class DetailedViewModel
{
    public float MinValue = 100000;
    public float MaxValue = -100000;
    public float AvgValue { get {
        return (MinValue + MaxValue) / 2.0f;
    }}
    
    public int CurrentStartingEpoch;
}

public class SpeciesStatistics : MonoBehaviour
{
    
    public LineChart ChartSummary;
    public DetailedStatsViewUI DetailedView;
    
    ChartData2D SpeciesPercentageData = new ChartData2D();
    DetailedViewModel DetailedViewModel = new DetailedViewModel();
    Dictionary<string, int> SpeciesNameToIndex = new Dictionary<string, int>();
    Dictionary<int, string> IndexToSpeciesName = new Dictionary<int, string>();
    int NextIndex = 0;
    
    int CurrentStartingEpoch = -1;
    int CurrentEndingEpoch = -1;
    
    void Awake()
    {
        Connection.Instance.OnMessageEvent += OnServerMessage;
    }
    void Start()
    {
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
        Debug.Log("Msg on Species Controller");
        BaseEvent base_ev = JsonUtility.FromJson<BaseEvent>(msg);
        // Avoid parsing creature_observation events that aren't EPOCH_STATS because those
        // can be real fat
        if (base_ev.Service == "creature_observation" && base_ev.EventString == "EPOCH_STATS")
        {
            CreatureObservationEvent ev = JsonUtility.FromJson<CreatureObservationEvent>(msg);
            
            if (CurrentStartingEpoch == -1)
            {
                CurrentStartingEpoch = ev.EpochStats.Epoch;
                // This is madness
                for (int i = -4; i < 4; ++i)                 
                {
                    CreatureObservationCommand getStatsCmd;
                    // TODO HARDCODED
                    getStatsCmd = new CreatureObservationCommand(2011, CurrentStartingEpoch + i);
                    getStatsCmd.Command = "GET_STATS";
                    Connection.Instance.Send(JsonUtility.ToJson(getStatsCmd));
                }
            }
            
            if (CurrentEndingEpoch == -1 && ev.EpochStats.Epoch >= CurrentStartingEpoch) 
            {
                CurrentEndingEpoch = ev.EpochStats.Epoch;
            }

           // SpeciesPercentageData.Clear();
            
            foreach(StatEntry perc in ev.EpochStats.Percentages)
            {
                
                int inx = -1;
                if (!SpeciesNameToIndex.TryGetValue(perc.SpeciesName, out inx))
                {
                    SpeciesNameToIndex[perc.SpeciesName] = NextIndex++;
                    inx = SpeciesNameToIndex[perc.SpeciesName];
                }
                IndexToSpeciesName[inx] = perc.SpeciesName;
                if (ev.EpochStats.Epoch - (CurrentStartingEpoch - 4) >= 1)
                {
                    SpeciesPercentageData[inx, (int)Mathf.Max(0.0f, ev.EpochStats.Epoch - (CurrentStartingEpoch - 4))] = perc.Value;
                
                    if (perc.Value > DetailedViewModel.MaxValue)
                    {
                        DetailedViewModel.MaxValue = perc.Value;
                    }
                    if (perc.Value < DetailedViewModel.MinValue)
                    {
                        DetailedViewModel.MinValue = perc.Value;
                    }
                    DetailedViewModel.CurrentStartingEpoch = (int)Mathf.Max(1.0f, CurrentStartingEpoch - 4);
                }
            }
        }
    }
}