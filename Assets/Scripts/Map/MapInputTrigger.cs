using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class MapInputTrigger : MonoBehaviour
{
    public bool debug = false;

    void Start()
    {
        GlobalData.SceneManager.Map = this;
    }

    void OnMouseEnter()
    {
        GlobalData.Player.IsMouseOverMap = true;
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
            if (pos != Vector3.zero && GlobalData.Player.Table.TableController)
            {
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
                    SetUnitTargetToPos(gameObject.GetComponent<Collider>());
                }
            }
            else
            {
                // This is canceling the actions.
                GlobalData.Player.UnitActionInMind = UnitActionInMind.None;

                SetUnitTargetToPos(gameObject.GetComponent<Collider>());
            }
            if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
            {
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
                GlobalData.Player.PlayerActionInMind = PlayerActionInMind.Moving;
            }
        }
    }

    private void SetUnitTargetToPos(Collider collider, bool forTable = false)
    {
        var pos = Logic.GetPointHitAtMousePosition(collider);
        //Debug.Log(collider.bounds.Contains(pos));
        if (pos != Vector3.zero)
        {
            //Debug.Log("point is inside collider");
            if (forTable)
            {
                GlobalData.Player.UnitProperties.ThisUnitTarget.thisTransform.position = GlobalData.Player.Table.TableController.MoveTable();
                GlobalData.Player.ActivateTarget(true);
            }
            else
            {
                GlobalData.Player.UnitProperties.ThisUnitTarget.thisTransform.position = pos;
                GlobalData.Player.UnitController.GoToTarget();
            }
        }
        else
        {
            Debug.LogError("point doesnt exists on the WALKABLE map");
        }
    }

    void OnMouseExit()
    {
        GlobalData.Player.IsMouseOverMap = false;
        GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
