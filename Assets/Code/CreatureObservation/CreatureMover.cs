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
    UP = 0,
    LEFT = 1,
    DOWN = 2,
    RIGHT = 3,
	
	POSITIVE,
	NEGATIVE,
}

class MovementFactory
{
	public static CreatureMovementAction CreateFromStep(CreatureMover mover, PhysicsStep step)
	{
		MovementType type = step.LastAction.Direction == ActionDirection.ROTATION ? MovementType.ROTATE : MovementType.TRANSLATE;
		
		Vector3 tPos = Vector3.zero;
		Quaternion tRot = Quaternion.identity;
		if (type == MovementType.TRANSLATE)
		{
			tPos = CreatureMover.SimulationPositionToScenePosition(step.Position);
			Debug.Log(tPos);
		}
		
		if (type == MovementType.ROTATE)
		{
			tRot = CreatureMover.SimulationRotationToSceneRotation(step.Orientation);
		}
		
		if (step.Collisions.Count != 0)
		{
			return new CollisionAction(type, tPos, tRot);
		}
		else
		{
			return new CreatureMovementAction(type, tPos, tRot);
		}
	}
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
	
	public CreatureMovementAction(MovementType mType, Vector3 tPos, Quaternion tRot )
	{
        MovType = mType;
		TargetPosition = tPos;
		TargetRotation = tRot;
	}
	
//	[{"Control":{"Hidden":[0.9177292585372925,0.8209009766578674,0.8652114272117615,0.8968551158905029,0.7769179940223694,0.8013090491294861],"Inputs":[0.7400000095367432,0.05000000074505806,0.0,1.0],"Outputs":[]},"ID":8,"Physics":{"ID":8,"Orientation":"UP","Position":[74.0,5.0],"collisions":[],"last_action":{}}}
	
    public virtual void StartOn(CreatureMover mover)
    {
        TimeStarted = Time.time;
		Speed = mover.Speed;
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
		Debug.Log("WOOOPA!");
    }
	public CollisionAction(MovementType mType, Vector3 tPos, Quaternion tRot ) : base(mType, tPos, tRot)
	{
		Debug.Log("WEEPA!");
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
    public const float SQUARE_SIZE = 2.5f;
    public const float TOTAL_SIZE = 125f;
    public float Speed;
    public Vector2 InitialPosition;
	
	Vector2  SimulationPosition;
    
    CreatureMovementAction CurrentAction;
    Queue<CreatureMovementAction> ActionStream;
    
	public static Vector3 SimulationPositionToScenePosition(Vector2 simPos)
	{
		return new Vector3(simPos.x * SQUARE_SIZE - TOTAL_SIZE, 0, simPos.y * SQUARE_SIZE - TOTAL_SIZE );
	}
	
	public static Quaternion SimulationRotationToSceneRotation(int orientation)
	{
		MovementDirection MovDir = (MovementDirection)orientation;
		Quaternion ReturnRotation = Quaternion.identity;
		switch (MovDir)
		{
		case MovementDirection.UP:
			ReturnRotation = Heading.UP_HEADING;
			break;
		case MovementDirection.DOWN:
			ReturnRotation = Heading.DOWN_HEADING;
			break;
		case MovementDirection.RIGHT:
			ReturnRotation = Heading.RIGHT_HEADING;
			break;
		case MovementDirection.LEFT:
			ReturnRotation = Heading.LEFT_HEADING;
			break;
		}	
		return ReturnRotation;
	}
	
	public void SetDataFromModel(IndividualModel model)
	{
		InitialPosition = model.Physics.StartingPos;
        gameObject.transform.localPosition = SimulationPositionToScenePosition(InitialPosition);
	}
	
	public void AddStep(PhysicsStep step)
	{
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, step));
	}
    
    void Start ()
    { 
        ActionStream = new Queue<CreatureMovementAction>();
/*		
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 1.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 2.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 3.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 4.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 5.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 6.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 8.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 9.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 11.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 12.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 13.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 14.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 15.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		ActionStream.Enqueue( MovementFactory.CreateFromStep(this, JsonUtility.FromJson<PhysicsStep>("{\"ID\":70,\"Orientation\":\"UP\",\"Position\":{\"x\": 16.0, \"y\": 10.0},\"Collisions\":[],\"LastAction\":{\"Direction\":\"HORIZONTAL\",\"Impulse\":0.8759307861328125}}")));
		*/
        gameObject.transform.localPosition = SimulationPositionToScenePosition(InitialPosition);
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
				return;
            }
        }
        
        if (CurrentAction.ApplyTo(this))
        {
            CurrentAction = null;    
        }
    }
}