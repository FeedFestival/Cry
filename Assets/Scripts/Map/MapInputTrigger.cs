using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class MapInputTrigger : MonoBehaviour
{
    public bool debug = false;

    void OnMouseEnter()
    {
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

        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
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
        GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
