using UnityEngine;
using System.Collections;

public class StarmapRenderer : MonoBehaviour, IPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
    public WarpDialog WarpDialog;
    public GameObject BlockingDialog;
    private Vector2 SpacePos;
    private Camera TargetCamera;
    private float DistanceToSpaceship;

    public void RenderUpdate(Planet model)
    {
        SpacePos = model.SpacePosition;
        DistanceToSpaceship = model.DistanceToSpaceship;
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

    private void Update()
    {
        if (BlockingDialog != null && this.GetComponent<SphereCollider>().enabled)
        {
            UpdateCollider(false);
        }
        else if(BlockingDialog == null && !this.GetComponent<SphereCollider>().enabled)
        {
            UpdateCollider(true);
        }
    }

    private void OnEnable()
    {
        // to prevent that there already is a dialog in the scene
        PolyminisDialog dialog = FindObjectOfType<PolyminisDialog>();
        if (dialog)
        {
            UpdateCollider(false);
        }
    }
    
    public  void UpdateCollider(bool enable)
    {
        this.GetComponent<SphereCollider>().enabled = enable;
    }

    public void SetTargetCamera(Camera camera)
    {
        TargetCamera = camera;
    }



    void OnMouseDown()
    {
        WarpDialog wp = Instantiate(WarpDialog);
        wp.SetWarpParams(SpacePos);
    }

    // update relative to starmap object
    void UpdatePosition(Vector2 newSpacePos)
    {
        this.transform.localPosition = UIManager.ToStarmapPos(newSpacePos);
    }
}
