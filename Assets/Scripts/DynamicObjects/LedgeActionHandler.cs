using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LedgeActionHandler : MonoBehaviour
{
    public bool debuging = false;

    private Ledge Ledge;

    private Unit Unit;

    // Use this for initialization
    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
    }

    public void PlayActionAnimation(Unit unit)
    {
        Unit = unit;
        switch (Ledge.LedgeStartPoint)
        {
            case LedgeStartPoint.Bottom:

                Unit.UnitActionAnimation.PlayAnimation(WallClimb_Animations.WallClimb_2Metters.ToString());
                Play(WallClimb_Animations.WallClimb_2Metters.ToString());
                break;

            case LedgeStartPoint.Top:

                Unit.UnitActionAnimation.PlayAnimation(WallClimb_Animations.WallClimbDown_2Metters.ToString());
                Play(WallClimb_Animations.WallClimbDown_2Metters.ToString());
                break;

            default:
                break;
        }
    }

    private void Play(string animationString)
    {
        Ledge.Ledge_Animator.CrossFade(animationString);

        float animationLenght = Unit.UnitAnimator[animationString].length;
        if (debuging)
            Debug.Log("an - " + animationString + " , langht = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);
        Unit.UnitActionHandler.ExitCurentAction();
    }
}
