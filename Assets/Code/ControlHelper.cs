using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelper : MonoBehaviour
{

    public static float GetHorizontalAxis()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            return -1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            return 1.0f;
        }
        return 0;
    }

    public static float GetVerticalAxis()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            return 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            return -1.0f;
        }
        return 0;
    }
}
