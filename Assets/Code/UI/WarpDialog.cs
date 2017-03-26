using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDialog : MonoBehaviour
{
    private Vector2 SpacePos;
    private float WarpCost;

    public void SetWarpParams(Vector2 targetPosition)
    {
        Connection.Instance.OnMessageEvent += OnCostCalculated;
        SpacePos = targetPosition;
        var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.CALC_WARP_COST, SpacePos);
        Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
        
    }
    public void SendWarpMessage()
    {
        if(SpacePos != null)
        {
            var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.ATTEMPT_WARP, SpacePos);
            Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
            Destroy(gameObject);
        }
    }
    
    public void Update()
    {
        if (WarpCost != 0.0f)
        {
            string warpCostString = WarpCost.ToString("0.0");
            PolyminisDialog dialog = GetComponent<PolyminisDialog>();
            
            if (WarpCost > Session.Instance.Biomass) 
            {
                dialog.DialogString = "You don't have enough Biomass to warp to that planet. (" + Session.Instance.Biomass + "/" + warpCostString + ")";
                dialog.LeftButton.gameObject.SetActive(false);
            }
            else if (WarpCost < 0.0f)
            {
                dialog.DialogString = "You cannot Warp that far!";
                dialog.LeftButton.gameObject.SetActive(false);
            }
            else
            {
                dialog.DialogString = "The cost of warping to this planet is " + warpCostString + " Biomass.\n";
                dialog.DialogString += "Do you want to warp?";
                dialog.LeftButton.gameObject.SetActive(true);
            }
        }
    }
    
    public void OnCostCalculated(string message)
    {
        SpaceExplorationEvent spaceExEvent = JsonUtility.FromJson<SpaceExplorationEvent>(message);
        if (spaceExEvent != null)
        {
            switch (spaceExEvent.EventType)
            {
            case SpaceExplorationEventType.WARP_COST_RESULT:
                Connection.Instance.OnMessageEvent -= OnCostCalculated;
                WarpCost = spaceExEvent.WarpCost;
                Debug.Log("RESULT! " + message);
                break;
            }
        }
    }
}
