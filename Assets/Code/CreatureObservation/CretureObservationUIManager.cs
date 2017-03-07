using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CretureObservationUIManager : MonoBehaviour
{
	public PlanetModel ObservingPlanet;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void OnBackClicked()
	{
		SceneManager.LoadScene("space_exploration");
	}
}
