using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OrbitalApproachRenderer : UIPlanetRenderer
{
    Camera TargetCamera;
	public bool Visible = false;
	public float DistanceToSpaceship = 0.0f;

    // Use this for initialization
    void Start ()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        gameObject.SetActive(Visible);
    }
    
    public override void RenderUpdate(Planet model)
    {
		base.RenderUpdate (model);
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
