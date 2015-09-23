using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderInputTrigger : MonoBehaviour
{
    private LadderTriggerInput TriggerInput;

    private LadderActionHandler LadderActionHandler;

    // Use this for initialization
    public void Initialize(LadderTriggerInput triggerInput, LadderActionHandler ladderActionHandler)
    {
        LadderActionHandler = ladderActionHandler;
        TriggerInput = triggerInput;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            LadderActionHandler.SetAction(TriggerInput);
        }
        LadderActionHandler.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Up);
    }
    void OnMouseExit()
    {
        LadderActionHandler.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
