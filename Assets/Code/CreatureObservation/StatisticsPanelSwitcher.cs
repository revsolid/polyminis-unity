using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatisticsPanelSwitcher : MonoBehaviour
{
    public GameObject DetailsPanel;
    public GameObject SummaryPanel;
    
    void Awake()
    {
        ShowSummary();
    }
    
    public void ShowDetails()
    {
        DetailsPanel.SetActive(true);
        SummaryPanel.SetActive(false);
    }
    
    public void ShowSummary()
    {
        DetailsPanel.SetActive(false);
        SummaryPanel.SetActive(true);
    }
}