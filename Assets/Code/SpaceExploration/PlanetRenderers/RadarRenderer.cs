using UnityEngine;
using System.Collections;

public class RadarRenderer : MonoBehaviour, IPlanetRenderer
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
        if (Mathf.Abs(model.RelativeAngle) < 25)
        {
            if (!Visible)
            {
                Visible = true; 
                gameObject.SetActive(true);
            }
        }
        else
        {
            Visible = false;
            gameObject.SetActive(false);
            return;
        }
        
        Debug.Log(model.RelativeAngle);
        Debug.Log(model.Azimuth);
        gameObject.transform.position = new Vector3(model.RelativeAngle, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
