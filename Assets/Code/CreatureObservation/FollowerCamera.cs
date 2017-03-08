
using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    public Transform Target;
    public int DistanceMultiplier = 25;
    public int Offset = 25;
    
    void Start()
    {
        gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }
        transform.position = Target.position + Vector3.up;
        transform.LookAt(Target.position);
        transform.position -= (transform.up * Offset);
        transform.position -= (transform.forward * DistanceMultiplier);
        transform.LookAt(Target.position);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}