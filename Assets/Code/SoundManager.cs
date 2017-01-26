using UnityEngine;


public class SoundManager : MonoBehaviour
{
    
    public string[] BankNames = {}; 
    
    
	private static bool PersistentSingletonStarted = false; 
	private bool UnloadBank = false;

	void Awake()
	{
		if (!PersistentSingletonStarted)
		{
            for (int i = 0; i < BankNames.Length; ++i)
            {
        		AkBankManager.LoadBank(BankNames[i]+".bnk", false, false);
                Debug.Log("Loaded " + BankNames[i]+".bnk");
            }
			// Only UnloadBank if this instance loaded it
			UnloadBank = true;
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
            for (int i = 0; i < BankNames.Length; ++i)
            {
        		AkBankManager.UnloadBank(BankNames[i]+".bnk");
            }
		}
	}
}