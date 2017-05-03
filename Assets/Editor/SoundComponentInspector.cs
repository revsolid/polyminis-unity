using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(UISoundComponent))]
public class UISoundComponentInspector : AkBaseInspector
{
	SerializedProperty eventID;
    SerializedProperty UIEventID;
	AkEvent m_akEvent;
    int m_selected = 0;
    UISoundComponent m_uiComponent { get { return m_akEvent as UISoundComponent; } }
    
	AkUnityEventHandlerInspector m_UnityEventHandlerInspector = new AkUnityEventHandlerInspector();
    
    public void OnEnable()
	{
		m_akEvent = target as AkEvent;
		
		m_UnityEventHandlerInspector.Init(serializedObject);
		
		eventID				= serializedObject.FindProperty("eventID");
        UIEventID           = serializedObject.FindProperty("UIEventID");
		
		m_guidProperty		= new SerializedProperty[1];
		m_guidProperty[0]	= serializedObject.FindProperty("valueGuid.Array");
		
		//Needed by the base class to know which type of component its working with
		m_typeName		= "Event";
		m_objectType	= AkWwiseProjectData.WwiseObjectType.EVENT;
	}
    
    public override void OnChildInspectorGUI ()
	{
        serializedObject.Update (); 
        GuiForComponent(m_akEvent.gameObject.GetComponent<Selectable>());
    }
    
    public override string UpdateIds (Guid[] in_guid)
	{
		for(int i = 0; i < AkWwiseProjectInfo.GetData().EventWwu.Count; i++)
		{
			AkWwiseProjectData.Event e = AkWwiseProjectInfo.GetData().EventWwu[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[0]));
			
			if(e != null)
			{
				serializedObject.Update();
				eventID.intValue = e.ID;
				serializedObject.ApplyModifiedProperties();

				return e.Name;
			}
		}

		return string.Empty;
	}
    
    private void GuiForComponent(Selectable s)
    {
        string[] options = { "No GUI Element in this Object" };
        Button b = (s as Button);
        if (b != null)
        {
            string[] opts = { "Click" };
            options = opts;
        }
        
        Toggle t = (s as Toggle);
        if (t != null)
        {
            string[] opts = { "Check", "Uncheck" };
            options = opts;
        }
        
        InputField ifield = (s as InputField);
        if (ifield != null)
        {
            string[] opts = { "OnChanged" };
            options = opts;
        }
        
	    serializedObject.Update();
        UIEventID.intValue = m_selected;
        serializedObject.ApplyModifiedProperties();
        m_selected = EditorGUILayout.Popup("Trigger On", m_selected,  options);
        m_uiComponent.UIEventName = options[m_selected];
    }
}