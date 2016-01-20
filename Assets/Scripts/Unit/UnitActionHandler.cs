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
            case ActionType.TableClimb:

                curentActionType = actionType;

                if (!Unit.Table)
                    Unit.Table = worldObject.GetComponent<Table>();

                SetPathToStartPoint();

                break;
            case ActionType.GrabTable:

                curentActionType = actionType;

                if (!Unit.Table)
                    Unit.Table = worldObject.GetComponent<Table>();

                SetPathToStartPoint();

                break;

            case ActionType.PickupObject:

                curentActionType = actionType;

                if (!Unit.InteractiveObject)
                    Unit.InteractiveObject = worldObject.GetComponent<InteractiveObject>();

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

            case ActionType.TableClimb:

                this.Unit.UnitActionInMind = UnitActionInMind.ClimbTable;
                this.Unit.UnitController.SetPathToTarget(Unit.Table.TableProperties.StartPointPosition);
                break;

            case ActionType.GrabTable:

                this.Unit.UnitActionInMind = UnitActionInMind.MovingTable;
                this.Unit.UnitController.SetPathToTarget(Unit.Table.TableProperties.StartPointPosition);
                break;

            case ActionType.PickupObject:

                this.Unit.UnitActionInMind = UnitActionInMind.PickupObject;
                this.Unit.UnitController.SetPathToTarget(Unit.InteractiveObject.StartPointPosition);
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

                //  This also sets the Player to follow the pivot of the ladder to be guided throut the animation
                Unit.UnitProperties.Root = Unit.Ladder.Root;

                Unit.Ladder.LadderActionHandler.PlayActionAnimation();
                break;

            case ActionType.ChairClimb:

                Unit.UnitActionState = UnitActionState.ClimbingChair;

                Unit.UnitProperties.Root = Unit.Chair.Root;

                break;

            case ActionType.LedgeClimb:

                if (Unit.Table)
                {
                    Unit.Table.ExitTableAction(true);
                }

                Unit.Ledge.Unit = Unit;
                Unit.Ledge.ResetUI();

                Unit.UnitActionState = UnitActionState.ClimbingWall;

                Unit.UnitProperties.Root = Unit.Ledge.Root;

                Unit.Ledge.LedgeActionHandler.PlayActionAnimation();

                break;

            case ActionType.TableClimb:

                Unit.UnitActionState = UnitActionState.ClimbingTable;
                Unit.UnitActionInMind = UnitActionInMind.ClimbTable;

                Unit.UnitProperties.Root = Unit.Table.TableProperties.StaticRoot;

                Unit.Table.TableActionHandler.PlayActionAnimation(Unit);

                break;

            case ActionType.GrabTable:

                Unit.PlayerActionInMind = PlayerActionInMind.MovingTable;
                Unit.UnitActionState = UnitActionState.MovingTable;
                Unit.UnitActionInMind = UnitActionInMind.MovingTable;

                Unit.UnitProperties.Root = Unit.Table.TableProperties.StaticRoot;

                Unit.Table.TableActionHandler.PlayActionAnimation(Unit);
                break;

            case ActionType.PickupObject:

                // Play animation of picking up.
                Debug.Log("Pickup Object");

                this.ExitCurentAction();

                break;

            default:
                break;
        }
    }

    public void ExitCurentAction(bool toAnotherAction = false)
    {
        Unit.UnitPrimaryState = UnitPrimaryState.Idle;
        Unit.UnitActionState = UnitActionState.None;
        Unit.UnitActionInMind = UnitActionInMind.None;

        Unit.UnitController.ExitAction(toAnotherAction);

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

            case ActionType.TableClimb:

                Unit.Table.TableState = TableState.Static;
                Unit.Table.TableStaticAnimator.transform.localPosition = Vector3.zero;
                Unit.Table = null;
                break;

            case ActionType.GrabTable:

                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                Unit.PlayerActionInMind = PlayerActionInMind.Moving;

                Unit.Table.TableState = TableState.Static;

                Unit.Table = null;

                break;

            case ActionType.PickupObject:

                Unit.PlayerActionInMind = PlayerActionInMind.Moving;
                Unit.InteractiveObject = null;

                break;

            default:
                break;
        }

        curentActionType = ActionType.None;
    }

    public void ExitSpecificAction(ActionType action)
    {
        Unit.UnitController.ExitAction();

        switch (action)
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
                Debug.Log("ntra");
                Unit.Ledge = null;
                break;

            case ActionType.TableClimb:

                Unit.Table.TableState = TableState.Static;
                Unit.Table.TableStaticAnimator.transform.localPosition = Vector3.zero;
                Unit.Table = null;
                break;

            case ActionType.GrabTable:

                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                Unit.PlayerActionInMind = PlayerActionInMind.Moving;

                Unit.Table.TableState = TableState.Static;

                Unit.Table = null;

                break;

            default:
                break;
        }
    }
}
