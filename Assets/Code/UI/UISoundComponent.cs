using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


[AddComponentMenu("Wwise/UISoundElement")]
public class UISoundComponent : AkEvent 
{
    public int UIEventID; // = 0; 
    public string UIEventName; //  = ""; 
    bool CallbacksWired = false;
    string LastInputFieldValue = "NEVER_EVER_NEVER_EVER";
    


    void Awake()
    {
    }
    
    void Start()
    {
        UIEventID = eventID;
    }
    
    void Update()
    {
        if (!CallbacksWired)
        {
            Selectable sel = gameObject.GetComponent<Selectable>();
            
            Button b = sel as Button;
            if (b != null)
            {
                b.onClick.AddListener( () =>
                {
                    Debug.Log(UIEventID);
                    Debug.Log(eventID);
                    OnClick();
                });
                CallbacksWired = true;
            }
            
            Toggle t = sel as Toggle;
            if (t != null)
            {
                CallbacksWired = true;
            }
            
            InputField ifield = sel as InputField;
            if (ifield != null)
            {
                ifield.onValueChanged.AddListener( (string value) =>
                {
                    OnInputFieldChanged(value);
                });
            }
        }
    }
    
    void OnClick()
    {
        AkSoundEngine.PostEvent((uint)eventID, gameObject);
    }
    
    void OnInputFieldChanged(string v)
    {
        if (v != LastInputFieldValue)
        {
            AkSoundEngine.PostEvent((uint)eventID, gameObject);
            LastInputFieldValue = v;
        }
    }
    
    void OnDown()
    {
    } 
    
    void OnUp()
    {
    }
}