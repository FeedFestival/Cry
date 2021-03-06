﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class LedgeAnimation : MonoBehaviour
{
    public bool debuging = false;

    private Ledge Ledge;

    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
    }

    public void SetStartupAnimation()
    {
        Ledge.Ledge_Animator.Play(WallClimb_Animations.WallClimbDown_2Metters.ToString());
        Ledge.Ledge_Animator[WallClimb_Animations.WallClimbDown_2Metters.ToString()].speed = 0;
    }

    public void Play(string animationString)
    {
        Ledge.Ledge_Animator.CrossFade(animationString);

        float animationLenght = Ledge.Unit.UnitAnimator[animationString].length;
        if (debuging)
            Debug.Log("an - " + animationString + " , langht = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);

        if (Ledge.LedgeBottomPoint == LedgeBottomPoint.Table && Ledge.LedgeStartPoint == LedgeStartPoint.Top)
        {
            Ledge.Table.Unit = Ledge.Unit;
            Ledge.Unit.UnitActionHandler.ExitCurentAction(true);
            Ledge.Table.SetUnitOnTable();
        }
        else
        {
            Ledge.Unit.UnitActionHandler.ExitCurentAction();
        }
    }
}
