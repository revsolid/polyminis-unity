using UnityEngine;
using System.Collections;

public class CreatureMover : MonoBehaviour
{

	public Transform CurrentTarget;
	public float Speed;
	
	Quaternion TargetRotation = RIGHT_HEADING;
	
	enum Headings
	{
		Up,
		Down,
		Left,
		Right,
	}
	
	static Quaternion UP_HEADING = Quaternion.Euler(0, 0, 0);
	static Quaternion DOWN_HEADING = Quaternion.Euler(0, 180, 0);
	static Quaternion LEFT_HEADING = Quaternion.Euler(0, -90, 0);
	static Quaternion RIGHT_HEADING = Quaternion.Euler(0, 90, 0);

	void Start ()
	{ 
	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: Either by animation or some interesting math make it look like "impulse and decay" instead
		//      of just linear advancing
		Vector3 azimuth = gameObject.transform.position - CurrentTarget.position; 
		azimuth.Normalize();
		gameObject.transform.Translate(-1 * azimuth * Time.deltaTime * Speed, Space.World);
		
		gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, TargetRotation, Time.time * Speed);
	}
}