using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitBasicAnimation : MonoBehaviour
{
    private Unit Unit;

    // Use this for initialization
    public void Initialize(Unit unit)
    {
        Unit = unit;

        SetupAnimations();
    }

    public void PlayIdle()
    {
        Unit.UnitAnimator.Play(Unit.UnitProperties.ArmatureName + UnitPrimaryState.Idle);
    }

    public void GoWalk()
    {
        Unit.UnitAnimator.CrossFade(Unit.UnitProperties.ArmatureName + UnitPrimaryState.Walk);
    }
    public void GoIdle()
    {
        Unit.UnitAnimator.CrossFade(Unit.UnitProperties.ArmatureName + UnitPrimaryState.Idle);
    }

    void SetupAnimations()
    {
        Unit.UnitAnimator[UnitPrimaryState.Idle.ToString()].wrapMode = WrapMode.Loop;
        Unit.UnitAnimator[UnitPrimaryState.Walk.ToString()].wrapMode = WrapMode.Loop;
    }
}
