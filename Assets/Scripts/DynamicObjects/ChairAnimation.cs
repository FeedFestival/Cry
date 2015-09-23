using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class ChairAnimation : MonoBehaviour {

    private ChairStats ChairStats;

    public void Initialize(ChairStats chairStats)
    {
        ChairStats = chairStats;
    }

    public void PlayAnimation(ChairStaticAnimation animation)
    {
        //ChairStats.lastPlayedAnimation = animationIndex;
        float animationLenght = 0.0f;
        switch (animation)
        {
            case ChairStaticAnimation.GetOn_FromFront:

                ChairStats.ChairStaticAnimator.CrossFade(ChairStats.GetOn_FromFront);
                animationLenght = ChairStats.ChairStaticAnimator[ChairStats.GetOn_FromFront].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            case ChairStaticAnimation.GetOn_FromLeft:

                ChairStats.ChairStaticAnimator.CrossFade(ChairStats.GetOn_FromLeft);
                animationLenght = ChairStats.ChairStaticAnimator[ChairStats.GetOn_FromLeft].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            case ChairStaticAnimation.GetOn_FromRight:

                ChairStats.ChairStaticAnimator.CrossFade(ChairStats.GetOn_FromRight);
                animationLenght = ChairStats.ChairStaticAnimator[ChairStats.GetOn_FromRight].length;
                StartCoroutine(WaitForEndOfAnimation(animationLenght));
                break;

            //case ChairAnimation.GetOn_FromRight:

            //    ChairStats.ChairStaticAnimations.CrossFade(ChairStats.GetOn_FromRight);
            //    animationLenght = ChairStats.ChairStaticAnimations[ChairStats.GetOn_FromRight].length;
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
