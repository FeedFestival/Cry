using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public bool debuging = false;
    /*
     This script recieves info about where the unit should go, if it should stop or resume its movement.
     */
    UnitStats UnitStats;

    public bool IsWalking { get; set; }

    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    public void StopMoving(bool targetReached = true)
    {
        if (debuging)
            Debug.Log("Reaching (AnActionIsSet = " + UnitStats.UnitBasicAnimation.GetIsDoingAction() + ")");

        IsWalking = false;
        UnitStats.AIPath.stopMoving();

        //  If we finish the journey and an action is set. That mean we must fire the action
        if (targetReached && UnitStats.UnitBasicAnimation.GetIsDoingAction())
            UnitStats.UnitActionHandler.StartAction();
        else
            UnitStats.UnitBasicAnimation.GoIdle();
    }

    public void ResumeMoving()
    {
        IsWalking = true;
        UnitStats.AIPath.resumeMoving();
        UnitStats.UnitBasicAnimation.GoWalk();
    }

    public void GoToTarget()
    {
        IsWalking = true;
        UnitStats.AIPath.SearchPath();
        UnitStats.UnitBasicAnimation.GoWalk();
    }

    public void SetPathToTarget(Vector3 targetVector)
    {
        UnitStats.thisUnitTarget.transform.position = targetVector;
        this.GoToTarget();
    }
}
