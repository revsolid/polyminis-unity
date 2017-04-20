using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {

    public StaticObject ObjectPrototype;
	public ShaderHarness WaterShaderHarness;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetEnvironment(SimulationEnvironment env)
	{
		Debug.Log(env.WorldObjects);
		
		foreach(WorldObjectModel wom in env.WorldObjects)
		{
			if (!wom.IsBorder)
				SpawnObject(wom);
		}
		
		float[,] intensities = new float[(int)env.ThermalWorld.Dimensions.x, (int)env.ThermalWorld.Dimensions.y];
		for(int i = 0; i < env.ThermalWorld.Dimensions.x; i++)
		{
			for(int j = 0; j < env.ThermalWorld.Dimensions.y; j++)
			{
				intensities[i,j] = env.ThermalWorld.Grid[i * (int)env.ThermalWorld.Dimensions.y + j];
			}
		}
		
		WaterShaderHarness.UpdateTexture(intensities);
	}
	
	void SpawnObject(WorldObjectModel model)
	{
        StaticObject obj = Instantiate<StaticObject>(ObjectPrototype);
        obj.gameObject.transform.position = CreatureMover.SimulationPositionToScenePosition(model.Position);
        obj.gameObject.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
        obj.gameObject.transform.localScale = CreatureMover.SimulationScaleToSceneScale(model.Dimensions);
	}
}