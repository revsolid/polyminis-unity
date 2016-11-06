using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadarRenderer : MonoBehaviour, IPlanetRenderer
{
    bool Visible = false;
    public float DistanceToSpaceship = 0.0f;
    public Text DistanceText;

    // Use this for initialization
    void Start ()
    {
        gameObject.SetActive(false);
    }
    
    public void RenderUpdate(Planet model)
    {
        DistanceToSpaceship = model.DistanceToSpaceship;
        if (Mathf.Abs(model.RelativeAngle) < 25 && DistanceToSpaceship > 30 && DistanceToSpaceship < 300)
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
        
        gameObject.transform.position = new Vector3(model.RelativeAngle / 5.0f, gameObject.transform.position.y, gameObject.transform.position.z);
        DistanceText.text = DistanceToSpaceship.ToString();
    }
}
