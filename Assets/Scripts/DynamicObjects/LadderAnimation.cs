using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class LadderAnimation : MonoBehaviour {

    public bool debug = false;

    private Ladder Ladder;

    [HideInInspector]
    public LadderPath CurrentAction;

    public void Initialize(Ladder ladder)
    {
        Ladder = ladder;

        SetupAnimations();
    }

    // After the user click somewhere this is gonna get called to play the pivot animation.
    public void PlayAnimation(LadderPath action)
    {
        CurrentAction = action;

        if (action.LadderAnimation == LadderAnimations.Climb_Exit_To_Level2_Top)
            action.ExitAction = true;
        else if (action.LadderAnimation == LadderAnimations.GetOn_From_Level2_Top)
            Ladder.LadderAnimator[LadderAnimations.GetOn_From_Level2_Top.ToString()].speed = 1f;
        else if (action.LadderAnimation == LadderAnimations.ClimbDown_Exit_To_Bottom)
            action.ExitAction = true;
        else if (action.LadderAnimation == LadderAnimations.Jump_Exit_To_Bottom)
            action.ExitAction = true;

        Play(action.LadderAnimation.ToString());
    }

    private void Play(string animationString)
    {
        Ladder.LadderAnimator.CrossFade(animationString);

        float animationLenght = Ladder.LadderAnimator[animationString].length;

        if (debug)
            Debug.Log("an (Player) - " + animationString + " , length = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime, bool endEvent = true)
    {
        yield return new WaitForSeconds(animTime);

        if (CurrentAction.IsLastAction)
            Ladder.LadderAnimator.Stop();

        if (CurrentAction.ExitAction)
        {
            Ladder.LadderAnimator.Stop();
            GlobalData.Player.UnitActionHandler.ExitCurentAction();
        }
        else
        {
            // Play Next Animation in the List
            GlobalData.Player.Ladder.LadderActionHandler.PlayActionAnimation();
        }
    }

    void SetupAnimations()
    {
        Ladder.LadderAnimator[LadderAnimations.GetOn_From_Bottom.ToString()].wrapMode = WrapMode.PingPong;
        Ladder.LadderAnimator[LadderAnimations.Climb_From_Level1_To_Level2.ToString()].wrapMode = WrapMode.PingPong;
        Ladder.LadderAnimator[LadderAnimations.Climb_Exit_To_Level2_Top.ToString()].wrapMode = WrapMode.PingPong;

        Ladder.LadderAnimator[LadderAnimations.GetOn_From_Level2_Top.ToString()].wrapMode = WrapMode.PingPong;
        Ladder.LadderAnimator[LadderAnimations.Jump_Exit_To_Bottom.ToString()].wrapMode = WrapMode.PingPong;

        Ladder.LadderAnimator[LadderAnimations.ClimbDown_Exit_To_Bottom.ToString()].wrapMode = WrapMode.PingPong;
    }
}