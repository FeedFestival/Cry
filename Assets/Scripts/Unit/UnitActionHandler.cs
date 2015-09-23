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

    private Unit Unit;

    public ActionType curentActionType;
    
    // Use this for initialization
    public void Initialize(Unit unit)
    {
        Unit = unit;
        curentActionType = ActionType.None;
    }

    #endregion

    public void SetAction(GameObject worldObject, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.ChairClimb:

                curentActionType = actionType;
                if (!Unit.ChairStats)
                    Unit.ChairStats = worldObject.GetComponent<ChairStats>();

                CalculateStartPointOfAction();

                break;
            default:
                break;
        }
    }

    public void SetAction(GameObject worldObject, ActionType actionType, LadderTriggerInput ladderTriggerInput)
    {
        switch (actionType)
        {
            case ActionType.Ladder:

                if (Unit.UnitActionState == UnitActionState.None)
                {
                    curentActionType = actionType;

                    if (!Unit.LadderStats)
                        Unit.LadderStats = worldObject.GetComponent<LadderStats>();

                    CalculateStartPointOfAction();

                    Unit.UnitLadderAction.CalculateLadderPath(ladderTriggerInput);
                }
                else if (Unit.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    Unit.UnitLadderAction.CalculateLadderPath(ladderTriggerInput, true);
                }

                break;

            default:
                break;
        }
    }

    // We calculate wich part of the 'real' gameObject is closer to the player, and set a path to that object.
    public void CalculateStartPointOfAction()
    {
        var playerPos = Unit.UnitProperties.thisTransform.position;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                float[] distancesLadder = new float[2];
                distancesLadder[(int)LadderStartPoint.Bottom] = Vector3.Distance(playerPos, Unit.LadderStats.StartPoint_Bottom.position);
                distancesLadder[(int)LadderStartPoint.Level2_Top] = Vector3.Distance(playerPos, Unit.LadderStats.StartPoint_Level2_Top.position);

                var smallestLadderDistance = Logic.GetSmallestDistance(distancesLadder);
                Unit.UnitLadderAction.SetPathToStartPoint((LadderStartPoint)smallestLadderDistance);

                break;

            case ActionType.ChairClimb:

                float[] distancesChair = new float[3];
                distancesChair[(int)ChairStartPoint.Front] = Vector3.Distance(playerPos, Unit.ChairStats.StartPoint_Front.position);
                distancesChair[(int)ChairStartPoint.Left] = Vector3.Distance(playerPos, Unit.ChairStats.StartPoint_Left.position);
                distancesChair[(int)ChairStartPoint.Right] = Vector3.Distance(playerPos, Unit.ChairStats.StartPoint_Right.position);

                //distances[(int)ChairStartPoint.Back] = Vector3.Distance(playerPos, Unit.ChairStats.StartPoint_Back.position);

                var smallestChairDistance = Logic.GetSmallestDistance(distancesChair);
                Unit.UnitChairAction.SetPathToStartPoint((ChairStartPoint)smallestChairDistance);

                break;

            default:
                break;   // Never enter here !
        }
    }

    public int CalculateEndPointOfAction()
    {
        var targetPos = Unit.UnitProperties.thisUnitTarget.transform.position;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                //var distanceToStartPointGround = Vector3.Distance(targetPos, Unit.LadderStats.StartPoint_Ground.position);
                //var distanceToStartPoint4m = Vector3.Distance(targetPos, Unit.LadderStats.StartPoint_4m.position);

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
        Unit.UnitPrimaryState = UnitPrimaryState.Busy;
        Unit.UnitActionInMind = UnitActionInMind.None;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                Unit.UnitActionState = UnitActionState.ClimbingLadder;

                //  This also sets the Unit to follow the pivot of the ladder to be guided throut the animation
                Unit.UnitProperties.Root = Unit.LadderStats.Root;

                Unit.UnitLadderAction.PlayActionAnimation();
                break;

            case ActionType.ChairClimb:

                Unit.UnitActionState = UnitActionState.ClimbingChair;

                Unit.UnitProperties.Root = Unit.ChairStats.Root;

                Unit.UnitChairAction.PlayActionAnimation();
                break;

            default:
                break;
        }
    }

    public void ExitCurentAction()
    {
        Unit.UnitProperties.Root = null;

        Unit.UnitPrimaryState = UnitPrimaryState.Idle;
        Unit.UnitActionState = UnitActionState.None;
        Unit.UnitActionInMind = UnitActionInMind.None;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                Unit.LadderStats = null;
                Unit.UnitLadderAction.LadderPath = null;
                break;

            case ActionType.ChairClimb:

                Unit.ChairStats = null;

                break;

            default:
                break;
        }

        curentActionType = ActionType.None;
        Unit.UnitController.StopMoving();
    }
}
