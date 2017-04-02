using UnityEngine;
using System.Collections;


//
static class SpaceCoordsExtension
{
    public static Vector3 FromSpaceCoordsToVec3(this Vector2 v)
    {
        return new Vector3(v.y, 0, v.x);
    }
}

public class SpaceExPlanetRenderer : MonoBehaviour, IPlanetRenderer
{
    public float DistanceToSpaceship = 0.0f;
    public bool Visible = false;
    
    protected Planet Model;

    // Use this for initialization
    void Start ()
    { 
        gameObject.SetActive(Visible);
        
    }
    
    void Update()
    {
       transform.Rotate(0, 1, 0);
    }
    
    public virtual void RenderUpdate(Planet model)
    {
        if (Model != model)
        {
            // We got a new model, either we're being initialized or we implemented pooling
            Model = model;
            transform.localPosition = model.SpacePosition.FromSpaceCoordsToVec3();
          

            // Prepare texture
            Texture2D EnvTexture = PrepareTexture(model);
            GetComponent<Renderer>().material.SetTexture("_EnvTexture", EnvTexture);
        }

        DistanceToSpaceship = model.DistanceToSpaceship;
        Vector2 delta = model.LastDelta;
        
        transform.localPosition += delta.FromSpaceCoordsToVec3();
        
        if (DistanceToSpaceship < 50)
        {
            transform.localPosition =  new Vector3(transform.localPosition.x, -12.5f + DistanceToSpaceship / 2.0f , transform.localPosition.z);
        }
        else
        {
            transform.localPosition =  new Vector3(transform.localPosition.x, 0.0f, transform.localPosition.z);
        }
     
        if (DistanceToSpaceship > 200)
        {
          Visible = false;
          gameObject.SetActive(Visible);  
          return;
        }
        else if (!Visible)
        {          
          Visible = true;
          gameObject.SetActive(Visible);
        }

        DistanceToSpaceship = Mathf.Max(0.0000001f, DistanceToSpaceship);
        
        transform.localScale = (Vector3.one * Mathf.Min(600 / (0.05f*DistanceToSpaceship*DistanceToSpaceship), 30));
    }
    
    protected Texture2D PrepareTexture(Planet model)
    {
        int Points = 5;
        Color[,] pixels = new Color[Points, Points];
        Texture2D to_ret = new Texture2D(Points, Points);
        
        for(int i = 0; i < Points; i++)
        {
            for(int j = 0; j < Points; j++)
            {
                pixels[j,i] = new Color(model.Temperature.Average(), 0.0f, 0.0f, 1.0f);
            }
        }
            

        for(int i = 0; i < Points; i++)
        {
            // Set top and bottom most with the coldest range
            pixels[0,i] =  new Color(model.Temperature.Min, 0.0f, 0.0f, 1.0f);
            pixels[Points - 1, i] =  new Color(model.Temperature.Min, 0.0f, 0.0f, 1.0f);
            
            // set the mid rows with the hottest
            int midp =  Points / 2;
            pixels[midp, i] =  new Color(model.Temperature.Max, 0.0f, 0.0f, 1.0f);
        }
        Color[] pixels_1_d = new Color[Points * Points];
        for(int i = 0; i < Points; i++)
        {
            for(int j = 0; j < Points; j++)
            {
                pixels_1_d[i*Points+j] = pixels[i,j];
            }
        }

        to_ret.SetPixels(pixels_1_d);
        to_ret.Apply();
        return to_ret;
    }
            
}
