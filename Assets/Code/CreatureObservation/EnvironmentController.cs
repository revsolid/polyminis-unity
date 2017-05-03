using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {

    public StaticObject ObjectPrototype;
	public ShaderHarness WaterShaderHarness;
	public MistSpawner MistController;
	
	List<StaticObject> SpawnedObjects = new List<StaticObject>();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetEnvironment(SimulationEnvironment env)
	{
		Debug.Log(env.WorldObjects);
		
		while(SpawnedObjects.Count > 0)
		{
			StaticObject toDestroy = SpawnedObjects[0];
			SpawnedObjects.RemoveAt(0);
			Destroy(toDestroy.gameObject);
		}
		
		SpawnedObjects = new List<StaticObject>();
		
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
		
		intensities = new float[(int)env.PhWorld.Dimensions.x, (int)env.PhWorld.Dimensions.y];
		for(int i = 0; i < env.PhWorld.Dimensions.x; i++)
		{
			for(int j = 0; j < env.PhWorld.Dimensions.y; j++)
			{
				intensities[i,j] = env.PhWorld.Grid[i * (int)env.PhWorld.Dimensions.y + j];
			}
		}
		
		MistController.SetPhData(intensities);
	}
	
	void SpawnObject(WorldObjectModel model)
	{
        StaticObject obj = Instantiate<StaticObject>(ObjectPrototype);
		Vector3 pos = CreatureMover.SimulationPositionToScenePosition(model.Position);
		Vector3 dims = CreatureMover.SimulationScaleToSceneScale(model.Dimensions);

        obj.gameObject.transform.position = pos;
		SpawnedObjects.Add(obj);
		
		int DENSITY = 3;
		for(int i = 0; i < DENSITY; i++)
		{
			Vector3 deltaPos = new Vector3(Random.Range(0.0f, dims.x), 0, Random.Range(0.0f, dims.z));
            obj = Instantiate<StaticObject>(ObjectPrototype);
			SpawnedObjects.Add(obj);
			
			if (pos.x < 0)
			{
		//		deltaPos.x -= 2.5f;
			}
			else
			{
	//			deltaPos.x += 2.5f;
			}
			if (pos.y < 0)
			{
		//		deltaPos.z -= 2.5f;
			}
			else
			{
			//	deltaPos.z += 2.5f;
			}

        	obj.gameObject.transform.position = pos + deltaPos;
		}
	}
}