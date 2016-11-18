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
    TRANSLATE,
    COLLIDE
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
    protected float TimeStarted;
	protected float Speed;
	protected Vector3 InitialPosition;
    protected Vector3 TargetPosition;
    protected Quaternion TargetRotation;
	
    
    public CreatureMovementAction(MovementType mType, MovementDirection mDir)
    {
        MovType = mType;
        MovDir = mDir;
    }
    
    public virtual void StartOn(CreatureMover mover)
    {
        float SQUARE_SIZE = 5.0f;
        TimeStarted = Time.time;
		Speed = mover.Speed;
        
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
		InitialPosition = mover.gameObject.transform.position;
    }
    
    public virtual bool ApplyTo(CreatureMover mover)
    {
        //TODO: Either by animation or some interesting math make it look like "impulse and decay" instead
        //      of just linear advancing    
        
        switch (MovType)
        {
            case MovementType.TRANSLATE:
                Vector3 azimuth = mover.gameObject.transform.position - TargetPosition; 
                
                if (azimuth.sqrMagnitude < 0.001)
                {
                    mover.gameObject.transform.position = TargetPosition;
                    return true;
                }
                else
                {
                    azimuth.Normalize();
                    mover.gameObject.transform.Translate(-1 * azimuth * Time.deltaTime * Speed, Space.World);
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
                    mover.gameObject.transform.localRotation = Quaternion.Slerp(mover.gameObject.transform.localRotation, TargetRotation, t * Speed / 8);
                }
                break;
        }
        return false;
    }
}

class CollisionAction: CreatureMovementAction
{
    int ShakeTimes;
    Vector3 ShakeTargetPosition;
    Quaternion ShakeTargetRotation;
	
    public CollisionAction(MovementType mType, MovementDirection mDir) : base(mType, mDir)
    {
    }

    public override void StartOn(CreatureMover mover)
    {
        base.StartOn(mover);
		ShakeTimes = 4;
        ShakeTargetPosition =  InitialPosition + ( (InitialPosition - TargetPosition) / 10.0f);
		TargetPosition = ShakeTargetPosition;
		Speed /= 2;
    }
    
    public override bool ApplyTo(CreatureMover mover)
    {
		if (base.ApplyTo(mover))
		{
			if (ShakeTimes == 0)
				return true;
				
			ShakeTimes -= 1;
			TargetPosition = ShakeTimes % 2 == 0  ? InitialPosition : ShakeTargetPosition;
			Debug.Log("XXXXXXXXXXXX");
			Debug.Log(TargetPosition);
			Debug.Log(ShakeTargetPosition);
			Debug.Log(InitialPosition);
			Debug.Log("XXXXXXXXXXXX");
            TimeStarted = Time.time;
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
        ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CreatureMovementAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
        ActionStream.Enqueue(new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT));
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
				// TODO: TEMP Implementation
                CurrentAction = new CollisionAction(MovementType.TRANSLATE, MovementDirection.RIGHT);
                CurrentAction.StartOn(this);
				return;
            }
        }
        
        if (CurrentAction.ApplyTo(this))
        {
            CurrentAction = null;    
        }
    }
}