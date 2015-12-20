using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class _memory_JohnKillsFather : MonoBehaviour
{
    public Actor John;
    public JohnAnimations JohnLastAnimation;

    public Actor Father;
    public FatherAnimations FatherLastAnimation;

    public AudioSource AudioSource;

    void Start() {

        AudioSource = GetComponent<AudioSource>();

        StartCutscene();
    }

    // Use this for initialization
    void StartCutscene()
    {
        var soundTrack = Resources.Load("Sound/Soundtrack_BentAndBroken") as AudioClip;
        AudioSource.volume = 0.144f;
        AudioSource.PlayOneShot(soundTrack);

        John.ActorAnimator.Play(JohnAnimations.Act1_John_Idle.ToString());

        var animTime = Father.ActorAnimator[FatherAnimations.Act1_Father_Walks_ToScene.ToString()].length;
        Father.ActorAnimator.Play(FatherAnimations.Act1_Father_Walks_ToScene.ToString());
        FatherLastAnimation = FatherAnimations.Act1_Father_Walks_ToScene;
        Father.SayLine("John, I've been looking everywhere for you. \n Why havent you sticked with the plan ?"
                        , DialogBoxType.LeftSide_2Sentence
                        , animTime);

        StartCoroutine(WaitForEndOfAnimation(animTime, Father));
    }

    public void EndAct(Actor actor) {
        if (actor.Name == ActorName.John)
        {
            switch (JohnLastAnimation)
            {
                case JohnAnimations.Act1_John_Idle:
                    break;
                case JohnAnimations.Act1_John_Idle_SweatClear:

                    John.ActorAnimator.Play(JohnAnimations.Act1_JohnLightsUpACigarette.ToString());
                    JohnLastAnimation = JohnAnimations.Act1_JohnLightsUpACigarette;
                    var animTime = John.ActorAnimator[JohnAnimations.Act1_JohnLightsUpACigarette.ToString()].length;
                    StartCoroutine(WaitForEndOfAnimation(animTime, John));

                    Father.SayLine("What did you do John ?"
                        , DialogBoxType.LeftSide_1MediumSentence
                        , animTime / 4);

                    break;
                case JohnAnimations.Act1_JohnLightsUpACigarette:

                    var talkTime = 2f;
                    John.SayLine("Only me and you know ..."
                        , DialogBoxType.LeftSide_1SmallSentence
                        , talkTime);

                    StartCoroutine(WaitForTalkLine(talkTime, JohnTalk.Talk2, FatherTalk.NoTalk));

                    break;
                case JohnAnimations.Act1_John_Cigarette_Turn_ToFather_Talk:
                    break;
                default:
                    break;
            }
        }
        else if (actor.Name == ActorName.Father)
        {
            switch (FatherLastAnimation)
            {
                case FatherAnimations.Act1_Father_Idle:
                    
                    break;
                case FatherAnimations.Act1_Father_Walks_ToScene:

                    StartCoroutine(WaitForPauseLine(3f, JohnPause.NoPause, FatherPause.Pause1));

                    break;
                default:
                    break;
            }
        }
    }

    public void EndPause(JohnPause JohnPause, FatherPause FatherPause)
    {
        if (JohnPause != JohnPause.NoPause)
        {
            switch (JohnPause)
            {
                case JohnPause.NoPause:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (FatherPause)
            {
                case FatherPause.NoPause:

                    break;
                case FatherPause.Pause1:

                    Father.ActorAnimator.Play(FatherAnimations.Act1_Father_Idle.ToString());

                    var talkTime = 4f;
                    Father.SayLine("John? ..."
                        , DialogBoxType.LeftSide_1SmallSentence
                        , talkTime / 2);
                    StartCoroutine(WaitForTalkLine(talkTime, JohnTalk.NoTalk, FatherTalk.Talk1));

                    break;
                default:
                    break;
            }
        }
    }

    public void EndTalk(JohnTalk JohnTalk, FatherTalk FatherTalk)
    {
        if (JohnTalk != JohnTalk.NoTalk)
        {
            switch (JohnTalk)
            {
                case JohnTalk.NoTalk:
                    break;
                case JohnTalk.Talk1:

                    John.ActorAnimator.Play(JohnAnimations.Act1_John_Idle_SweatClear.ToString());
                    JohnLastAnimation = JohnAnimations.Act1_John_Idle_SweatClear;
                    var animTime = John.ActorAnimator[JohnAnimations.Act1_John_Idle_SweatClear.ToString()].length;
                    StartCoroutine(WaitForEndOfAnimation(animTime, John));
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (FatherTalk)
            {
                case FatherTalk.NoTalk:
                    break;
                case FatherTalk.Talk1:

                    var talkTime = 4f;
                    John.SayLine("I had to do it Eric ..."
                        , DialogBoxType.LeftSide_1MediumSentence
                        , talkTime);
                    StartCoroutine(WaitForTalkLine(talkTime, JohnTalk.Talk1, FatherTalk.NoTalk));

                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator WaitForEndOfAnimation(float animTime , Actor actor)
    {
        yield return new WaitForSeconds(animTime);
        EndAct(actor);
    }

    IEnumerator WaitForPauseLine(float pauseTime, JohnPause JohnPause, FatherPause FatherPause)
    {
        yield return new WaitForSeconds(pauseTime);
        EndPause(JohnPause, FatherPause);
    }

    IEnumerator WaitForTalkLine(float talkTime, JohnTalk JohnTalk, FatherTalk FatherTalk)
    {
        yield return new WaitForSeconds(talkTime);
        EndTalk(JohnTalk, FatherTalk);
    }
}

public enum JohnPause {
    NoPause = 0
}

public enum FatherPause {
    NoPause = 0, Pause1 = 1
}

public enum JohnTalk
{
    NoTalk = 0, Talk1 = 1, Talk2 = 2
}

public enum FatherTalk
{
    NoTalk = 0, Talk1 = 1
}

public enum JohnAnimations {
    Act1_John_Idle = 0, Act1_John_Idle_SweatClear = 1, Act1_JohnLightsUpACigarette = 2, Act1_John_Cigarette_Turn_ToFather_Talk = 3
}
public enum FatherAnimations
{
    Act1_Father_Idle = 0, Act1_Father_Walks_ToScene = 1
}