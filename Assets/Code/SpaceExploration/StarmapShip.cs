using UnityEngine;
using System.Collections;

public class StarmapShip : MonoBehaviour
{
    public SpaceMovementTracker SpaceMovementTracker;
    // Update is called once per frame
    void Update ()
    {
        this.transform.position = this.transform.parent.position + UIManager.ToStarmapPos(spaceMovementTracker.CurrentPosition) -Vector3.forward * (+1.0f);
        this.transform.forward = new Vector3(-spaceMovementTracker.Forward.x, spaceMovementTracker.Forward.y, 0);
    }
}
