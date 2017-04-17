using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//TMP
class Heading
{
    public static Quaternion UP_HEADING    = Quaternion.Euler(0,    0, 0);
    public static Quaternion DOWN_HEADING  = Quaternion.Euler(0,  180, 0);
    public static Quaternion LEFT_HEADING  = Quaternion.Euler(0,  270, 0);
    public static Quaternion RIGHT_HEADING = Quaternion.Euler(0,   90, 0);
}

enum MovementType
{
    ROTATE,
    TRANSLATE,
    COLLIDE
}

class MovementFactory
{
    public static CreatureMovementAction CreateFromStep(CreatureMover mover, PhysicsStep step)
    {
        MovementType type = step.LastAction.EDirection == ActionDirection.ROTATION ? MovementType.ROTATE : MovementType.TRANSLATE;
        
        Vector3 tPos = Vector3.zero;
        Quaternion tRot = Quaternion.identity;
        if (type == MovementType.TRANSLATE)
        {
            tPos = CreatureMover.SimulationPositionToScenePosition(step.Position);
        }
        
        if (type == MovementType.ROTATE)
        {
            tRot = CreatureMover.SimulationRotationToSceneRotation(step.EOrientation);
        }
        
        return new CreatureMovementAction(type, tPos, tRot, step);
    }
}

class CreatureMovementAction
{
    public MovementType MovType;
    public MovementDirection MovDir;
    public PhysicsStep Step;
    protected float TimeStarted;
    protected float Speed;
    public Vector3 InitialPosition;
    protected Vector3 TargetPosition;
    protected Quaternion TargetRotation;
    
    public CreatureMovementAction(MovementType mType, MovementDirection mDir)
    {
        MovType = mType;
        MovDir = mDir;
    }
    
    public CreatureMovementAction(MovementType mType, Vector3 tPos, Quaternion tRot, PhysicsStep step)
    {
        MovType = mType;
        TargetPosition = tPos;
        TargetRotation = tRot;
        Step = step;        
    }
    
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
                
                if (azimuth.sqrMagnitude < 0.05)
                {
                    mover.gameObject.transform.position = TargetPosition;
                    return true;
                }
                else
                {
                    azimuth.Normalize();
                    mover.gameObject.transform.Translate(-1 * azimuth * Time.deltaTime * Speed * 1.25f, Space.World);
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
                    mover.gameObject.transform.localRotation = Quaternion.Slerp(mover.gameObject.transform.localRotation, TargetRotation, t * 0.5f * Speed / 2);
                }
                break;
        }
        return false;
    }
}

class CollisionAction: CreatureMovementAction
{
    
    float WaitTime = 1.0f;
    public CollisionAction(MovementType mType, MovementDirection mDir) : base(mType, mDir)
    {
    }
    public CollisionAction(MovementType mType, Vector3 tPos, Quaternion tRot ) : base(mType, tPos, tRot, null)
    {
    }

    public override void StartOn(CreatureMover mover)
    {
        base.StartOn(mover);
    }
    
    public override bool ApplyTo(CreatureMover mover)
    {
        return true; 
    }

}



public class CreatureMover : MonoBehaviour
{
    public const float SQUARE_SIZE = 2.5f;
    public const float TOTAL_SIZE = 125f;
    public float Speed;
    public Vector2 InitialPosition;
    public int ExecutedSteps {get; private set;}
    public string DebugText = "\n";
    
    Vector2  SimulationPosition;
    
    CreatureMovementAction CurrentAction;
    Queue<CreatureMovementAction> ActionStream;
    
    public static Vector3 SimulationPositionToScenePosition(Vector2 simPos)
    {
        return new Vector3(simPos.x * SQUARE_SIZE - TOTAL_SIZE, 0, simPos.y * SQUARE_SIZE - TOTAL_SIZE );
    }

    public static Vector3 SimulationScaleToSceneScale(Vector2 simPos)
    {
        return new Vector3(simPos.x * SQUARE_SIZE, 1.0f, simPos.y * SQUARE_SIZE);
    }
    
    public static Quaternion SimulationRotationToSceneRotation(MovementDirection MovDir)
    {
        //MovementDirection MovDir = (MovementDirection)orientation;
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
        Speed = Mathf.Min(model.Speed, 4);
        //SetInitialPosition(model.Physics.Position);
    }
    
    public void SetInitialPosition(Vector2 v)
    {
        InitialPosition = v;
        gameObject.transform.localPosition = SimulationPositionToScenePosition(InitialPosition);
    }
    
    public void SetInitialOrientation(MovementDirection or)
    {
        gameObject.transform.localRotation = SimulationRotationToSceneRotation(or);
    }
    
    public void AddStep(PhysicsStep step)
    {
        ActionStream.Enqueue( MovementFactory.CreateFromStep(this, step));
    }
    
    void Start ()
    { 
        ActionStream = new Queue<CreatureMovementAction>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        
        if (CurrentAction == null)
        {
            if (ActionStream.Count > 0)
            {
                CurrentAction = ActionStream.Dequeue();
                
                DebugText = string.Format("\nDebug Movement: {0} {1} Speed: {2}", CurrentAction.Step.Orientation, CurrentAction.Step.Position, Speed);

                CurrentAction.StartOn(this);
                ExecutedSteps++;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (CurrentAction.ApplyTo(this))
            {
                CurrentAction = null;    
            }
        }
    }
    
    public int GetRemainingSteps()
    {
        if (ActionStream == null)
        {
            return 0;
        }
        return ActionStream.Count;
    }
}