using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeWarpController : MonoBehaviour {

    public int StartEpoch;
    int EndEpoch;
    public ExperimentManager Experiment;

    public Button[] Buttons = new Button[10];
     
	// Use this for initialization
	void Start () {
        UpdateButtons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateButtons()
    {
        EndEpoch = 9 + StartEpoch;

        for (int i = StartEpoch; i <= EndEpoch; i++)
        {
            Buttons[i - StartEpoch].GetComponentInChildren<Text>().text = "" + i;
        }
    }

    public void OnForwardClicked()
    {
        StartEpoch += 10;
        UpdateButtons();
    }

    public void OnBackClicked()
    {
        if (StartEpoch - 10 <= 0) return;
        StartEpoch -= 10;
        UpdateButtons();
    }

    public void OnEpochButtonClicked(int buttonNumber)
    {
        Experiment.OnLoadExp("demo_" + Buttons[buttonNumber].GetComponentInChildren<Text>().text + "_1");
    }
}
