using UnityEngine;
using System.Collections;

public class PlanetLiquid : MonoBehaviour
{
    public delegate void OnClickDelegate(Vector3 point);
    public event OnClickDelegate OnClick;
    
    public Camera FloatingCamera;
    public Camera TopDownCamera;
    public Collider MyCollider;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var xx = Input.mousePosition;
            Camera current = FloatingCamera.enabled ? FloatingCamera : TopDownCamera;
            Ray ray = current.ScreenPointToRay(xx);
            var allHits = Physics.RaycastAll(ray);
            if (allHits.Length == 1)
            {
                OnClick(allHits[0].point); 
            }
        }
    }
}