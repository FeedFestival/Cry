using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using System.Collections.Generic;

public class LadderActionHandler : MonoBehaviour
{
    public bool debuging;

    private Ladder Ladder;

    private Unit Unit;

    private GameObject LadderGameObject;

    [HideInInspector]
    public List<LadderPath> LadderPath;

    private ActionType LadderActionType;

    // Use this for initialization
    public void Initialize(Ladder ladder, GameObject ladderGameObject)
    {
        this.Ladder = ladder;

        this.LadderGameObject = ladderGameObject;

        this.Unit = GlobalData.Player;  // HARD_CODED

        LadderActionType = ActionType.Ladder;
    }

    public void SetAction(LadderTriggerInput triggerInput)
    {
        Ladder.LadderTriggerInput = triggerInput;
        switch (triggerInput)
        {
            case LadderTriggerInput.Bottom:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType);
                break;

            case LadderTriggerInput.Level1:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType);
                break;
            case LadderTriggerInput.Level2_Top:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType);
                break;

            case LadderTriggerInput.Level2:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType);
                break;

            default:
                break;
        }
    }

    public void CalculateStartPoint()
    {
        var playerPos = this.Unit.UnitProperties.thisTransform.position;

        float[] distancesLadder = new float[2];
        distancesLadder[(int)LadderStartPoint.Bottom] = Vector3.Distance(playerPos, this.Ladder.StartPoint_Bottom.position);
        distancesLadder[(int)LadderStartPoint.Level2_Top] = Vector3.Distance(playerPos, this.Ladder.StartPoint_Level2_Top.position);

        Ladder.LadderStartPoint = (LadderStartPoint)Logic.GetSmallestDistance(distancesLadder);
    }

    public void CalculateLadderCursor()
    {
        if (this.Unit.UnitPrimaryState != UnitPrimaryState.Busy)
        {
            CalculateStartPoint();
            if (Ladder.LadderStartPoint == LadderStartPoint.Bottom)
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Up);
            else
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Down);

            return;
        }
        else if (this.Unit.UnitActionState == UnitActionState.ClimbingLadder)
        {
            RaycastHit CircleHit;
            Ray CircleRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(CircleRay, out CircleHit, 100))
            {
                var unitPosition = Mathf.RoundToInt(this.Unit.UnitProperties.thisTransform.position.y);
                var AimCircle_YPosition = Mathf.RoundToInt(CircleHit.point.y);

                if (AimCircle_YPosition < unitPosition)
                {
                    GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Down);
                }
                else
                {
                    GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Up);
                }
            }
        }
    }

    public void CalculateLadderPath(LadderTriggerInput ladderTriggerInput, bool isUnitOnLadder = false)
    {
        if (!isUnitOnLadder)
        {
            switch (Ladder.LadderStartPoint)
            {
                case LadderStartPoint.Bottom:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[3] { 0, 1, 2 });
                            break;
                        case LadderTriggerInput.Level1:
                            LadderPath = CreateLadderPath(new int[2] { 0, 1 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[1] { 0 });
                            break;
                        default:
                            break;
                    }
                    break;

                case LadderStartPoint.Level2_Top:

                    Ladder.LadderAnimator.Play(LadderAnimations.GetOn_From_Level2_Top.ToString());
                    Ladder.LadderAnimator[LadderAnimations.GetOn_From_Level2_Top.ToString()].speed = 0f;

                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[1] { 3 });
                            break;
                        case LadderTriggerInput.Level1:
                            LadderPath = CreateLadderPath(new int[1] { 3 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[2] { 3, 11 });
                            break;
                        default:
                            break;
                    }
                    break;
                default: break;
            }
        }
        else
        {
            switch (Unit.Ladder.LadderAnimation.CurrentAction.LadderAnimation)
            {
                case LadderAnimations.GetOn_From_Bottom:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[2] { 1, 2 });
                            break;
                        case LadderTriggerInput.Level1:
                            LadderPath = CreateLadderPath(new int[1] { 1 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[1] { 5 });
                            break;
                        default:
                            break;
                    }
                    break;
                case LadderAnimations.ClimbDown_From_Level1_To_Bottom:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[2] { 1, 2 });
                            break;
                        case LadderTriggerInput.Level1:
                            LadderPath = CreateLadderPath(new int[1] { 1 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[1] { 5 });
                            break;
                        default:
                            break;
                    }
                    break;

                case LadderAnimations.Climb_From_Level1_To_Level2:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[1] { 2 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[1] { 11 });
                            break;
                        default:
                            break;
                    }
                    break;
                case LadderAnimations.GetOn_From_Level2_Top:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[1] { 2 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[1] { 11 }); // Get_Down_Fast
                            break;
                        default:
                            break;
                    }
                    break;
                default: break;
            }

            if (Unit.UnitActionAnimation.CurrentAction.LadderAnimation == LadderAnimations.Idle_Ladder)
                PlayActionAnimation();
        }
    }

    public List<LadderPath> CreateLadderPath(int[] actions)
    {
        var actionList = new List<LadderPath>();

        if (actions.Length == 1)
        {
            var action = new LadderPath
            {
                Played = false,
                IsLastAction = true,
                LadderAnimation = (LadderAnimations)actions[0]
            };
            actionList.Add(action);
        }
        else
        {
            for (int i = 0; i < actions.Length; i++)
            {
                var action = new LadderPath
                {
                    Played = false,
                    IsLastAction = false,
                    LadderAnimation = (LadderAnimations)actions[i]
                };
                if (i == actions.Length - 1)
                    action.IsLastAction = true;

                actionList.Add(action);
            }
        }
        return actionList;
    }

    public void PlayActionAnimation()
    {
        if (LadderPath != null)
        {
            foreach (LadderPath ladderAction in LadderPath)
            {
                if (ladderAction.Played == false)
                {
                    ladderAction.Played = true;

                    if (debuging)
                        Debug.Log(" - action : " + ladderAction.LadderAnimation + " ; last action : " + ladderAction.IsLastAction);

                    Unit.UnitActionAnimation.PlayAnimation(ladderAction);
                    Unit.Ladder.LadderAnimation.PlayAnimation(ladderAction);

                    return;
                }
            }
        }
    }
}
