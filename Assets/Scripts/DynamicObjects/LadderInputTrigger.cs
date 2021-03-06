﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class LadderInputTrigger : MonoBehaviour
{
    private LadderTriggerInput TriggerInput;

    private Ladder Ladder;

    // Use this for initialization
    public void Initialize(LadderTriggerInput triggerInput, Ladder ladder)
    {
        this.Ladder = ladder;
        this.TriggerInput = triggerInput;
    }

    void OnMouseEnter()
    {
        this.Ladder.LadderActionHandler.CalculateLadderCursor();
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            this.Ladder.LadderActionHandler.SetAction(this.TriggerInput);
        }
    }
    void OnMouseExit()
    {
        GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }
}
