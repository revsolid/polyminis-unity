using UnityEngine;
using System.Collections;

public class StarmapShip : MonoBehaviour
{
    public SpaceMovementTracker SpaceMovementTracker;
    // Update is called once per frame
    void Update ()
    {
        this.transform.localPosition = UIManager.ToStarmapPos(SpaceMovementTracker.CurrentPosition) - Vector3.forward * (+1.0f);
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, -1*SpaceMovementTracker.Heading + 90);//Quaternion.LookRotation(new Vector3(-SpaceMovementTracker.Forward.x, SpaceMovementTracker.Forward.y, 0), Vector3.up);
    }
}
