using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera FloatingCamera;
    public float RotationRadius = 500;
    
    public Camera TopDownCamera;
    public Transform TargetPoint;
    
    public FollowerCamera DetailViewCamera;
    public NeuralNetworkUI NeuralNetworkUI;
    
    public PlanetLiquid Liquid;
    
    public Slider ZoomSlider;
    
    public Canvas CameraSwitcher;

    float Angle = 0.0f;
    float HorImpulse;
    float VerImpulse;
    
    
    public void UseTopDownCamera()
    {
        FloatingCamera.enabled = false;
        TopDownCamera.enabled = true;
    }
    public void UseFloatingCamera()
    {
        FloatingCamera.enabled = true;
        TopDownCamera.enabled = false;
    }
    
    public void SetFocusOnCreature(Creature creature)
    {
        DetailViewCamera.gameObject.SetActive(true);
        //FloatingCamera.enabled = false;
        //TopDownCamera.enabled = false;
        DetailViewCamera.Target = creature.transform;
        NeuralNetworkUI.SetCreature(creature);
        CameraSwitcher.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start ()
	{
        FloatingCamera.transform.position = new Vector3(RotationRadius, 100, 0.0f);
        TopDownCamera.enabled = false;
        Liquid.TopDownCamera = TopDownCamera;
        Liquid.FloatingCamera = FloatingCamera;
        Liquid.OnClick += LookAtPoint;
	}
	
	// Update is called once per frame
	void Update ()
	{
        HorImpulse = ControlHelper.GetHorizontalAxis();
        VerImpulse = ControlHelper.GetVerticalAxis();
	}

    void LateUpdate()
    {
       if (FloatingCamera == Camera.current) 
       {
           CameraSwitcher.gameObject.SetActive(true);
           LateUpdateFloating(); 
       }
       
       if (TopDownCamera == Camera.current)
       {
           CameraSwitcher.gameObject.SetActive(true);
           LateUpdateTopDown(); 
       }
       
       
    }
    
    void LateUpdateFloating()
    {
        Vector3 newPos = FloatingCamera.transform.position;
        newPos.y = Mathf.Min(125, Mathf.Max(newPos.y + VerImpulse, 5.0f));
        if (HorImpulse != 0)
        {
            Angle = (Angle + ( HorImpulse / 60 ) ) % 360;
            float rad = Angle * Mathf.PI;
            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            newPos.x = x * RotationRadius;
            newPos.z = z * RotationRadius;
        }
        FloatingCamera.transform.position = newPos;
        FloatingCamera.transform.LookAt(Vector3.zero);
        //ToControl.fieldOfView = ZoomSlider.value;       
    }
    
    void LateUpdateTopDown()
    {
        Vector3 newPos = TargetPoint.position;
        if (HorImpulse != 0)
        {
            newPos.x = Mathf.Min(100, Mathf.Max(newPos.x + HorImpulse, -100.0f));
        }
        if (VerImpulse != 0)
        {
            newPos.z = Mathf.Min(100, Mathf.Max(newPos.z + VerImpulse, -100.0f));
        }
        
        TargetPoint.position = newPos;
        
        TopDownCamera.transform.position = TargetPoint.position + Vector3.up*100; 
        TopDownCamera.transform.LookAt(TargetPoint);
    } 
        
    void LookAtPoint(Vector3 lookAt)
    {
        TargetPoint.position = lookAt;
        TopDownCamera.transform.position = lookAt + Vector3.up*100; 
        FloatingCamera.enabled = false;
        TopDownCamera.enabled = true;
    }
    
    void OnDestroy()
    {
        //
        Liquid.OnClick -= LookAtPoint;
    }
}