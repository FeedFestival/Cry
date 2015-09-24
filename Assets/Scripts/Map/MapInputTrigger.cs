using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class MapInputTrigger : MonoBehaviour {

    private SceneManager SceneManager;

    public void Initialize(SceneManager sceneManager)
    {
        SceneManager = sceneManager;
    }

    void OnMouseEnter()
    {
        if (SceneManager)
        {
            if (SceneManager.PlayerStats.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (SceneManager.PlayerStats.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    SceneManager.PlayerStats.LadderStats.LadderActionHandler.CalculateLadderCursor();
                }
            }
            else
            {
                SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
            }
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            if (SceneManager.PlayerStats.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (SceneManager.PlayerStats.UnitActionState == UnitActionState.ClimbingLadder)
                {
                    if (SceneManager.CameraControl.CameraCursor.lastCursor == CursorType.Ladder_Up)
                    {
                        SceneManager.PlayerStats.LadderStats.LadderActionHandler.SetAction(LadderTriggerInput.Level2_Top);
                    }
                    else
                    {
                        SceneManager.PlayerStats.LadderStats.LadderActionHandler.SetAction(LadderTriggerInput.Bottom);
                    }
                }
            }
        }
    }
    void OnMouseExit()
    {
        SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
