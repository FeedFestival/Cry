using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class MapInputTrigger : MonoBehaviour
{
    public bool debug = false;

    void OnMouseEnter()
    {
        GlobalData.Player.isMouseOverMap = true;
        if (GlobalData.Player != null && GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Busy)
        {
            if (GlobalData.Player.UnitActionState == UnitActionState.ClimbingLadder)
            {
                GlobalData.Player.Ladder.LadderActionHandler.CalculateLadderCursor();
            }
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
        }
    }
    void OnMouseOver()
    {
        if (GlobalData.Player != null && GlobalData.Player.UnitActionState == UnitActionState.MovingTable)
        {
            var pos = Logic.GetPointHitAtMousePosition();
            if (pos != Vector3.zero)
            {
                if (GlobalData.Player.Table.TableController)
                    GlobalData.Player.Table.TableController.CalculateAction(pos);
            }
        }
    }

    public void DoAction()  // lack of a better name;
    {
        if (GlobalData.Player.UnitActionState == UnitActionState.MovingItemInInventory)
        {
            GlobalData.Player.UnitActionState = UnitActionState.None;
        }
        else
        {
            if (GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (GlobalData.Player.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    if (GlobalData.CameraControl.CameraCursor.lastCursor == CursorType.Ladder_Up)
                    {
                        GlobalData.Player.Ladder.LadderActionHandler.SetAction(LadderTriggerInput.Level2_Top);
                    }
                    else
                    {
                        GlobalData.Player.Ladder.LadderActionHandler.SetAction(LadderTriggerInput.Bottom);
                    }
                }
                else if (GlobalData.Player.UnitActionState == UnitActionState.MovingTable)
                {
                    var pos = Logic.GetPointHitAtMousePosition();
                    if (pos != Vector3.zero)
                    {
                        GlobalData.Player.UnitProperties.thisUnitTarget.thisTransform.position = GlobalData.Player.Table.TableController.MoveTable();
                        GlobalData.Player.ActivateTarget(true);
                    }
                }
            }
            else
            {
                // This is canceling the actions.
                GlobalData.Player.UnitActionInMind = UnitActionInMind.None;

                var pos = Logic.GetPointHitAtMousePosition();
                if (pos != Vector3.zero)
                {
                    GlobalData.Player.UnitProperties.thisUnitTarget.thisTransform.position = pos;
                    GlobalData.Player.UnitController.GoToTarget();
                }
            }
            if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
            {
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                GlobalData.Player.PlayerActionInMind = PlayerActionInMind.Moving;
            }
        }
    }

    void OnMouseExit()
    {
        GlobalData.Player.isMouseOverMap = false;
        GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
