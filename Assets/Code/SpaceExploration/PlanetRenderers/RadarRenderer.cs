using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RadarRenderer : UIPlanetRenderer
{
    bool Visible = false;
	float StartingX;
    float StartingY;
    public float DistanceToSpaceship = 0.0f;
    public Text DistanceText;
	public GameObject PlanetContainer;

    // Use this for initialization
    void Start ()
    {
		StartingY = PlanetContainer.transform.position.y;
		StartingX = PlanetContainer.transform.position.x;
        gameObject.SetActive(false);
    }

	void Update(){}
    
    public override void RenderUpdate(Planet model)
    {
        DistanceToSpaceship = model.DistanceToSpaceship;
        if (Mathf.Abs(model.RelativeAngle) < 60 && DistanceToSpaceship > 30 && DistanceToSpaceship < 300)
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
        //+ Mathf.Abs(model.RelativeAngle) / 100.0f
		print ("BBBBBB " + model.RelativeAngle * 100.0f);
		PlanetContainer.transform.position = new Vector3(StartingX + model.RelativeAngle * 3.0f, StartingY, gameObject.transform.position.z);

		DistanceText.text = Math.Round(DistanceToSpaceship).ToString();
		base.RenderUpdate (model);
    }
}
