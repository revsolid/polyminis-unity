using UnityEngine;
using System.Collections;

public class SpaceExPlanetRenderer : MonoBehaviour, IPlanetRenderer
{
    bool Visible = true;
    public float DistanceToSpaceship = 0.0f;

    // Use this for initialization
    void Start ()
    { 
    }
    
    public void RenderUpdate(Planet model)
    {
        DistanceToSpaceship = model.DistanceToSpaceship;
        Vector2 delta = model.LastDelta;
        transform.localPosition += new Vector3(delta.y, 0.0f, -1*delta.x);
     
        if (DistanceToSpaceship > 200 || DistanceToSpaceship < 30)
        {
          gameObject.SetActive(false);  
          Visible = false;
          return;
        }
        else if (!Visible)
        {          
          gameObject.SetActive(true);
          Visible = true;
        }
        
        DistanceToSpaceship = Mathf.Max(0.0000001f, DistanceToSpaceship);
        transform.localScale = (Vector3.one * (600 / (0.05f*DistanceToSpaceship*DistanceToSpaceship)));
    }
}
