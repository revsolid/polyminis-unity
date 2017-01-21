using UnityEngine;
using System.Collections;

public class StarmapRenderer : MonoBehaviour, IPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
    private Vector2 SpacePos;
    Camera TargetCamera;
    public GameObject WarpDialog;
    // Use this for initialization

    public void RenderUpdate(Planet model)
    {
        SpacePos = model.SpacePosition;
        UpdatePosition(SpacePos);

        if (Starmap != null)
        {
            UpdatePosition(model.SpacePosition);

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
        GameObject wp = Instantiate(WarpDialog);
        wp.GetComponent<WarpDialog>().SpacePos = SpacePos;
    }

    // update relative to starmap object
    void UpdatePosition(Vector2 newSpacePos)
    {
        this.transform.localPosition = UIManager.ToStarmapPos(newSpacePos);
    }
}
