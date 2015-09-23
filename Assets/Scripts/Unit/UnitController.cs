using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitController : MonoBehaviour
{
    public bool debuging = false;
    /*
     This script recieves info about where the unit should go, if it should stop or resume its movement.
     */
    private UnitStats UnitStats;

    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    public void StopMoving(bool targetReached = true)
    {
        if (debuging)
            Debug.Log("Reaching (Unit action in mind = " + UnitStats.UnitActionInMind + ")");

        UnitStats.AIPath.stopMoving();

        if (targetReached)
        {
            //  If we finish the journey
            //      - And an action is set in mind
            //      - And the Unit is not busy with another action.
            //  --> That means we must fire the Action in mind and exit.
            if (UnitStats.UnitActionInMind != UnitActionInMind.None && UnitStats.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                UnitStats.UnitActionHandler.StartAction();

                return;
            }

            //  If we finish the journey
            //      - And for some reason the unit is Busy - dont go into Idle.
            if (UnitStats.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                UnitStats.UnitPrimaryState = UnitPrimaryState.Idle;
                UnitStats.UnitBasicAnimation.GoIdle();
            }
        }
        else
        {
            UnitStats.UnitPrimaryState = UnitPrimaryState.Idle;
            UnitStats.UnitBasicAnimation.GoIdle();

            UnitStats.thisUnitTarget.thisTransform.position = UnitStats.thisTransform.position;
        }
    }

    public void ResumeMoving()
    {
        UnitStats.AIPath.resumeMoving();
        UnitStats.UnitPrimaryState = UnitPrimaryState.Walking;
        UnitStats.UnitBasicAnimation.GoWalk();
    }

    public void GoToTarget()
    {
        ResumeMoving();
        UnitStats.AIPath.SearchPath();
    }

    public void SetPathToTarget(Vector3 targetVector)
    {
        UnitStats.thisUnitTarget.transform.position = targetVector;
        this.GoToTarget();
    }
}
