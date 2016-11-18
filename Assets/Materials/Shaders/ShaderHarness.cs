using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;




public class ShaderHarness: MonoBehaviour
{
    // Idea use this numbers to grid the world
    public int XPoints = 10;
    public int YPoints = 10;
    public Material material;
    //TODO: Temp Implementation - x,y = pos, z = value
    public List<Vector3> Emmitters;
	void Start ()
	{
		material.SetInt("_X_Points", XPoints);
		material.SetInt("_Y_Points", YPoints);
        float[] intensities = FillIntensities();
        material.SetFloatArray("_Values", intensities);
    }
    
    int from_xy(int x, int y)
    {
        return XPoints*y + x;
    }
    float[] FillIntensities()
    {
        float[] intensities =  new float[XPoints * YPoints];
        // Zero it
        int p_inx = 0;
        for(int i = 0; i < XPoints * YPoints; i++)
        {
            intensities[i] = 0.1f;
        }

        foreach(Vector3 TempEmmitter in Emmitters)
        {
            Vector2 emmitter_pos = new Vector2(TempEmmitter.x, TempEmmitter.y);
            float power = TempEmmitter.z;
            for (int i = 0; i < XPoints; i++)
            {
                for(int j = 0; j < YPoints; j++)
                {
                    int diff_x = (int) Mathf.Abs((float) i - emmitter_pos.x);
                    int diff_y = (int) Mathf.Abs((float) j - emmitter_pos.y);

                    intensities[from_xy(i, j)] +=  power / Math.Max(1, Math.Max(diff_x, diff_y));
                }
            }
        }
        for(int i = 0; i < XPoints * YPoints; i++)
        {
            intensities[i] += 0.5f;
        }
        return intensities;
	}
}