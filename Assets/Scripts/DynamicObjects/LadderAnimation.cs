using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderAnimation : MonoBehaviour {

    public bool debug = false;

    private LadderStats LadderStats;

    [HideInInspector]
    public LadderPath CurrentAction;

    public void Initialize(LadderStats ladderStats)
    {
        LadderStats = ladderStats;
    }

    // After the user click somewhere this is gonna get called to play the pivot animation.
    public void PlayAnimation(LadderPath action)
    {
        CurrentAction = action;

        var txtObject = "Ladder|";

        if (action.LadderAnimation == LadderAnimations.Climb_Exit_To_Level2_Top)
            action.ExitAction = true;
        else if (action.LadderAnimation == LadderAnimations.ClimbDown_Exit_To_Bottom)
            action.ExitAction = true;
        else if (action.LadderAnimation == LadderAnimations.Jump_Exit_To_Bottom)
            action.ExitAction = true;

        Play(txtObject + action.LadderAnimation);
    }

    private void Play(string animationString)
    {
        LadderStats.LadderAnimator.CrossFade(animationString);

        float animationLenght = LadderStats.LadderAnimator[animationString].length;

        if (debug)
            Debug.Log("an (Unit) - " + animationString + " , length = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }
    IEnumerator WaitForEndOfAnimation(float animTime, bool endEvent = true)
    {
        yield return new WaitForSeconds(animTime);


        if (CurrentAction.ExitAction)
            LadderStats.SceneManager.PlayerStats.UnitActionHandler.ExitCurentAction();
        else 
            // Play Next Animation in the List
            LadderStats.SceneManager.PlayerStats.UnitLadderAction.PlayActionAnimation();
    }
}