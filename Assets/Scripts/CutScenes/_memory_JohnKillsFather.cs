using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class _memory_JohnKillsFather : MonoBehaviour
{
    public Actor John;

    public Actor Father;

    private ActObject[] acts;

    public AudioSource AudioSource;

    void Start()
    {

        AudioSource = GetComponent<AudioSource>();
        GenerateCutscene();

        StartCutscene();
    }

    void StartCutscene()
    {
        currentIndex = 1;

        ContinueCutscene();

        var soundTrack = Resources.Load("Sound/Soundtrack_BentAndBroken") as AudioClip;
        AudioSource.volume = 0.144f;
        AudioSource.PlayOneShot(soundTrack);
    }

    private int currentIndex;

    void ContinueCutscene()
    {
        var act = acts[currentIndex];
        //Debug.Log(act.Time);

        if (act.hasAnimation)
        {
            if (act.Time == 0)
                act.Time = act.animTime;
            act.Actor.ActorAnimator.Play(act.animString);
        }

        if (act.hasLine)
        {
            if (act.hasAnimation == false || act.Time == 0)
                act.Time = act.lineTime;
            act.Actor.SayLine(act.Line, act.DialogBoxType, act.lineTime);
        }

        if (act.nextImmediate)
        {
            currentIndex = act.nextImmediateIndex;
            ContinueCutscene();

            if (act.nextIndex == 0)
                return;
        }

        if (act.endPoint == false)
            StartCoroutine(WaitForAct(act));
    }

    IEnumerator WaitForAct(ActObject act)
    {
        yield return new WaitForSeconds(act.Time);
        if (act.hasPauseAfter)
            yield return new WaitForSeconds(act.PauseLength);

        if (currentIndex < act.nextIndex)
        {
            currentIndex = act.nextIndex;
            //Debug.Log(act.nextIndex);
            ContinueCutscene();
        }
    }

    void GenerateCutscene()
    {
        acts = new ActObject[20];

        acts[1] = new ActObject
        {
            nextImmediate = true,
            nextImmediateIndex = 2,
            nextIndex = 0,  //  No next index;

            Actor = John,

            hasAnimation = true,
            animString = "Act1_John_Idle",
        };

        acts[2] = new ActObject
        {
            nextIndex = 3,

            Actor = Father,

            hasLine = true,
            Line = "John. I've been looking everywhere for you. \n Why haven't you stuck with the plan ?",
            lineTime = Father.ActorAnimator["Act1_Father_Walks_ToScene"].length,
            DialogBoxType = DialogBoxType.LeftSide_2Sentence,

            hasAnimation = true,
            animString = "Act1_Father_Walks_ToScene",
            animTime = Father.ActorAnimator["Act1_Father_Walks_ToScene"].length,

            hasPauseAfter = true,
            PauseLength = 3f
        };

        acts[3] = new ActObject
        {
            nextIndex = 4,

            Actor = Father,

            hasLine = true,
            Line = "John? ...",
            lineTime = 2.5f,
            DialogBoxType = DialogBoxType.LeftSide_1SmallSentence,

            hasPauseAfter = true,
            PauseLength = 2f
        };

        acts[4] = new ActObject
        {
            nextIndex = 5,

            Actor = John,

            hasLine = true,
            Line = "I had to do it ... I had too",
            lineTime = John.ActorAnimator["Act1_John_Idle_SweatClear"].length,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,

            hasAnimation = true,
            animString = "Act1_John_Idle_SweatClear",
            animTime = John.ActorAnimator["Act1_John_Idle_SweatClear"].length,

            hasPauseAfter = true,
            PauseLength = 2f
        };

        acts[5] = new ActObject
        {
            nextImmediate = true,
            nextImmediateIndex = 6,
            nextIndex = 7,  // this is immediate so we set nextIndex as the index at wich we want the conversation to continue.

            Actor = John,

            hasAnimation = true,
            animString = "Act1_JohnLightsUpACigarette",
            animTime = John.ActorAnimator["Act1_JohnLightsUpACigarette"].length,
        };

        acts[6] = new ActObject
        {
            endPoint = true,

            Actor = Father,

            hasLine = true,
            Line = "What did you do John ?",
            lineTime = John.ActorAnimator["Act1_JohnLightsUpACigarette"].length / 4,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
        };

        acts[7] = new ActObject
        {
            nextImmediate = true,
            nextImmediateIndex = 8,
            nextIndex = 10,

            Actor = John,

            hasAnimation = true,
            animString = "Act1_John_Cigarette_Turn_ToFather_Talk",
            animTime = John.ActorAnimator["Act1_John_Cigarette_Turn_ToFather_Talk"].length,

            hasPauseAfter = true,
            PauseLength = 1f
        };

        acts[8] = new ActObject
        {
            nextIndex = 9,

            Actor = John,

            hasLine = true,
            Line = "Only we know of it now ...",
            lineTime = 2f,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
 
            hasPauseAfter = true,
            PauseLength = 4.5f
        };

        acts[9] = new ActObject
        {
            endPoint = true,

            Actor = John,

            hasLine = true,
            Line = "Only you, and me ...",
            lineTime = John.ActorAnimator["Act1_John_Cigarette_Turn_ToFather_Talk"].length,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
        };
    }
}