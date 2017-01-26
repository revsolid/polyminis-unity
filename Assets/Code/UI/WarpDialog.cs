using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDialog : MonoBehaviour
{
    [HideInInspector]
    public Vector2 SpacePos;

    public void SendWarpMessage()
    {
        if(SpacePos != null)
        {
            var spaceExCommand = new SpaceExplorationCommand(SpaceExplorationCommandType.WARP, SpacePos);
            Connection.Instance.Send(JsonUtility.ToJson(spaceExCommand));
            Destroy(gameObject);
        }
    }
}
