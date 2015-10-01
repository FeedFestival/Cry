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

                Unit.UnitActionAnimation.PlaySingleAnimation(WallClimb_Animations.WallClimb_2Metters.ToString());
                Play(WallClimb_Animations.WallClimb_2Metters.ToString());
                break;

            case LedgeStartPoint.Top:

                Unit.UnitActionAnimation.PlaySingleAnimation(WallClimb_Animations.WallClimbDown_2Metters.ToString());
                Ledge.Ledge_Animator[WallClimb_Animations.WallClimbDown_2Metters.ToString()].speed = 1; // CUSTOM
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
