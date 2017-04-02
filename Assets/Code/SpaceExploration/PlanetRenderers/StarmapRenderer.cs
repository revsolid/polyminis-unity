using UnityEngine;
using System.Collections;

public class StarmapRenderer : UIPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
    public WarpDialog WarpDialog;
    public GameObject BlockingDialog;
    private Vector2 SpacePos;
    private Camera TargetCamera;
    private float DistanceToSpaceship;
    public float ScaleFactor;
    public Sprite PlanetArrow;
    public float DisplayRange , MaxRange;

    public override void RenderUpdate(Planet model)
    {
        SpacePos = model.SpacePosition;
        DistanceToSpaceship = model.DistanceToSpaceship;

        if( DistanceToSpaceship < DisplayRange)
        {

        }
        UpdatePosition(SpacePos);

        if (Starmap != null)
        {
            UpdatePosition(model.SpacePosition);

            if (Starmap.activeSelf)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        base.RenderUpdate(model);
    }

    public void PlanetClick()
    {
        WarpDialog wp = Instantiate(WarpDialog);
        wp.SetWarpParams(SpacePos);
    }
    
    public void SetTargetCamera(Camera camera)
    {
        TargetCamera = camera;
    }

    // update relative to starmap object
    void UpdatePosition(Vector2 newSpacePos)
    {
        this.transform.localPosition = UIManager.ToStarmapPos(newSpacePos);


    }
}
