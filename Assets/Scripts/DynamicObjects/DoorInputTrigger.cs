using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class DoorInputTrigger : MonoBehaviour
{
    private Door Door;

    public void Initialize(Door door)
    {
        Door = door;
    }

    void OnMouseEnter()
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.LookInInventory)
        {
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.None);
            Door.CircleAction.GoAvailable();
        }
    }

    void OnMouseOver()
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.LookInInventory)
        {
            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                Door.DoorActionHandler.CalculateStartPoint();
                GlobalData.Player.UnitActionHandler.SetAction(Door.gameObject, ActionType.OpenCloseDoor);
            }
        }
    }

    void OnMouseExit()
    {
        GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
        Door.CircleAction.GoUnavailable();
    }
}
