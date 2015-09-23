using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Types;

public class UnitActionHandler : MonoBehaviour
{
    // Boolean determing if we want to see stuff in the console.
    public bool debuging;

    #region Variables and Start/Awake

    private UnitStats UnitStats;

    public ActionType curentActionType;
    
    // Use this for initialization
    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    #endregion

    public void SetAction(GameObject worldObject, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.ChairClimb:

                curentActionType = actionType;
                if (!UnitStats.ChairStats)
                    UnitStats.ChairStats = worldObject.GetComponent<ChairStats>();

                CalculateStartPointOfAction();

                break;
            default:
                break;
        }
    }

    public void SetAction(GameObject worldObject, ActionType actionType, LadderTriggerInput ladderTriggerInput,bool cancelFutureActions = false, bool clickedOutside = false)
    {
        switch (actionType)
        {
            case ActionType.Ladder:

                curentActionType = actionType;

                if (!UnitStats.LadderStats)
                    UnitStats.LadderStats = worldObject.GetComponent<LadderStats>();

                CalculateStartPointOfAction();

                UnitStats.UnitLadderAction.CalculateLadderPath(ladderTriggerInput);

                break;

            default:
                break;
        }
    }

    // We calculate wich part of the 'real' gameObject is closer to the player, and set a path to that object.
    public void CalculateStartPointOfAction()
    {
        var playerPos = UnitStats.thisTransform.position;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                float[] distancesLadder = new float[2];
                distancesLadder[(int)LadderStartPoint.Bottom] = Vector3.Distance(playerPos, UnitStats.LadderStats.StartPoint_Bottom.position);
                distancesLadder[(int)LadderStartPoint.Level2_Top] = Vector3.Distance(playerPos, UnitStats.LadderStats.StartPoint_Level2_Top.position);

                var smallestLadderDistance = Logic.GetSmallestDistance(distancesLadder);
                UnitStats.UnitLadderAction.SetPathToStartPoint((LadderStartPoint)smallestLadderDistance);

                break;

            case ActionType.ChairClimb:

                float[] distancesChair = new float[3];
                distancesChair[(int)ChairStartPoint.Front] = Vector3.Distance(playerPos, UnitStats.ChairStats.StartPoint_Front.position);
                distancesChair[(int)ChairStartPoint.Left] = Vector3.Distance(playerPos, UnitStats.ChairStats.StartPoint_Left.position);
                distancesChair[(int)ChairStartPoint.Right] = Vector3.Distance(playerPos, UnitStats.ChairStats.StartPoint_Right.position);

                //distances[(int)ChairStartPoint.Back] = Vector3.Distance(playerPos, UnitStats.ChairStats.StartPoint_Back.position);

                var smallestChairDistance = Logic.GetSmallestDistance(distancesChair);
                UnitStats.UnitChairAction.SetPathToStartPoint((ChairStartPoint)smallestChairDistance);

                break;

            default:
                break;   // Never enter here !
        }
    }

    public int CalculateEndPointOfAction()
    {
        var targetPos = UnitStats.thisUnitTarget.transform.position;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                //var distanceToStartPointGround = Vector3.Distance(targetPos, UnitStats.LadderStats.StartPoint_Ground.position);
                //var distanceToStartPoint4m = Vector3.Distance(targetPos, UnitStats.LadderStats.StartPoint_4m.position);

                //if (distanceToStartPointGround < distanceToStartPoint4m)
                //{
                //    return 2;
                //}
                //else
                //{
                //    return 5;
                //}

            default:
                return 0;   // Never enter here !
        }
    }

    public void StartAction()
    {
        switch (curentActionType)
        {
            case ActionType.Ladder:

                UnitStats.UnitLadderAction.PlayActionAnimation();
                break;

            case ActionType.ChairClimb:

                UnitStats.UnitChairAction.PlayActionAnimation();
                break;

            default:
                break;
        }
    }

    //  Cancel the curent action thorugh visual input. (ex: Climb down the ladder.)
    public void ExitCurentAction()
    {

    }

    // Call when you cancel an action.
    public void ResetActions()
    {

    }

    // The calls and getters to the individual actions.
    private void setLastAction(bool value)
    {

    }

    public bool getIsPlayingAction()
    {
        return false;
    }

    public void setIsPlayingAction(bool value)
    {
        
    }
}
