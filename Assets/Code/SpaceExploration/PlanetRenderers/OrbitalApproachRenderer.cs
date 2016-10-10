using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OrbitalApproachRenderer : MonoBehaviour, IPlanetRenderer
{
    bool Visible = true;
    public float DistanceToSpaceship = 0.0f;
    Camera TargetCamera;

    // Use this for initialization
    void Start ()
    { 
        Canvas canvas = gameObject.GetComponent(typeof(Canvas)) as Canvas;
        canvas.worldCamera = Camera.main;
    }
    
    public void RenderUpdate(Planet model)
    {
        DistanceToSpaceship = model.DistanceToSpaceship;
        if (DistanceToSpaceship < 30)
        {
            if (!Visible)
            {
                Visible = true;
                gameObject.SetActive(true);  
                Canvas canvas = gameObject.GetComponent(typeof(Canvas)) as Canvas;
                canvas.enabled = true;
            }
        }
        else
        {          
            gameObject.SetActive(false);  
            Visible = false;
        }
    }
    
    public void SetTargetCamera(Camera camera)
    {
        TargetCamera = camera;        
    }
    
    void Update()
    {
        
    }
    
	public void OnButtonClick()
	{
        Debug.Log("Clickety Click");
        Camera.main.enabled = false;
        TargetCamera.enabled = true;
	}
}
