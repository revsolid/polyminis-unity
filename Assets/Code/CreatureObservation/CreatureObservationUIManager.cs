using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CreatureObservationUIManager : MonoBehaviour
{
	public PlanetModel ObservingPlanet;

	// Use this for initialization
	void Start ()
	{
        ObservingPlanet = PlanetInfoCacher.planetModel;
        if (ObservingPlanet == null)
        {
            ObservingPlanet = new PlanetModel();
            ObservingPlanet.ID = 2011;
            ObservingPlanet.Epoch = 37;
			ObservingPlanet.PlanetName = "El Planeto";
        }
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	}
	
	public void OnBackClicked()
	{
		SceneManager.LoadScene("space_exploration");
	}
}
