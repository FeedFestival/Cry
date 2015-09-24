using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Types;

public class UnitLadderAction : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    Unit Unit;

    [HideInInspector]
    public LadderStartPoint LadderStartPoint;

    [HideInInspector]
    public List<LadderPath> LadderPath;

    public void Initialize(Unit unit)
    {
        Unit = unit;
    }

    public void SetPathToStartPoint(LadderStartPoint ladderStartPoint)
    {
        this.Unit.UnitActionInMind = UnitActionInMind.ClimbingLadder;

        LadderStartPoint = ladderStartPoint;
        switch (ladderStartPoint)
        {
            case LadderStartPoint.Bottom:

                this.Unit.UnitController.SetPathToTarget(Unit.LadderStats.StartPoint_Bottom.position);
                break;

            case LadderStartPoint.Level2_Top:

                this.Unit.UnitController.SetPathToTarget(Unit.LadderStats.StartPoint_Level2_Top.position);
                break;

            default: break;
        }
    }

    public void CalculateLadderPath(LadderTriggerInput ladderTriggerInput, bool isUnitOnLadder = false)
    {
        if (!isUnitOnLadder)
        {
            switch (LadderStartPoint)
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
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[1] { 3 });
                            break;
                        case LadderTriggerInput.Level1:
                            LadderPath = CreateLadderPath(new int[1] { 3 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[3] { 3, 4, 5 });
                            break;
                        default:
                            break;
                    }
                    break;
                default:break;
            }
        }
        else
        {
            switch (Unit.LadderStats.LadderAnimation.CurrentAction.LadderAnimation)
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
                    
                case LadderAnimations.Climb_From_Bottom_To_Level1:
                    switch (ladderTriggerInput)
                    {
                        case LadderTriggerInput.Level2_Top:
                            LadderPath = CreateLadderPath(new int[1] { 2 });
                            break;
                        case LadderTriggerInput.Bottom:
                            LadderPath = CreateLadderPath(new int[2] { 4, 5 });
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
                default:break;
            }

            if (Unit.UnitActionAnimation.CurrentAction.LadderAnimation == LadderAnimations.Idle)
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
                    Unit.LadderStats.LadderAnimation.PlayAnimation(ladderAction);

                    return;
                }
            }
        }
    }
}
