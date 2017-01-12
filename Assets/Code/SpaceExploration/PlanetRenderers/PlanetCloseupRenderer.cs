using UnityEngine;
using System.Collections;

public class PlanetCloseupRenderer : SpaceExPlanetRenderer, IPlanetRenderer
{
    public float rotationSpeed; //Degrees per frame
    // Use this for initialization
    void Start ()
    { 
    }
    
    public override void RenderUpdate(Planet model)
    {
        if (Model != model)
        {
            // We got a new model, either we're being initialized or we implemented pooling
            Model = model;
            Texture2D EnvTexture = PrepareTexture(model);
            GetComponent<Renderer>().material.SetTexture("_EnvTexture", EnvTexture);
        }
    }
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }
}
