using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitController : MonoBehaviour
{
    public bool debuging = false;
    /*
     This script recieves info about where the unit should go, if it should stop or resume its movement.
     */
    private Unit Unit;

    public void Initialize(Unit unit)
    {
        Unit = unit;
    }

    public void StopMoving(bool targetReached = true)
    {
        if (debuging)
            Debug.Log("Reaching (Unit action in mind = " + Unit.UnitActionInMind + ")");

        Unit.AIPath.stopMoving();

        if (targetReached)
        {
            //  If we finish the journey
            //      - And an action is set in mind
            //      - And the Unit is not busy with another action.
            //  --> That means we must fire the Action in mind and exit.
            if (Unit.UnitActionInMind != UnitActionInMind.None && Unit.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                Unit.UnitActionHandler.StartAction();

                return;
            }

            //  If we finish the journey
            //      - And for some reason the unit is Busy - dont go into Idle.
            if (Unit.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                Unit.UnitPrimaryState = UnitPrimaryState.Idle;
                Unit.UnitBasicAnimation.GoIdle();
            }
        }
        else
        {
            Unit.UnitPrimaryState = UnitPrimaryState.Idle;
            Unit.UnitBasicAnimation.GoIdle();

            Unit.UnitProperties.thisUnitTarget.thisTransform.position = Unit.UnitProperties.thisTransform.position;
        }
    }

    public void ResumeMoving()
    {
        Unit.AIPath.resumeMoving();
        Unit.UnitPrimaryState = UnitPrimaryState.Walk;
        Unit.UnitBasicAnimation.GoWalk();
    }

    public void GoToTarget()
    {
        ResumeMoving();
        Unit.AIPath.SearchPath();
    }

    public void SetPathToTarget(Vector3 targetVector)
    {
        Unit.UnitProperties.thisUnitTarget.transform.position = targetVector;
        this.GoToTarget();
    }

    void Update()
    {
        if (Unit != null && Unit.UnitProperties.ControllerFollowRoot)
        {
            Unit.UnitProperties.thisTransform.position = new Vector3(Unit.UnitProperties.Root.position.x, Unit.UnitProperties.Root.position.y + 1, Unit.UnitProperties.Root.position.z);
            var rot = new Quaternion();
            rot.eulerAngles = new Vector3(Unit.UnitProperties.Root.eulerAngles.x + 90, Unit.UnitProperties.Root.eulerAngles.y + 180, Unit.UnitProperties.Root.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(Unit.UnitProperties.thisTransform.rotation, rot, Time.deltaTime * 5);
        }
        if (Unit.UnitPrimaryState == UnitPrimaryState.Walk)
        {
            var pos = Unit.UnitProperties.FeetCollider.transform.position;

            var posDown =new Vector3(pos.x,pos.y - 2f,pos.z);

            var direction = posDown - pos;

            if (debuging)
                Debug.DrawLine(pos, (pos + direction));

            Ray Ray = new Ray(pos, direction);
            RaycastHit Hit;
            if (Physics.Raycast(Ray, out Hit, 100))
            {
                if (Hit.transform.tag == "Map")
                {
                    if (debuging)
                        Debug.DrawLine(pos, (pos + direction), Color.red);
                    Unit.UnitProperties.thisTransform.position = new Vector3(Unit.UnitProperties.thisTransform.position.x,
                                                                            Hit.point.y + 1f,
                                                                            Unit.UnitProperties.thisTransform.position.z);
                }
            }
        }
    }

}
