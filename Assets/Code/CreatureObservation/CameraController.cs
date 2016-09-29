using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject TargetPoint;
    public float RotationRadius = 500;

    float Angle = 0.0f;
    float HorImpulse;
    float VerImpulse;

	// Use this for initialization
	void Start ()
	{
            transform.position = new Vector3(0.0f, 0.0f, RotationRadius);
	}
	
	// Update is called once per frame
	void Update ()
	{
		HorImpulse = Input.GetAxis ("Horizontal");
		VerImpulse = Input.GetAxis ("Vertical");
	}

    void LateUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.y += VerImpulse;
        if (HorImpulse != 0)
        {
            Angle = (Angle + ( HorImpulse / 60 ) ) % 360;
            float rad = Angle * Mathf.PI;
            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            newPos.x = x * RotationRadius;
            newPos.z = z * RotationRadius;
        }
        transform.position = newPos;
        transform.LookAt(TargetPoint.transform);
    }
}

