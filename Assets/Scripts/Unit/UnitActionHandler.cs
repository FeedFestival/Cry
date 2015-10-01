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
            case ActionType.Ladder:

                if (Unit.UnitActionState == UnitActionState.None)
                {
                    curentActionType = actionType;

                    if (!Unit.Ladder)
                        Unit.Ladder = worldObject.GetComponent<Ladder>();

                    Unit.Ladder.LadderActionHandler.CalculateStartPoint();
                    this.SetPathToStartPoint();

                    Unit.Ladder.LadderActionHandler.CalculateLadderPath(Unit.Ladder.LadderTriggerInput);
                }
                else if (Unit.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    Unit.Ladder.LadderActionHandler.CalculateLadderPath(Unit.Ladder.LadderTriggerInput, true);
                }

                break;

            case ActionType.ChairClimb:

                curentActionType = actionType;
                if (!Unit.Chair)
                    Unit.Chair = worldObject.GetComponent<Chair>();

                Unit.Chair.ChairActionHandler.CalculateStartPoint();

                break;
            case ActionType.LedgeClimb:

                curentActionType = actionType;

                if (!Unit.Ledge)
                    Unit.Ledge = worldObject.GetComponent<Ledge>();

                SetPathToStartPoint();

                break;
            default:
                break;
        }
    }

    public void SetPathToStartPoint()
    {
        switch (curentActionType)
        {
            case ActionType.None:
                break;
            case ActionType.Ladder:

                this.Unit.UnitActionInMind = UnitActionInMind.ClimbingLadder;
                switch (Unit.Ladder.LadderStartPoint)
                {
                    case LadderStartPoint.Bottom:

                        this.Unit.UnitController.SetPathToTarget(Unit.Ladder.StartPoint_Bottom.position);
                        break;

                    case LadderStartPoint.Level2_Top:

                        this.Unit.UnitController.SetPathToTarget(Unit.Ladder.StartPoint_Level2_Top.position);
                        break;

                    default: break;
                }
                break;
            case ActionType.ChairClimb:

                this.Unit.UnitActionInMind = UnitActionInMind.ClimbingChair;
                switch (Unit.Chair.ChairStartPoint)
                {
                    case ChairStartPoint.Front:

                        this.Unit.UnitController.SetPathToTarget(Unit.Chair.StartPoint_Front.position);
                        break;

                    case ChairStartPoint.Left:

                        this.Unit.UnitController.SetPathToTarget(Unit.Chair.StartPoint_Left.position);
                        break;

                    case ChairStartPoint.Right:

                        this.Unit.UnitController.SetPathToTarget(Unit.Chair.StartPoint_Right.position);
                        break;

                    case ChairStartPoint.Back:

                        this.Unit.UnitController.SetPathToTarget(Unit.Chair.StartPoint_Back.position);
                        break;

                    default: break;
                }
                break;
            case ActionType.ChairGrab:
                break;
            case ActionType.LedgeClimb:

                this.Unit.UnitActionInMind = UnitActionInMind.ClimbingWall;

                this.Unit.UnitController.SetPathToTarget(Unit.Ledge.StartPointPosition);
                
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
                Unit.Ladder.LadderActionHandler.CalculateLadderCursor();   // DOUBLE_CHECK

                //  This also sets the Unit to follow the pivot of the ladder to be guided throut the animation
                Unit.UnitProperties.Root = Unit.Ladder.Root;

                Unit.Ladder.LadderActionHandler.PlayActionAnimation();
                break;

            case ActionType.ChairClimb:

                Unit.UnitActionState = UnitActionState.ClimbingChair;

                Unit.UnitProperties.Root = Unit.Chair.Root;

                break;

            case ActionType.LedgeClimb:

                Unit.UnitActionState = UnitActionState.ClimbingWall;

                Unit.UnitProperties.Root = Unit.Ledge.Root;

                Unit.Ledge.LedgeActionHandler.PlayActionAnimation(Unit);

                break;

            default:
                break;
        }
    }

    public void ExitCurentAction()
    {
        Unit.UnitPrimaryState = UnitPrimaryState.Idle;
        Unit.UnitActionState = UnitActionState.None;
        Unit.UnitActionInMind = UnitActionInMind.None;

        Unit.UnitController.ExitAction();

        switch (curentActionType)
        {
            case ActionType.Ladder:

                Unit.Ladder.LadderActionHandler.LadderPath = null;
                Unit.Ladder = null;
                break;

            case ActionType.ChairClimb:

                Unit.Chair = null;
                break;

            case ActionType.LedgeClimb:

                Unit.Ledge.ResetLedgeAction();
                Unit.Ledge = null;
                break;

            default:
                break;
        }

        curentActionType = ActionType.None;
        
    }
}
