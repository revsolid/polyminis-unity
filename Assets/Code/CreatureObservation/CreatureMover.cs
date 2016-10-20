using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//TMP
class Heading
{
	public static Quaternion UP_HEADING = Quaternion.Euler(0, 0, 0);
	public static Quaternion DOWN_HEADING = Quaternion.Euler(0, 180, 0);
	public static Quaternion LEFT_HEADING = Quaternion.Euler(0, 270, 0);
	public static Quaternion RIGHT_HEADING = Quaternion.Euler(0, 90, 0);
}

enum MovementType
{
    ROTATE,
    TRANSLATE
}

enum MovementDirection
{
	UP,
	DOWN,
	LEFT,
	RIGHT
}

class CreatureMovementAction
{
    public MovementType MovType;
	public MovementDirection MovDir;
	float TimeStarted;
    Vector3 TargetPosition;
    Quaternion TargetRotation;
	
	public CreatureMovementAction(MovementType mType, MovementDirection mDir)
	{
		MovType = mType;
		MovDir = mDir;	
	}
	
	public void StartOn(CreatureMover mover)
	{
		float SQUARE_SIZE = 5.0f;
		TimeStarted = Time.time;
		
		switch (MovType)
		{
		case MovementType.ROTATE:
			switch (MovDir)
			{
			case MovementDirection.UP:
				TargetRotation = Heading.UP_HEADING;
				break;
			case MovementDirection.DOWN:
				TargetRotation = Heading.DOWN_HEADING;
				break;
			case MovementDirection.RIGHT:
				TargetRotation = Heading.RIGHT_HEADING;
				break;
			case MovementDirection.LEFT:
				TargetRotation = Heading.LEFT_HEADING;
				break;
			}
			break;
		case MovementType.TRANSLATE:
			// Calculate the Target based on the first time we see the mover
			switch (MovDir)
			{
			case MovementDirection.UP:
				TargetPosition = mover.transform.position + new Vector3(-SQUARE_SIZE, 0, 0);
				break;
			case MovementDirection.DOWN:
				TargetPosition = mover.transform.position + new Vector3(SQUARE_SIZE, 0, 0);
				break;
			case MovementDirection.RIGHT:
				TargetPosition = mover.transform.position + new Vector3(0, 0, SQUARE_SIZE);
				break;
			case MovementDirection.LEFT:
				TargetPosition = mover.transform.position + new Vector3(0, 0, -SQUARE_SIZE);
				break;
				
			}
			break;
		}
	}
	
	public bool ApplyTo(CreatureMover mover)
	{
		//TODO: Either by animation or some interesting math make it look like "impulse and decay" instead
		//      of just linear advancing	
		
		switch (MovType)
        {
            case MovementType.TRANSLATE:
                Vector3 azimuth = mover.gameObject.transform.position - TargetPosition; 
				Debug.Log(azimuth.sqrMagnitude);
				if (azimuth.sqrMagnitude < 0.001)
				{
					mover.gameObject.transform.position = TargetPosition;
					return true;
				}
				else
				{
                	azimuth.Normalize();
                	mover.gameObject.transform.Translate(-1 * azimuth * Time.deltaTime * mover.Speed, Space.World);
				}
                break;
                
            case MovementType.ROTATE:
				if (Quaternion.Angle(mover.gameObject.transform.localRotation, TargetRotation) < 0.01)
				{
					mover.gameObject.transform.localRotation = TargetRotation;
					return true;
				}
				else
				{
					float t = Time.time - TimeStarted;
    	            mover.gameObject.transform.localRotation = Quaternion.Slerp(mover.gameObject.transform.localRotation, TargetRotation, t * mover.Speed / 8);
				}
                break;
        }
		return false;
	}
}


public class CreatureMover : MonoBehaviour
{

    public float Speed;
	public Vector2 InitialPosition;
    
    CreatureMovementAction CurrentAction;
    Queue<CreatureMovementAction> ActionStream;
    
    enum Headings
    {
        Up,
        Down,
        Left,
        Right,
    }
    
    void Start ()
    { 
        ActionStream = new Queue<CreatureMovementAction>();
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, MovementDirection.RIGHT));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, MovementDirection.UP));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.UP));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.UP));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.DOWN));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
		ActionStream.Enqueue(new CreatureMovementAction(MovementType.ROTATE, MovementDirection.RIGHT));
		gameObject.transform.Translate(InitialPosition.y, 0, -InitialPosition.x);
    }
    
    // Update is called once per frame
    void Update ()
    {
		
        if (CurrentAction == null)
        {
			if (ActionStream.Count > 0)
			{
				CurrentAction = ActionStream.Dequeue();
				CurrentAction.StartOn(this);
			}
			else
			{
				var values = Enum.GetValues(typeof(MovementType));
				System.Random random = new System.Random();
				MovementType t = (MovementType)values.GetValue(random.Next(values.Length));
				
				values = Enum.GetValues(typeof(MovementDirection));
				MovementDirection d = (MovementDirection) values.GetValue(random.Next(values.Length));
				
				CurrentAction = new CreatureMovementAction(t, d);
				CurrentAction.StartOn(this);
			}
        }
		
		if (CurrentAction.ApplyTo(this))
		{
			CurrentAction = null;	
		}
		
		/*
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
		*/
    }
}