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
        switch (action.LadderAnimation)
        {
            case LadderAnimations.GetOn_From_Bottom:
                
                Play(LadderStats.GetOn_From_Bottom);
                break;

            case LadderAnimations.Climb_From_Bottom_To_Level1:

                Play(LadderStats.Climb_From_Bottom_To_Level1);
                break;

            case LadderAnimations.Climb_Exit_To_Level2_Top:

                action.ExitAction = true;
                Play(LadderStats.Climb_Exit_To_Level2_Top);
                break;

            //------------------------------------------------------
            case LadderAnimations.GetOn_From_Level2_Top:

                Play(LadderStats.GetOn_From_Level2_Top);
                break;

            case LadderAnimations.ClimbDown_From_Level1_To_Bottom:

                Play(LadderStats.ClimbDown_From_Level1_To_Bottom);
                break;

            case LadderAnimations.ClimbDown_Exit_To_Bottom:

                action.ExitAction = true;
                Play(LadderStats.ClimbDown_Exit_To_Bottom);
                break;

            ////  Climb Down
            //case 5:
            //    // Climb ladder to zone 1

            //    LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Zone_1_to_0);
            //    StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Zone_1_to_0].length));
            //    break;
            //case 6:
            //    // Climb ladder to zone 2
            //    LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Zone_2_to_1);
            //    StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Zone_2_to_1].length));
            //    break;
            //case 7:
            //    // Climb ladder to zone 3
            //    LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Zone_3_to_2);
            //    StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Zone_3_to_2].length));
            //    break;

            ////  Get off the ladder from up or down and stop the interaction
            //case 8:
            //    // Climb down from the ladder onto surface.
            //    LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Get_Down_Origin);
            //    StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Get_Down_Origin].length, false));
            //    break;
            //case 9:
            //    // Get Up from the ladder
            //    LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Get_Up_Origin);
            //    StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Get_Up_Origin].length, false));
            //    break;

            default:
                // Get down from the ladder
                //LadderStats.LadderAnimations.CrossFade(LadderStats.Ladder_Get_Down_Fast_O);
                //StartCoroutine(WaitForEndOfAnimation(LadderStats.LadderAnimations[LadderStats.Ladder_Get_Down_Fast_O].length, false));
                break;
        }
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