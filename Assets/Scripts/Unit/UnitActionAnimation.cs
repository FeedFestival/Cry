using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitActionAnimation : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    private Unit Unit;

    [HideInInspector]
    public string CurrentAnimation;

    [HideInInspector]
    public LadderPath CurrentAction;

    public void Initialize(Unit unit)
    {
        Unit = unit;

        SetupAnimations();
    }

    public void PlayAnimation(LadderPath action)
    {
        CurrentAction = action;

        Play(Unit.UnitProperties.ArmatureName + action.LadderAnimation.ToString());
    }

    public void PlaySingleAnimation(string animationString)
    {
        Play(Unit.UnitProperties.ArmatureName + animationString);
    }

    public void PlayLoopAction(string animationString)
    {
        Unit.UnitAnimator[animationString].wrapMode = WrapMode.Loop;
        Unit.UnitAnimator.CrossFade(animationString);
    }

    private void Play(string animationString)
    {
        Unit.UnitAnimator.CrossFade(animationString);

        float animationLenght = Unit.UnitAnimator[animationString].length;
        if (debuging)
            Debug.Log("an - " + animationString + " , langht = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);

        switch (Unit.UnitActionHandler.curentActionType)
        {
            case ActionType.Ladder:

                if (CurrentAction.ExitAction)
                    Unit.UnitBasicAnimation.PlayIdle();
                if (CurrentAction.IsLastAction)
                    PlayAnimation(new LadderPath { Played = false, LadderAnimation = LadderAnimations.Idle_Ladder });    // OVERKILL

                break;

            case ActionType.ChairClimb:
                break;
            case ActionType.ChairGrab:
                break;
            case ActionType.LedgeClimb:

                Unit.UnitBasicAnimation.PlayIdle();

                break;

            default:
                break;
        }
    }

    void SetupAnimations()
    {
        Unit.UnitAnimator[WallClimb_Animations.WallClimb_2Metters.ToString()].wrapMode = WrapMode.PingPong;
        Unit.UnitAnimator[WallClimb_Animations.WallClimbDown_2Metters.ToString()].wrapMode = WrapMode.PingPong;

        Unit.UnitAnimator[LadderAnimations.Idle_Ladder.ToString()].wrapMode = WrapMode.Loop;

        Unit.UnitAnimator[LadderAnimations.GetOn_From_Bottom.ToString()].wrapMode = WrapMode.PingPong;
        Unit.UnitAnimator[LadderAnimations.Climb_From_Level1_To_Level2.ToString()].wrapMode = WrapMode.PingPong;
        Unit.UnitAnimator[LadderAnimations.Climb_Exit_To_Level2_Top.ToString()].wrapMode = WrapMode.PingPong;

        Unit.UnitAnimator[LadderAnimations.GetOn_From_Level2_Top.ToString()].wrapMode = WrapMode.PingPong;
        Unit.UnitAnimator[LadderAnimations.Jump_Exit_To_Bottom.ToString()].wrapMode = WrapMode.PingPong;

        //Player.UnitAnimator[LadderAnimations.ClimbDown_From_Level1_To_Bottom.ToString()].wrapMode = WrapMode.PingPong;
        Unit.UnitAnimator[LadderAnimations.ClimbDown_Exit_To_Bottom.ToString()].wrapMode = WrapMode.PingPong;
    }
}
