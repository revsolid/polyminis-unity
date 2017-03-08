using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
enum AdminCommandType
{
    RELOAD_FROM_DB,
    SAVE_CURRENT_TO_DB,
    UPLOAD_GAME_RULES,
    GET_FROM_SERVER
}

[Serializable]
class PmAdminCommand : BaseCommand
{
    AdminCommandType CommandType;
    // Cheating, using this command also as the event as 90% of the fields are the same
    public string EventString;
    public List<float> WarpCostCurveKeyframes = new List<float>();
    public AnimationCurve WarpCostCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));
    public List<float> PercentageToBiomassCurveKeyframes = new List<float>();
    public AnimationCurve PercentageToBiomassCostCurve;
    public List<float> BiomassToPercentageCurveKeyframes = new List<float>();
    public AnimationCurve BiomassToPercentageCostCurve;
    public int BaseWarpCost;
    public int WarpCostMultiplier;
    public int MaxWarpDistance;
    
    public PmAdminCommand(AdminCommandType commandType)
    {
        CommandType = commandType; 
        Command = CommandType.ToString();
        Service = "admin";
    } 
}

public class PmAdminPanel : EditorWindow
{
    
	[MenuItem ("Polyminis/Window/PmAdminPanel")]
	static void Init ()
    {
		// Get existing open window or if none, make a new one:
		PmAdminPanel window = (PmAdminPanel)EditorWindow.GetWindow (typeof (PmAdminPanel), false, "Polyminis Powerful Admin Panel");
        window.HookCallbacks();
		window.Show();
	}
    
    AnimationCurve WarpCostCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));
    public int SlicesForCurve = 10;

    public int BaseWarpCost = 200;
    public int WarpCostMultiplier = 1;
    public int MaxWarpDistance = 600;
    
    void HookCallbacks()
    {
  /*      Connection.OnMessageEvent += (msg) =>
        {
            PmAdminCommand ev = JsonUtility.FromJson<PmAdminCommand>(msg);
            
            if (ev.EventString == "ADMIN_GAME_RULES")
            {
                BaseWarpCost = ev.BaseWarpCost;
                WarpCostMultiplier = ev.WarpCostMultiplier;
                MaxWarpDistance = ev.MaxWarpDistance;
                if (ev.WarpCostCurve != null)
                {
                    //TODO: WarpCostCurve = ev.WarpCostCurve;
                }
            }
        }; */
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("WARPING RULES");
        EditorGUILayout.LabelField("Cost Calculation BaseCost + WarpCostMultiplier *");
        EditorGUILayout.LabelField("EvalCurve(WarpDistance / MaxWarpDistance)");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base Warp Cost");
        BaseWarpCost = (int)EditorGUILayout.Slider(BaseWarpCost, 50, 500);
        EditorGUILayout.LabelField("Warp Cost Multiplier");
        WarpCostMultiplier = (int)EditorGUILayout.Slider(WarpCostMultiplier, 1, 5);
        EditorGUILayout.LabelField("Max Warp Distance");
        MaxWarpDistance = (int)EditorGUILayout.Slider(MaxWarpDistance, 300, 1000);
        EditorGUILayout.CurveField(WarpCostCurve);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("SERVER");
        //EditorGUILayout.LabelField(Connection.Instance.Address);
        if (GUILayout.Button("Upload Current Ruleset To Server"))
        {
            EditorApplication.delayCall += () => { SaveToServer(AdminCommandType.UPLOAD_GAME_RULES); };
        }
        if (GUILayout.Button("Commit Current Server To Database"))
        {
            EditorApplication.delayCall += () => { SaveToServer(AdminCommandType.SAVE_CURRENT_TO_DB); };
        }
        if (GUILayout.Button("Reload Current Server Ruleset from Database"))
        {
            EditorApplication.delayCall += () => { SaveToServer(AdminCommandType.RELOAD_FROM_DB); };
        }
        if (GUILayout.Button("Load the current Server Configuration"))
        {
            EditorApplication.delayCall += () => { SaveToServer(AdminCommandType.GET_FROM_SERVER); };
        }
        
        EditorGUILayout.Space();
        // Advanced Stuffz
        EditorGUILayout.LabelField("ADVANCED");
        EditorGUILayout.LabelField("Granularity for Curve Slices");
        SlicesForCurve = (int)EditorGUILayout.Slider(SlicesForCurve, 1, 20);

        EditorGUILayout.Space();
        if (GUILayout.Button("Refresh Server Connection"))
        {
            EditorApplication.delayCall += () => { 
             //   Connection.Instance.CloseConnection();
             //   Debug.Log(Connection.Instance.Address);
             //   HookCallbacks();
            };
        }
    }
    
    void SaveToServer(AdminCommandType commType)
    {
        PmAdminCommand comm = new PmAdminCommand(commType);
        
        switch (commType)
        {
            case AdminCommandType.UPLOAD_GAME_RULES:
                float t = 0.0f;
                for(int i = 0; i < SlicesForCurve; i++)
                {
                    comm.WarpCostCurveKeyframes.Add(WarpCostCurve.Evaluate(t));
                    t += (1.0f/SlicesForCurve);
                }
                comm.BaseWarpCost = BaseWarpCost;
                comm.WarpCostMultiplier = WarpCostMultiplier;
                comm.MaxWarpDistance = MaxWarpDistance;
                comm.WarpCostCurve = WarpCostCurve;
                break;
            case AdminCommandType.SAVE_CURRENT_TO_DB:
                break;
            case AdminCommandType.RELOAD_FROM_DB:
                break;
            default:
                break;
        }
        Debug.Log(JsonUtility.ToJson(comm)); 
       // Connection.Instance.Send(JsonUtility.ToJson(comm));
    }
}