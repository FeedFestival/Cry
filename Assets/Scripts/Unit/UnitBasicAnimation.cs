using UnityEngine;
using System.Collections;

public class UnitBasicAnimation : MonoBehaviour
{
    UnitStats UnitStats;

    private bool goIdle;
    private bool goWalk;

    private bool isDoingAction;

    #region BasicAnimations
    private string Walk;
    private string Idle;
    #endregion

    // Use this for initialization
    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;

        Idle = "Stealth_Idle";
        UnitStats.UnitAnimator[Idle].wrapMode = WrapMode.Loop;

        Walk = "Stealth_Walk";
        UnitStats.UnitAnimator[Walk].wrapMode = WrapMode.Loop;

        GoIdle();
    }

    public void GoWalk()
    {
        if (!isDoingAction)
        {
            goWalk = true;
            goIdle = false;

            UnitStats.UnitAnimator.CrossFade(Walk);
        }
    }
    public void GoIdle()
    {
        if (!isDoingAction && goWalk)
        {
            goIdle = true;
            goWalk = false;

            UnitStats.UnitAnimator.CrossFade(Idle);
        }
    }

    public void SetIsDoingAction(bool value)
    {
        isDoingAction = value;
    }
    public bool GetIsDoingAction()
    {
        return isDoingAction;
    }
}
