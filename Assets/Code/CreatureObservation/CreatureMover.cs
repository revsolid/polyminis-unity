using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//TMP
enum MovementType
{
    ROTATE,
    TRANSLATE
}

class CreatureMovementAction
{
    public MovementType MovType;
    public Transform Target;
    public Quaternion TargetRotation;
	public float TimeStarted;
	
	public CreatureMovementAction(MovementType mType, Transform target, Quaternion targetRotation)
	{
		MovType = mType;
		Target = target;
		TargetRotation = targetRotation;
		if (!Validate())	
		{
			// ERROR
		}
	}
	
	public bool Validate()
	{
		switch (MovType)
		{
            case MovementType.TRANSLATE:
				return Target != null;
            case MovementType.ROTATE:
				return TargetRotation != Quaternion.identity;
		}
		return false;
	}
	
	public void Start()
	{
		TimeStarted = Time.time;
	}
}


public class CreatureMover : MonoBehaviour
{

    public float Speed;
    public Transform CurrentTarget;
    Quaternion TargetRotation = RIGHT_HEADING;
    
    CreatureMovementAction CurrentAction;
    Queue<CreatureMovementAction> ActionStream;
    
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
        ActionStream = new Queue<CreatureMovementAction>();
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, null, RIGHT_HEADING));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, null, UP_HEADING));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, null, RIGHT_HEADING));
    }
    
    // Update is called once per frame
    void Update ()
    {
		
        if (CurrentAction == null)
        {
			if (ActionStream.Count > 0)
			{
				CurrentAction = ActionStream.Dequeue();
				CurrentAction.Start();
			}
			else
			{
            	return;
			}
        }
        
        switch (CurrentAction.MovType)
        {
            case MovementType.TRANSLATE:
                //TODO: Either by animation or some interesting math make it look like "impulse and decay" instead
                //      of just linear advancing
                Vector3 azimuth = gameObject.transform.position - CurrentAction.Target.position; 
				Debug.Log(azimuth.sqrMagnitude);
				if (azimuth.sqrMagnitude < 0.001)
				{
					gameObject.transform.position = CurrentAction.Target.position;
					CurrentAction = null;
				}
				else
				{
                	azimuth.Normalize();
                	gameObject.transform.Translate(-1 * azimuth * Time.deltaTime * Speed, Space.World);
				}
                break;
                
            case MovementType.ROTATE:
				if (Quaternion.Angle(gameObject.transform.localRotation, CurrentAction.TargetRotation) < 0.01)
				{
					CurrentAction = null;
				}
				else
				{
					float t = Time.time - CurrentAction.TimeStarted;
    	            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, CurrentAction.TargetRotation, t * Speed / 8);
				}
                break;
        }
    }
}