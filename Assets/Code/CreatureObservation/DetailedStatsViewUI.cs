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

    public LineChart MainChart;
    DetailedViewModel Model;
    ChartData2D Data;
    
    public Text MaxText; 
    public Text AvgText; 
    public Text MinText; 
    
    public Text[] Labels;
    
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
        for(i = 0; i < poss.Length && i < Labels.Length; ++i)
        {
            Labels[i].gameObject.SetActive(true);
            Labels[i].transform.localPosition = new Vector2(poss[i].position.x, Labels[i].transform.localPosition.y);
            Labels[i].text = Model.CurrentStartingEpoch + i + "";
        }
        for (; i < Labels.Length; ++i)
        {
            Labels[i].gameObject.SetActive(false);
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
    }
}
    