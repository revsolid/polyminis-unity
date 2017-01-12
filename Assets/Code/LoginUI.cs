using UnityEngine;
using UnityEngine.SceneManagement;
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
		SceneManager.LoadScene("space_exploration");
	}

}
