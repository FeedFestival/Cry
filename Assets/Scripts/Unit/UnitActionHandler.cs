using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Utils;

public class UnitActionHandler : MonoBehaviour
{
    private Unit _unit;

    public ActionType CurentActionType;

    // Use this for initialization
    public void Initialize(Unit unit)
    {
        _unit = unit;
        CurentActionType = ActionType.None;
    }

    public void SetAction(GameObject worldObject, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Ladder:

                if (_unit.UnitActionState == UnitActionState.None)
                {
                    CurentActionType = actionType;

                    if (!_unit.Ladder)
                        _unit.Ladder = worldObject.GetComponent<Ladder>();

                    _unit.Ladder.LadderActionHandler.CalculateStartPoint();
                    this.SetPathToStartPoint();

                    _unit.Ladder.LadderActionHandler.CalculateLadderPath(_unit.Ladder.LadderTriggerInput);
                }
                else if (_unit.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    _unit.Ladder.LadderActionHandler.CalculateLadderPath(_unit.Ladder.LadderTriggerInput, true);
                }

                break;

            case ActionType.ChairClimb:

                CurentActionType = actionType;
                if (!_unit.Chair)
                    _unit.Chair = worldObject.GetComponent<Chair>();

                _unit.Chair.ChairActionHandler.CalculateStartPoint();

                break;
            case ActionType.LedgeClimb:

                CurentActionType = actionType;

                if (!_unit.Ledge)
                    _unit.Ledge = worldObject.GetComponent<Ledge>();

                SetPathToStartPoint();

                break;
            case ActionType.TableClimb:

                CurentActionType = actionType;

                if (!_unit.Table)
                    _unit.Table = worldObject.GetComponent<Table>();

                SetPathToStartPoint();

                break;
            case ActionType.GrabTable:

                CurentActionType = actionType;

                if (!_unit.Table)
                    _unit.Table = worldObject.GetComponent<Table>();

                SetPathToStartPoint();

                break;

            case ActionType.PickupObject:

                CurentActionType = actionType;

                if (_unit.Item == null)
                    _unit.Item = worldObject.GetComponent<InteractiveObject>().Item;

                SetPathToStartPoint();

                break;

            case ActionType.OpenCloseDoor:

                CurentActionType = actionType;
                if (!_unit.Door)
                    _unit.Door = worldObject.GetComponent<Door>();

                SetPathToStartPoint();

                break;

            default:
                break;
        }
    }

    public void SetPathToStartPoint()
    {
        switch (CurentActionType)
        {
            case ActionType.None:
                break;
            case ActionType.Ladder:

                this._unit.UnitActionInMind = UnitActionInMind.ClimbingLadder;
                switch (_unit.Ladder.LadderStartPoint)
                {
                    case LadderStartPoint.Bottom:

                        this._unit.UnitController.SetPathToTarget(_unit.Ladder.StartPoint_Bottom.position);
                        break;

                    case LadderStartPoint.Level2_Top:

                        this._unit.UnitController.SetPathToTarget(_unit.Ladder.StartPoint_Level2_Top.position);
                        break;

                    default: break;
                }
                break;
            case ActionType.ChairClimb:

                this._unit.UnitActionInMind = UnitActionInMind.ClimbingChair;
                switch (_unit.Chair.ChairStartPoint)
                {
                    case ChairStartPoint.Front:

                        this._unit.UnitController.SetPathToTarget(_unit.Chair.StartPoint_Front.position);
                        break;

                    case ChairStartPoint.Left:

                        this._unit.UnitController.SetPathToTarget(_unit.Chair.StartPoint_Left.position);
                        break;

                    case ChairStartPoint.Right:

                        this._unit.UnitController.SetPathToTarget(_unit.Chair.StartPoint_Right.position);
                        break;

                    case ChairStartPoint.Back:

                        this._unit.UnitController.SetPathToTarget(_unit.Chair.StartPoint_Back.position);
                        break;

                    default: break;
                }
                break;
            case ActionType.ChairGrab:
                break;
            case ActionType.LedgeClimb:

                this._unit.UnitActionInMind = UnitActionInMind.ClimbingWall;
                this._unit.UnitController.SetPathToTarget(_unit.Ledge.StartPointPosition);
                break;

            case ActionType.TableClimb:

                this._unit.UnitActionInMind = UnitActionInMind.ClimbTable;
                this._unit.UnitController.SetPathToTarget(_unit.Table.TableProperties.StartPointPosition);
                break;

            case ActionType.GrabTable:

                this._unit.UnitActionInMind = UnitActionInMind.MovingTable;
                this._unit.UnitController.SetPathToTarget(_unit.Table.TableProperties.StartPointPosition);
                break;

            case ActionType.PickupObject:

                this._unit.UnitActionInMind = UnitActionInMind.PickupObject;
                this._unit.UnitController.SetPathToTarget(_unit.Item.StartPointPosition);
                break;

            case ActionType.OpenCloseDoor:

                this._unit.UnitActionInMind = UnitActionInMind.OpenCloseDoor;
                this._unit.UnitController.SetPathToTarget(_unit.Door.StartPointPosition);
                break;

            default:
                break;
        }
    }

    public void StartAction()
    {
        _unit.UnitPrimaryState = UnitPrimaryState.Busy;
        _unit.UnitActionInMind = UnitActionInMind.None;

        switch (CurentActionType)
        {
            case ActionType.Ladder:

                _unit.UnitActionState = UnitActionState.ClimbingLadder;
                _unit.Ladder.LadderActionHandler.CalculateLadderCursor();   // DOUBLE_CHECK

                //  This also sets the Player to follow the pivot of the ladder to be guided throut the animation
                _unit.UnitProperties.Root = _unit.Ladder.Root;

                _unit.Ladder.LadderActionHandler.PlayActionAnimation();
                break;

            case ActionType.ChairClimb:

                _unit.UnitActionState = UnitActionState.ClimbingChair;

                _unit.UnitProperties.Root = _unit.Chair.Root;

                break;

            case ActionType.LedgeClimb:

                if (_unit.Table)
                {
                    _unit.Table.ExitTableAction(true);
                }

                _unit.Ledge.Unit = _unit;
                _unit.Ledge.ResetUI();

                _unit.UnitActionState = UnitActionState.ClimbingWall;

                _unit.UnitProperties.Root = _unit.Ledge.Root;

                _unit.Ledge.LedgeActionHandler.PlayActionAnimation();

                break;

            case ActionType.TableClimb:

                _unit.UnitActionState = UnitActionState.ClimbingTable;
                _unit.UnitActionInMind = UnitActionInMind.ClimbTable;

                _unit.UnitProperties.Root = _unit.Table.TableProperties.StaticRoot;

                _unit.Table.TableActionHandler.PlayActionAnimation(_unit);

                break;

            case ActionType.GrabTable:

                _unit.PlayerActionInMind = PlayerActionInMind.MovingTable;
                _unit.UnitActionState = UnitActionState.MovingTable;
                _unit.UnitActionInMind = UnitActionInMind.MovingTable;

                _unit.UnitProperties.Root = _unit.Table.TableProperties.StaticRoot;

                _unit.Table.TableActionHandler.PlayActionAnimation(_unit);
                break;

            case ActionType.PickupObject:

                // Play animation of picking up.
                //-

                var isRoom = _unit.UnitInventory.FindSpaceInInventory(_unit.Item, out _unit.Item);

                if (isRoom)
                {
                    //Debug.Log("Pickup Object");

                    _unit.Item = Items.CreateInventoryObject2D(_unit.Item);
                    _unit.UnitInventory.PlaceInSpace();
                }
                else
                {
                    // open Inventory and try to switch places with an item.
                    _unit.UnitInventory.InventoryObjectInHand = null;
                    //Debug.Log("NoSpace");
                }
                this.ExitCurentAction();
                break;

            case ActionType.OpenCloseDoor:

                _unit.PlayerActionInMind = PlayerActionInMind.OpenCloseDoor;
                _unit.UnitActionState = UnitActionState.OpenCloseDoor;
                _unit.UnitActionInMind = UnitActionInMind.OpenCloseDoor;

                _unit.UnitProperties.Root = _unit.Door.GetRoot();

                _unit.Door.DoorActionHandler.PlayActionAnimation(_unit);
                break;

            default:
                break;
        }
    }

    public void ExitCurentAction(bool toAnotherAction = false)
    {
        _unit.UnitPrimaryState = UnitPrimaryState.Idle;
        _unit.UnitActionState = UnitActionState.None;
        _unit.UnitActionInMind = UnitActionInMind.None;

        _unit.UnitController.ExitAction(toAnotherAction);

        switch (CurentActionType)
        {
            case ActionType.Ladder:

                _unit.Ladder.LadderActionHandler.LadderPath = null;
                _unit.Ladder = null;
                break;

            case ActionType.ChairClimb:

                _unit.Chair = null;
                break;

            case ActionType.LedgeClimb:

                _unit.Ledge.ResetLedgeAction();
                _unit.Ledge = null;
                break;

            case ActionType.TableClimb:

                _unit.Table.TableState = TableState.Static;
                _unit.Table.TableStaticAnimator.transform.localPosition = Vector3.zero;
                _unit.Table = null;
                break;

            case ActionType.GrabTable:

                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                _unit.PlayerActionInMind = PlayerActionInMind.Moving;

                _unit.Table.TableState = TableState.Static;

                _unit.Table = null;

                break;

            case ActionType.PickupObject:

                if (_unit.PlayerActionInMind != PlayerActionInMind.LookInInventory)
                    _unit.PlayerActionInMind = PlayerActionInMind.Moving;
                if (_unit.Item.ObjectState == ObjectState.InInventory)
                    Destroy(_unit.Item.InteractiveObject.gameObject);
                _unit.Item = null;

                break;

            case ActionType.OpenCloseDoor:

                _unit.PlayerActionInMind = PlayerActionInMind.Moving;
                _unit.Door = null;

                break;

            default:
                break;
        }

        CurentActionType = ActionType.None;
    }

    public void ExitSpecificAction(ActionType action)
    {
        _unit.UnitController.ExitAction();

        switch (action)
        {
            case ActionType.Ladder:

                _unit.Ladder.LadderActionHandler.LadderPath = null;
                _unit.Ladder = null;
                break;

            case ActionType.ChairClimb:

                _unit.Chair = null;
                break;

            case ActionType.LedgeClimb:

                _unit.Ledge.ResetLedgeAction();
                _unit.Ledge = null;
                break;

            case ActionType.TableClimb:

                _unit.Table.TableState = TableState.Static;
                _unit.Table.TableStaticAnimator.transform.localPosition = Vector3.zero;
                _unit.Table = null;
                break;

            case ActionType.GrabTable:

                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                _unit.PlayerActionInMind = PlayerActionInMind.Moving;

                _unit.Table.TableState = TableState.Static;

                _unit.Table = null;

                break;

            default:
                break;
        }
    }
}
