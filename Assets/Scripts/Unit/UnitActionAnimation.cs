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

        Play(Unit.UnitProperties.ArmatureName + action.LadderAnimation.ToString());
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
            PlayAnimation(new LadderPath {Played = false, LadderAnimation = LadderAnimations.Idle_Ladder });    // OVERKILL
    }
}
