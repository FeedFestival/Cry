using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitActionAnimation : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    private UnitStats UnitStats;

    [HideInInspector]
    public bool ControllerFollowRoot;

    [HideInInspector]
    public bool IsLastAction;

    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;
    }

    public void PlayAnimation(LadderAnimations animation, bool isLastAction = false)
    {
        IsLastAction = isLastAction;
        switch (animation)
        {
            case LadderAnimations.GetOn_From_Bottom:

                Play(UnitStats.Ladder_Get_On);
                break;

            case LadderAnimations.Climb_From_Bottom_To_Level1:

                Play(UnitStats.Ladder_Climb);
                break;

            case LadderAnimations.Climb_Exit_To_Level2_Top:

                Play(UnitStats.Ladder_Get_Up);
                break;

            case LadderAnimations.GetOn_From_Level2_Top:

                Play(UnitStats.Ladder_Get_On_From_Up);
                break;

            case LadderAnimations.ClimbDown_From_Level1_To_Bottom:

                Play(UnitStats.Ladder_Climb_Down);
                break;

            case LadderAnimations.ClimbDown_Exit_To_Bottom:

                Play(UnitStats.Ladder_Get_Down);
                break;

            default:

                UnitStats.UnitAnimator.CrossFade(UnitStats.Ladder_Idle);
                break;
        }
    }

    private void Play(string animationString)
    {
        UnitStats.UnitAnimator.CrossFade(animationString);

        float animationLenght = UnitStats.UnitAnimator[animationString].length;
        if (debuging)
            Debug.Log("an - " + animationString + " , langht = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);

        if (IsLastAction)
            PlayAnimation(LadderAnimations.Idle);
    }

    void Update()
    {
        if (ControllerFollowRoot)
        {
            UnitStats.thisTransform.position = new Vector3(UnitStats.Root.position.x, UnitStats.Root.position.y + 1, UnitStats.Root.position.z);
            var rot = new Quaternion();
            rot.eulerAngles = new Vector3(UnitStats.Root.eulerAngles.x + 90, UnitStats.Root.eulerAngles.y - 90, UnitStats.Root.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(UnitStats.thisTransform.rotation, rot, Time.deltaTime * 5);
        }
    }
}
