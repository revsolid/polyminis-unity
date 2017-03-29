using UnityEngine;
using System.Collections;

public class PlanetLiquid : MonoBehaviour
{
    public delegate void OnClickDelegate(Vector3 point);
    public event OnClickDelegate OnClick;
    
    public Camera FloatingCamera;
    public Camera TopDownCamera;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var xx = Input.mousePosition;
            Camera current = FloatingCamera.enabled ? FloatingCamera : TopDownCamera;
            Ray ray = current.ScreenPointToRay(xx);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                OnClick(hit.point); 
            }
        }
    }
}