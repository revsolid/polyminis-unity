using UnityEngine;
using System.Collections;

public class StarmapRenderer : MonoBehaviour, IPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
    private Vector2 SpacePos;
    Camera TargetCamera;
    // Use this for initialization
    void Start () 
    {
    
    }
    
    public void RenderUpdate(Planet model)
    {
        SpacePos = model.SpacePosition;
        if (Starmap != null)
        {
            gameObject.transform.position = Starmap.transform.position + UIManager.ToStarmapPos (model.SpacePosition);
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

    public void SetTargetCamera(Camera camera)
    {
        TargetCamera = camera;
    }



    void OnMouseDown()
    {
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.WARP, SpacePos);
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
        Debug.Log("Clicked!");
    }

}
