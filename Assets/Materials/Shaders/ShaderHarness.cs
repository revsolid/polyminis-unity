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
    
    public Texture2D TempTexture;

	void Start ()
    {
        UpdateTexture();
    }
    
    void UpdateTexture()
	{
        float[,] intensities = FillIntensities();
        //material.SetFloatArray("_Values", intensities);
        TempTexture = PrepareTexture(intensities);
        Debug.Log("Setting");
        material.SetTexture("_TempTexture", TempTexture);
    }
    
    float[,] FillIntensities()
    {
        float[,] intensities =  new float[XPoints,YPoints];

        for(int i = 0; i < XPoints; i++)
        {
            for(int j = 0; j < YPoints; j++)
            {
                intensities[i,j] = 0.1f;
            }
        }

        foreach(Vector3 TempEmmitter in Emmitters)
        {
            Vector2 emmitter_pos = new Vector2(TempEmmitter.x, TempEmmitter.y);
            emmitter_pos *= 10;
            float power = TempEmmitter.z;
            for (int i = 0; i < XPoints; i++)
            {
                for(int j = 0; j < YPoints; j++)
                {
                    float diff_x = Mathf.Abs((float) i - emmitter_pos.x) / 10.0f;
                    float diff_y = Mathf.Abs((float) j - emmitter_pos.y) / 10.0f;

                    intensities[i,j] +=  power / Math.Max(1, Math.Max(diff_x, diff_y));
                }
            }
        }
        for(int i = 0; i < XPoints; i++)
        {
            for(int j = 0; j < YPoints; j++)
            {
                intensities[i,j] += 0.5f;
                intensities[i, j] = Mathf.Clamp(intensities[i,j], 0.0f, 1.0f);
            }
        }
        return intensities;
	}
    
    Texture2D PrepareTexture(float[,] intensities)
    {
        Texture2D to_ret = new Texture2D(XPoints, YPoints);
        for(int i = 0; i < XPoints; i++)
        {
            for(int j = 0; j < YPoints; j++)
            {
                to_ret.SetPixel(i, j, new Color(intensities[i,j], 0.0f, 0.0f, 1.0f));
            }
        }
        to_ret.Apply();
        return to_ret;
    }
}