using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
	//TODO: Abstract this pattern of persistent singleton OR highlander
	private static bool PersistentSingletonStarted = false; 
	
	private bool ChangeTrack = false;
	private bool UnloadBank = false;

	void Awake()
	{
		if (!PersistentSingletonStarted)
		{
    		DontDestroyOnLoad(this);

    		AkBankManager.LoadBank("Music.bnk", false, false);
			// Only UnloadBank if this instance loaded it
			UnloadBank = true;

    		SceneManager.activeSceneChanged += OnSceneChanged;
			ChangeTrack = true;
			PersistentSingletonStarted = true;
		}
		else
		{
			Destroy(this);
		}
	}
	
	void OnDestroy()
	{
		if (UnloadBank)
		{
    		SceneManager.activeSceneChanged -= OnSceneChanged;	
    		AkBankManager.UnloadBank("Music.bnk");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ChangeTrack)
		{
			PlayForScene(SceneManager.GetActiveScene().name);
			ChangeTrack = false;
		}
	}
	
	void OnSceneChanged(Scene prev_sc, Scene new_sc)
	{
		ChangeTrack = true;
	}
	
	void PlayForScene(string scene_name)
	{
		string event_name = "";
		if (scene_name == "pregame")
		{
			event_name = "Play_intro_theme";
		}
		else if (scene_name == "space_exploration")
		{
			event_name = "Play_exploration";
		}
		else if (scene_name == "creature_observation")
		{
			event_name = "Play_creature_observation";
		}
		AkSoundEngine.PostEvent(event_name, gameObject);
	}
}