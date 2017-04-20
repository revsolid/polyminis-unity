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
		//	if (!wom.IsBorder)
				SpawnObject(wom);
		}
		
		float[,] intensities = new float[(int)env.ThermalWorld.Dimensions.x, (int)env.ThermalWorld.Dimensions.y];
		for(int i = 0; i < env.ThermalWorld.Dimensions.x; i++)
		{
			for(int j = 0; j < env.ThermalWorld.Dimensions.y; j++)
			{
				intensities[i,j] = env.ThermalWorld.Grid[i * (int)env.ThermalWorld.Dimensions.y + j] - 0.5f;
			}
		}
		
		WaterShaderHarness.UpdateTexture(intensities);
	}
	
	void SpawnObject(WorldObjectModel model)
	{
        StaticObject obj = Instantiate<StaticObject>(ObjectPrototype);
		Vector3 pos = CreatureMover.SimulationPositionToScenePosition(model.Position);
		Vector3 dims = CreatureMover.SimulationScaleToSceneScale(model.Dimensions);

        obj.gameObject.transform.position = pos;
		
		int DENSITY = 10;
		for(int i = 0; i < DENSITY; i++)
		{
			Vector3 deltaPos = new Vector3(Random.Range(0.0f, dims.x), 0, Random.Range(0.0f, dims.z));
            obj = Instantiate<StaticObject>(ObjectPrototype);
			
			if (pos.x < 0)
			{
				deltaPos.x -= 2.5f;
			}
			else
			{
				deltaPos.x += 2.5f;
			}
			if (pos.y < 0)
			{
				deltaPos.y -= 2.5f;
			}
			else
			{
				deltaPos.y += 2.5f;
			}

        	obj.gameObject.transform.position = pos + deltaPos;
		}
	}
}