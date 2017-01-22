using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


[AddComponentMenu("Wwise/UISoundElement")]
public class UISoundComponent : AkEvent 
{
    public int UIEventID = 0; 
    public string UIEventName = ""; 
    void Awake()
    {
    }
    
    void Start()
    {
        Selectable sel = gameObject.GetComponent<Selectable>();
        
        Button b = sel as Button;
        if (b != null)
        {
            b.onClick.AddListener( () =>
            {
                Debug.Log(UIEventName);
                OnClick();
            });
        }
        
        Toggle t = sel as Toggle;
        if (t != null)
        {

        }
    }
    
    void Update()
    {
    }
    
    void OnClick()
    {
        AkSoundEngine.PostEvent((uint)eventID, gameObject);
    }
    
    void OnDown()
    {
    } 
    
    void OnUp()
    {
    }
}