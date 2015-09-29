using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class MapInputTrigger : MonoBehaviour
{
    public bool debug = true;

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
            else
            {
                var Target = SceneManager.PlayerStats.UnitProperties.thisUnitTarget.thisTransform;

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (debug)
                        Debug.Log("Ray launched");

                    Target.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                    if (debug)
                        Debug.Log("target_move to this pos : " + Target.position);

                    SceneManager.PlayerStats.UnitController.GoToTarget();
                }

            }
        }
    }
    void OnMouseExit()
    {
        SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
