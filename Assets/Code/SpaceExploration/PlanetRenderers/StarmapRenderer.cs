using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarmapRenderer : MonoBehaviour, IPlanetRenderer
{
    [HideInInspector] public GameObject Starmap;
    private Vector2 SpacePos;
    Camera TargetCamera;
    public GameObject WarpPanel;
    float DistanceToSpaceship;
    public float DistanceToBiomassFactor;
    private int WarpCost;
    public Text WarpMessageText;
    public Button WarpButton, CancelButton;


    // Use this for initialization

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

    public void SetTargetCamera(Camera camera)
    {
        TargetCamera = camera;
    }



    void OnMouseDown()
    {
       ConfigureWarpDialouge();

    }


    public void ConfigureWarpDialouge()
    {
        int cost = (int)(DistanceToSpaceship * DistanceToBiomassFactor);
        WarpCost = cost;

        WarpPanel.SetActive(true);
        CancelButton.enabled = true;
        if (BiomassManager.HasBiomass(cost))
        {
            WarpMessageText.text = "Would you like to warp? (Cost: " + cost + ")";
            WarpButton.enabled = true;
        }
        else
        {
            WarpMessageText.text = "You do not have enough biomass to warp. (Need: " + cost +")";
            WarpButton.enabled = false;
        }
    }
    public void DoWarp()
    {
        BiomassManager.ConsumeBiomass(WarpCost);
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.WARP, SpacePos);
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
        WarpPanel.SetActive(false);
    }

    public void CancelWarp()
    {
        WarpPanel.SetActive(false);
    }
    // update relative to starmap object
    void UpdatePosition(Vector2 newSpacePos)
    {
        this.transform.localPosition = UIManager.ToStarmapPos(newSpacePos);
    }
}
