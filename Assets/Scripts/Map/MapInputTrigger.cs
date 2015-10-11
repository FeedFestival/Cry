using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class MapInputTrigger : MonoBehaviour
{
    public bool debug = false;

    void OnMouseEnter()
    {
        if (GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Busy)
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

                }
            }
            else
            {
                // This is canceling the actions.
                GlobalData.Player.UnitActionInMind = UnitActionInMind.None;

                var Target = GlobalData.Player.UnitProperties.thisUnitTarget.thisTransform;

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (debug)
                        Debug.Log("Ray launched");

                    Target.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                    if (debug)
                        Debug.Log("target_move to this pos : " + Target.position);

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
