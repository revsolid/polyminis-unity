using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoginUI : MonoBehaviour
{

    public InputField LoginField;
	
	UserServiceEvent EventToProcess;
    void Awake()
    {
        Connection.OnMessageEvent += OnServerMessage;
    }

    // Use this for initialization
    void Start ()
    {
    }
    
    // Update is called once per frame
    void Update ()
    {
		if (EventToProcess != null)
		{
			if (!EventToProcess.Result)	
			{
				OnLoginFailed();
			}
			else
			{
				OnLoginSuccessful();
			}

			EventToProcess = null;
		}
    }
    
    public void StartLogin()
    {
        var loginCommand = new UserServiceCommand(LoginField.text);
        Connection.Instance.Send(JsonUtility.ToJson(loginCommand));
    }

    void OnServerMessage(string message)
	{
		//
        UserServiceEvent userEvent = JsonUtility.FromJson<UserServiceEvent>(message);
		EventToProcess = userEvent;
	}
    
    void OnLoginFailed() 
	{
	}
    void OnLoginSuccessful() 
    {
		Session.Instance.UserName = EventToProcess.UserName;
		Session.Instance.LastKnownPosition = EventToProcess.LastKnownPosition;
		SceneManager.LoadScene("space_exploration");
    }
	
	void OnDestroy()
	{
        Connection.OnMessageEvent -= OnServerMessage;
	}

}
