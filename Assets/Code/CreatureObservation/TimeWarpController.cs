using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeWarpController : MonoBehaviour {

    public int StartEpoch;
    public GameObject ButtonPrototype;
    public HorizontalLayoutGroup TimeLine;
    int EndEpoch;
     
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void UpdateButtons()
    {
        /*
        Buttons[0].GetComponentInChildren<Text>().text =  "0";
        Buttons[1].GetComponentInChildren<Text>().text =  "10";
        Buttons[2].GetComponentInChildren<Text>().text =  "25";

        for (int i = 3; i < 10; i++)
        {
            Buttons[i].GetComponentInChildren<Text>().text =  "";
        }*/
    }

    public void OnForwardClicked()
    {
        ///StartEpoch += 10;
        //UpdateButtons();
    }

    public void OnBackClicked()
    {
        //if (StartEpoch - 10 <= 0) return;
        //StartEpoch -= 10;
        //UpdateButtons();
    }

    public void OnEpochButtonClicked(int buttonNumber)
    {
        var exps = new string[]{"demo_0_1", "demo_31_1", "demo_46_1"};
        //Experiment.OnLoadExp(exps[buttonNumber]);
//        Experiment.OnLoadExp("demo_" + Buttons[buttonNumber].GetcomponentInChildren<Text>().text = .GetComponentInChildren<Text>().text + "_1");
    }
}
