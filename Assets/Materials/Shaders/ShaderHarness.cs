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

    public Color HighColor;
    public Color TempColor;
    public Color LowColor;

    //TODO: Temp Implementation - x,y = pos, z = value
    public List<Vector3> Emmitters;
    
    public Texture2D TempTexture;

	void Start ()
    {
        UpdateTexture();
        UpdateTiling();
    }
    
    void UpdateTexture()
	{
        float[,] intensities = FillIntensities();
        UpdateTexture(intensities);
    }
    
    public void UpdateTexture(float[,] intensities)
    {
        XPoints = intensities.GetLength(0);
        YPoints = intensities.GetLength(1);
        Debug.Log("X " + XPoints);
        Debug.Log("Y " + YPoints);
        TempTexture = PrepareTexture(intensities);
        material.SetTexture("_TemperatureTexture", TempTexture);
    }

    void UpdateTiling()
    {
        // these are for if the geometry is created from a standard plane in unity.
        material.SetFloat("_NormalTiling", 3000.0f / this.gameObject.transform.localScale.x );
        material.SetFloat("_FoamTiling", 3000f / this.gameObject.transform.localScale.x);
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
        float int_value;
        for (int i = 0; i < XPoints; i++)
        {
            for(int j = 0; j < YPoints; j++)
            {
                int_value = intensities[i, j];

                if(int_value < 0.5f)
                {
                    int_value *= 2;
                    to_ret.SetPixel(i, j, Color.Lerp(LowColor, TempColor, int_value));
                }
                else if (int_value >= 0.5f)
                {
                    int_value = (int_value - 0.5f) * 2;
                    to_ret.SetPixel(i, j, Color.Lerp(TempColor, HighColor, int_value));
                }
            }
        }
        to_ret.Apply();
        return to_ret;
    }
}