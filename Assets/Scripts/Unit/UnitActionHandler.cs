using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Types;

public class UnitActionHandler : MonoBehaviour
{
    // Boolean determing if we want to see stuff in the console.
    public bool debuging;

    private Unit Unit;

    public ActionType curentActionType;
    
    // Use this for initialization
    public void Initialize(Unit unit)
    {
        Unit = unit;
        curentActionType = ActionType.None;
    }

    public void SetAction(GameObject worldObject, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.ChairClimb:

                curentActionType = actionType;
                if (!Unit.ChairStats)
                    Unit.ChairStats = worldObject.GetComponent<ChairStats>();

                Unit.ChairStats.ChairActionHandler.CalculateStartPoint();

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

                    Unit.UnitLadderAction.SetPathToStartPoint(Unit.LadderStats.LadderActionHandler.CalculateStartPoint());

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

    public void StartAction()
    {
        Unit.UnitPrimaryState = UnitPrimaryState.Busy;
        Unit.UnitActionInMind = UnitActionInMind.None;

        switch (curentActionType)
        {
            case ActionType.Ladder:

                Unit.UnitActionState = UnitActionState.ClimbingLadder;
                Unit.LadderStats.LadderActionHandler.CalculateLadderCursor();   // DOUBLE_CHECK

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
