using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PolyminisDialog : MonoBehaviour
{
    public string DialogString = "Placeholder Message.";
    public string LeftButtonString = "Ok";
    public string RightButtonString = "Cancel";

    public UnityEvent OnLeftButton;
    public UnityEvent OnRightButton;

    private void OnEnable()
    {
        this.transform.GetChild(0).GetChild(0).FindChild("DialogMessage").GetComponent<Text>().text = DialogString;
        this.transform.GetChild(0).GetChild(0).FindChild("LeftButton").GetChild(0).GetComponent<Text>().text = LeftButtonString;
        this.transform.GetChild(0).GetChild(0).FindChild("RightButton").GetChild(0).GetComponent<Text>().text = RightButtonString;
    }

    public void OnLeftButtonClicked()
    {
        OnLeftButton.Invoke();
    }

    public void OnRightButtonClicked()
    {
        OnRightButton.Invoke();
    }
}
