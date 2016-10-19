using UnityEngine;
using System.Collections;

public class LoginUI : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void GoToSpaceScene()
	{
		Application.LoadLevel("space_exploration");
	}

	public void GoToObservationScene()
	{
		Application.LoadLevel("creature_observation");
	}
	
}
