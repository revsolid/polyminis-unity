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
    bool Visible = false;
    public float DistanceToSpaceship = 0.0f;

    Planet Model;

    // Use this for initialization
    void Start ()
    { 
        gameObject.SetActive(Visible);
    }
    
    public void RenderUpdate(Planet model)
    {
        if (Model != model)
        {
          // We got a new model, either we're being initialized or we implemented pooling
          Model = model;
          transform.localPosition = model.SpacePosition.FromSpaceCoordsToVec3();
        }

        DistanceToSpaceship = model.DistanceToSpaceship;
        Vector2 delta = model.LastDelta;
        transform.localPosition += delta.FromSpaceCoordsToVec3();
     
        if (DistanceToSpaceship > 200 || DistanceToSpaceship < 30)
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
        transform.localScale = (Vector3.one * (600 / (0.05f*DistanceToSpaceship*DistanceToSpaceship)));
    }
}
