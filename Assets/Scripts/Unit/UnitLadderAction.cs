using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Types;

public class UnitLadderAction : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    UnitStats UnitStats;

    [HideInInspector]
    public LadderStartPoint LadderStartPoint;

    [HideInInspector]
    public List<LadderPath> LadderPath;

    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    public void SetPathToStartPoint(LadderStartPoint ladderStartPoint)
    {
        LadderStartPoint = ladderStartPoint;
        switch (ladderStartPoint)
        {
            case LadderStartPoint.Bottom:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.LadderStats.StartPoint_Bottom.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                break;

            case LadderStartPoint.Level2_Top:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.LadderStats.StartPoint_Level2_Top.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                break;

            default: break;
        }
    }

    public void CalculateLadderPath(LadderTriggerInput ladderTriggerInput)
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

                        LadderPath = CreateLadderPath(new int[2] { 0 , 1 });
                        break;

                    case LadderTriggerInput.Bottom:

                        LadderPath = CreateLadderPath(new int[1] { 0 });
                        break;

                    //case LadderTriggerInput.Level2:

                    //    LadderPath = CreateLadderPath(new int[2] { 0, 1 });
                    //    break;

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

                    //case LadderTriggerInput.Level2:

                    //    LadderPath = CreateLadderPath(new int[1] { 3 });
                    //    break;

                    default:
                        break;
                }
                break;

            default:
                break;
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
        foreach (LadderPath ladderAction in LadderPath)
        {
            if (ladderAction.Played == false)
            {
                ladderAction.Played = true;
                //  (down)  This also sets the Unit to follow the pivot of the ladder to be guided throut the animation
                UnitStats.Root = UnitStats.LadderStats.Root;

                if (debuging)
                    Debug.Log(" - action : " + ladderAction.LadderAnimation + " ; last action : " +  ladderAction.IsLastAction);

                UnitStats.UnitActionAnimation.PlayAnimation(ladderAction.LadderAnimation, ladderAction.IsLastAction);
                UnitStats.LadderStats.LadderAnimation.PlayAnimation(ladderAction.LadderAnimation);

                return;
            }    
        }
    }
}
