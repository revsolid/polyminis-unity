using UnityEngine;
using System.Collections;

public class InOrbitRenderer: SpaceExPlanetRenderer
{
    // Use this for initialization
	float StartingZ;
    bool ReloadRenderer;
    void Start ()
    { 
		StartingZ = gameObject.transform.position.z;
        gameObject.SetActive(false);
    }
    
    void Update()
    {
       transform.Rotate(0, 3, 0);
          
        // Prepare texture
        if (ReloadRenderer && Model != null)
        {
            Texture2D EnvTexture = PrepareTexture(Model);
            GetComponent<Renderer>().material.SetTexture("_EnvTexture", EnvTexture);
            ReloadRenderer = false;
        }
    }
    
	public override void RenderUpdate(Planet model)
    {
        /*
        if (Model != model)
        {
            // We got a new model
            Model = model;
            ReloadRenderer = true;
		}
		
		DistanceToSpaceship = model.DistanceToSpaceship;
		float x = gameObject.transform.position.x;
		float y = gameObject.transform.position.y;
		if (Mathf.Abs(model.RelativeAngle) > 120.0)		
		{
			Visible = false;
            gameObject.SetActive(Visible);  
          	return;
		}
		else if (Mathf.Abs(model.RelativeAngle) > 50.0 || DistanceToSpaceship < 5)		
		{
			return;
		}
		else
		{
        	gameObject.transform.localScale = Vector3.one * (-0.66f * DistanceToSpaceship + 35f);
			y = DistanceToSpaceship/1.5f - 20.0f;
			x = model.RelativeAngle / 2.0f;
		}
		float delta_z = 17.0f - DistanceToSpaceship / 2.0f; 
        gameObject.transform.position = new Vector3(x, y, StartingZ + delta_z);
	    if (DistanceToSpaceship > 30)
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
        */
	

	}
        
}