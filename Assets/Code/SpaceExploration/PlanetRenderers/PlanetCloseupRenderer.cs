using UnityEngine;
using System.Collections;

public class PlanetCloseupRenderer : MonoBehaviour, IPlanetRenderer
{
    bool Visible = true;
    public float DistanceToSpaceship = 0.0f;
    public float rotationSpeed; //Degrees per frame
    // Use this for initialization
    void Start ()
    { 
    }
    
    public void RenderUpdate(Planet model)
    {
        DistanceToSpaceship = model.DistanceToSpaceship;
    }
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }
}
