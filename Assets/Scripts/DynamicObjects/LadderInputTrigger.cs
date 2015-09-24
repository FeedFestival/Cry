using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderInputTrigger : MonoBehaviour
{
    private LadderTriggerInput TriggerInput;

    private LadderStats LadderStats;

    // Use this for initialization
    public void Initialize(LadderTriggerInput triggerInput, LadderStats ladderStats)
    {
        this.LadderStats = ladderStats;
        this.TriggerInput = triggerInput;
    }

    void OnMouseEnter()
    {
        this.LadderStats.LadderActionHandler.CalculateLadderCursor();
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            this.LadderStats.LadderActionHandler.SetAction(this.TriggerInput);
        }
    }
    void OnMouseExit()
    {
        this.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
