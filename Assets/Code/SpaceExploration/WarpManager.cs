using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour {

    public AnimationCurve WarpCostCurve;
    public float MaxDistance;
    public float BaseCost;
	// Use this for initialization
	void Start () {

	}


    public float CalculateCost( float distance )
    {
        float percent;
        if (distance <= 0) percent = 0;
        else if (distance > MaxDistance) percent = 1;
        else
        {
            percent = distance / MaxDistance;
        }

        return BaseCost * WarpCostCurve.Evaluate(percent);
    }
	
	
}
