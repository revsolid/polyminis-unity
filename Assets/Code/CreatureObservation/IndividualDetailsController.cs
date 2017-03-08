using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CP.ProChart;

public class IndividualDetailsController : MonoBehaviour {

    public BarChart PerformanceChart;
    ChartData2D PerformanceData;
	// Use this for initialization
	void Start () {
        PerformanceData = new ChartData2D();
        PerformanceData.Resize(2, 2);
        PerformanceChart.SetValues(ref PerformanceData);
        PerformanceData[0, 0] = 0.5f;
        PerformanceData[1, 0] = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {
		
	}


   public void LoadIndividual(IndividualModel individual)
    {
        //some code here
    }
}
