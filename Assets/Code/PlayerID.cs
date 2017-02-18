using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerID : MonoBehaviour
{
    public Text PlayerName;
    public Text Biomass;

    // Use this for initialization
    void Start ()
    {
        PlayerName.text = Session.Instance.UserName;
        Biomass.text = Session.Instance.Biomass + "";
		
        Session.OnSessionChangedEvent += () => OnSessionChanged();
    }
	
	void OnSessionChanged()
	{
        PlayerName.text = Session.Instance.UserName;
        Biomass.text = Session.Instance.Biomass + "";
	}
    
    // Update is called once per frame
    void Update ()
    {
        
    }
}
