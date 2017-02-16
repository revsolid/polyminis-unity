using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OrbitalApproachRenderer :  SpaceExPlanetRenderer
{
    Camera TargetCamera;
    Planet Model = null;
    // Use this for initialization
    void Start ()
    {
        //Canvas canvas = gameObject.GetComponent(typeof(Canvas)) as Canvas;
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        gameObject.SetActive(Visible);
    }
    
    public override void RenderUpdate(Planet model)
    {
        if(Model == null)
        {
            Model = model;
            Texture2D EnvTexture = PrepareTexture(model);
            GetComponentInChildren<Renderer>().material.SetTexture("_EnvTexture", EnvTexture);
        }
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
        Camera.main.gameObject.SetActive(false);
        TargetCamera.gameObject.SetActive(true);
        TargetCamera.enabled = true;
        TargetCamera.GetComponentInChildren<OrbitalUI>().OnUIOpened(Model);
        Debug.Log("Orbiting: "+Model.PlanetName);
	}
}
