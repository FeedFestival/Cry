using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class ChairAnimation : MonoBehaviour {

    private Chair Chair;

    public void Initialize(Chair chair)
    {
        Chair = chair;
    }

    public void PlayAnimation(ChairStaticAnimation animation)
    {
        //Chair.lastPlayedAnimation = animationIndex;
        float animationLenght = 0.0f;
        switch (animation)
        {
            case ChairStaticAnimation.GetOn_FromFront:

                Chair.ChairStaticAnimator.CrossFade(Chair.GetOn_FromFront);
                animationLenght = Chair.ChairStaticAnimator[Chair.GetOn_FromFront].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            case ChairStaticAnimation.GetOn_FromLeft:

                Chair.ChairStaticAnimator.CrossFade(Chair.GetOn_FromLeft);
                animationLenght = Chair.ChairStaticAnimator[Chair.GetOn_FromLeft].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            case ChairStaticAnimation.GetOn_FromRight:

                Chair.ChairStaticAnimator.CrossFade(Chair.GetOn_FromRight);
                animationLenght = Chair.ChairStaticAnimator[Chair.GetOn_FromRight].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            //case ChairAnimation.GetOn_FromRight:

            //    Chair.ChairStaticAnimations.CrossFade(Chair.GetOn_FromRight);
            //    animationLenght = Chair.ChairStaticAnimations[Chair.GetOn_FromRight].length;
            //    StartCoroutine(WaitForEndOfAnimation(animationLenght));
            //    break;

            default:
                break;
        }
    }

    IEnumerator WaitForEndOfAnimation(float animationTime, bool endEvent = false)
    {
        yield return new WaitForSeconds(animationTime);
    }
}
