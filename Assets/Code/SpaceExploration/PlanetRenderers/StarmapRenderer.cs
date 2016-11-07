using UnityEngine;
using System.Collections;

public class StarmapRenderer : MonoBehaviour, IPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
	// Use this for initialization
	void Start () 
    {
	
	}
	
    public void RenderUpdate(Planet model)
    {
        if (Starmap != null)
        {
            gameObject.transform.position = Starmap.transform.position + ToStarmapPos (model.SpacePosition);
            if (Starmap.activeSelf)
            {
                this.gameObject.SetActive (true);
            } 
            else
            {
                this.gameObject.SetActive (false);
            }
        }

    }

    Vector3 ToStarmapPos(Vector2 spacePos)
    {
        float shrinkFactor = 200.0f;

        return (new Vector3(spacePos.x / shrinkFactor, spacePos.y / shrinkFactor, 0.0f));
    }

}
