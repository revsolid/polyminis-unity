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
    
    public Button RightButton;
    public Button LeftButton;
    public Text   DialogMessage;

    private void OnEnable()
    {
        UpdateTextOnElements();
        SetStarmapRendererBlock();
    }
    
    private void UpdateTextOnElements()
    {
        DialogMessage.text = DialogString;
        LeftButton.GetComponentInChildren<Text>().text = LeftButtonString;
        RightButton.GetComponentInChildren<Text>().text = RightButtonString;
    }
    
    public void Update()
    {
        UpdateTextOnElements();
    }

    public void OnLeftButtonClicked()
    {
        OnLeftButton.Invoke();
    }

    public void OnRightButtonClicked()
    {
        OnRightButton.Invoke();
    }

    private void SetStarmapRendererBlock()
    {
        foreach (StarmapRenderer s in FindObjectsOfType<StarmapRenderer>())
        {
            s.BlockingDialog = this.gameObject;
        }
    }
}
