using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitBasicAnimation : MonoBehaviour
{
    private UnitStats UnitStats;

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
    }

    public void GoWalk()
    {
        UnitStats.UnitAnimator.CrossFade(Walk);
    }
    public void GoIdle()
    {
        UnitStats.UnitAnimator.CrossFade(Idle);
    }
}
