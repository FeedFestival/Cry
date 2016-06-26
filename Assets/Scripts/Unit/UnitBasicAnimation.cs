using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitBasicAnimation : MonoBehaviour
{
    private Unit _unit;

    // Use this for initialization
    public void Initialize(Unit unit)
    {
        _unit = unit;

        SetupAnimations();
    }

    public void Play(UnitPrimaryState unitPrimaryState, bool forcePlay = false)
    {
        if (_unit.UnitAnimator)
        {
            if (forcePlay)
                _unit.UnitAnimator.Play(_unit.UnitProperties.ArmatureName + unitPrimaryState);
            else
                _unit.UnitAnimator.CrossFade(_unit.UnitProperties.ArmatureName + unitPrimaryState);
            return;
        }
        Debug.LogError("Can't play animation["+ unitPrimaryState + "] for unit[" + _unit.gameObject.name + "], error: There is no Model(UnitAnimator) for this Unit. [" + _unit.UnitAnimator + "]");
    }

    void SetupAnimations()
    {
        if (_unit.UnitAnimator)
        {
            _unit.UnitAnimator[UnitPrimaryState.Idle.ToString()].wrapMode = WrapMode.Loop;
            _unit.UnitAnimator[UnitPrimaryState.Walk.ToString()].wrapMode = WrapMode.Loop;
            return;
        }
        Debug.LogError("Can't setup animations for unit[" + _unit.gameObject.name + "]. error: There is no Model(UnitAnimator) for this Unit. [" + _unit.UnitAnimator + "]");
    }
}