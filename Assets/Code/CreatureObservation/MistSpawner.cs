using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistSpawner : MonoBehaviour
{
    public float fieldLength; // x
    public float fieldHeight; // z
    public int gridLength; // x
    public int gridHeight; // z

    public Color AcidicColor;
    public Color NeutralColor;
    public Color AlkalineColor;
    public Texture2D intensityMap;
    public List<Vector3> Emmitters;

    private float cellSizeX;
    private float cellSizeZ;

    public GameObject particlePrefab;
    Dictionary<Vector2, ParticleSystem> particles;

    float[,] FillPhValues()
    {
        float[,] phValues = new float[gridLength, gridHeight];

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                phValues[i, j] = 0.1f;
            }
        }

        foreach (Vector3 TempEmmitter in Emmitters)
        {
            Vector2 emmitter_pos = new Vector2(TempEmmitter.x, TempEmmitter.y);
            emmitter_pos *= 10;
            float power = TempEmmitter.z;
            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    float diff_x = Mathf.Abs((float)i - emmitter_pos.x) / 10.0f;
                    float diff_y = Mathf.Abs((float)j - emmitter_pos.y) / 10.0f;

                    phValues[i, j] += power / Math.Max(1, Math.Max(diff_x, diff_y));
                }
            }
        }
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                phValues[i, j] += 0.5f;
                phValues[i, j] = Mathf.Clamp(phValues[i, j], 0.0f, 1.0f);
            }
        }
        return phValues;
    }

    Texture2D PrepareTexture(float[,] intensities)
    {
        Texture2D to_ret = new Texture2D(gridLength, gridHeight);
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                to_ret.SetPixel(i, j, new Color(intensities[i, j], 0.0f, 0.0f, 1.0f));
            }
        }
        to_ret.Apply();
        return to_ret;
    }

    // takes in intensityMap
    Texture2D PrepareTexture()
    {
        Texture2D to_ret = new Texture2D(gridLength, gridHeight);
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                // doesn't read alpha channel of the noise map
                float readIntensity = (intensityMap.GetPixel(i, j).r + 
                    intensityMap.GetPixel(i, j).g +
                    intensityMap.GetPixel(i, j).b) / 3;
                to_ret.SetPixel(i, j, IntensityToColor(readIntensity));
            }
        }
        to_ret.Apply();
        return to_ret;
    }


    void SpawnGrid()
    {
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject particle = Instantiate<GameObject>(particlePrefab, new Vector3(this.transform.position.x + x * cellSizeX,
                                                                                        this.transform.position.y,
                                                                                        this.transform.position.z + z * cellSizeZ), Quaternion.identity);
                particle.transform.parent = this.transform;
                particles.Add(new Vector2(x, z), particle.GetComponent<ParticleSystem>());
            }
        }
    }

    Color IntensityToColor(float intensityIn)
    {
        intensityIn = Mathf.Clamp(intensityIn, 0.0f, 1.0f);

        if(intensityIn < 0.5f)
        {
            return Color.Lerp(AcidicColor, NeutralColor, intensityIn * 2);
        }
        else
        {
            return Color.Lerp(NeutralColor, AlkalineColor, (intensityIn - 0.5f) * 2);
        }
    }

    void ConfigureMistColor()
    {
        Texture2D phs;
        if (intensityMap)
            phs = PrepareTexture();
        else
            phs = PrepareTexture(FillPhValues());

        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                ParticleSystem.MainModule mainMod = particles[new Vector2(x, z)].main;
                Color toSet = new Color(phs.GetPixel(x, z).r,
                    phs.GetPixel(x, z).g,
                    phs.GetPixel(x, z).b,
                    phs.GetPixel(x, z).a / 20.0f);
                mainMod.startColor = toSet;

                int k = 0;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        particles = new Dictionary<Vector2, ParticleSystem>();
        cellSizeX = fieldLength / gridLength;
        cellSizeZ = fieldHeight / gridHeight;
        SpawnGrid();
        ConfigureMistColor();

    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
