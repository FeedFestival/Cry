using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitActionAnimation : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    private Unit Unit;

    [HideInInspector]
    public LadderPath CurrentAction;

    public void Initialize(Unit unit)
    {
        Unit = unit;
    }

    public void PlayAnimation(LadderPath action)
    {
        CurrentAction = action;
        switch (action.LadderAnimation)
        {
            case LadderAnimations.GetOn_From_Bottom:

                Play(Unit.UnitProperties.Ladder_Get_On);
                break;

            case LadderAnimations.Climb_From_Bottom_To_Level1:

                Play(Unit.UnitProperties.Ladder_Climb);
                break;

            case LadderAnimations.Climb_Exit_To_Level2_Top:

                Play(Unit.UnitProperties.Ladder_Get_Up);
                break;

            case LadderAnimations.GetOn_From_Level2_Top:

                Play(Unit.UnitProperties.Ladder_Get_On_From_Up);
                break;

            case LadderAnimations.ClimbDown_From_Level1_To_Bottom:

                Play(Unit.UnitProperties.Ladder_Climb_Down);
                break;

            case LadderAnimations.ClimbDown_Exit_To_Bottom:

                Play(Unit.UnitProperties.Ladder_Get_Down);
                break;

            case LadderAnimations.ClimbDown_Exit_To_Bottom_Fast:

                Play(Unit.UnitProperties.Ladder_Get_Down_Fast);
                break;

            default:
                Unit.UnitAnimator.CrossFade(Unit.UnitProperties.Ladder_Idle);
                break;
        }
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

        if (CurrentAction.IsLastAction)
            PlayAnimation(new LadderPath {Played = false, LadderAnimation = LadderAnimations.Idle });    // OVERKILL
    }
}
