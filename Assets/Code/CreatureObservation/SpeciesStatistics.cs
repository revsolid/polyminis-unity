using UnityEngine;
using CP.ProChart;
using System.Collections;
using System.Collections.Generic;

public class SpeciesStatistics : MonoBehaviour
{
    
    public LineChart Chart;
    ChartData2D SpeciesPercentageData = new ChartData2D();
    Dictionary<string, int> SpeciesNameToIndex = new Dictionary<string, int>();
    int NextIndex = 0;
    
    // TODO: HARDCODED
    int CurrentStartingEpoch = 50;
    int CurrentEndingEpoch = 1;
    
    void Awake()
    {
        SpeciesPercentageData[0,0] = 5;
        SpeciesPercentageData[0,1] = 50;
        SpeciesPercentageData[0,2] = 5;
        SpeciesPercentageData[0,3] = 50;
        SpeciesPercentageData[1,0] = 50;
        SpeciesPercentageData[1,1] = 5;
        SpeciesPercentageData[1,2] = 50;
        SpeciesPercentageData[1,3] = 5;
        SpeciesPercentageData[2,1] = 35;
        SpeciesPercentageData[2,2] = 10;
        SpeciesPercentageData[2,24] = 10;
        Chart.SetValues(ref SpeciesPercentageData);
        
        Connection.Instance.OnMessageEvent += OnServerMessage;
    }
    
    void OnDestroy()
    {
        Connection.Instance.OnMessageEvent -= OnServerMessage;
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
            
            SpeciesPercentageData.Clear();
            
            foreach(StatEntry perc in ev.EpochStats.Percentages)
            {
                
                int inx = -1;
                if (!SpeciesNameToIndex.TryGetValue(perc.SpeciesName, out inx))
                {
                    SpeciesNameToIndex[perc.SpeciesName] = NextIndex++;
                    inx = SpeciesNameToIndex[perc.SpeciesName];
                }
                SpeciesPercentageData[inx, ev.EpochStats.Epoch - CurrentStartingEpoch] = perc.Value;
            }
        }
    }
}