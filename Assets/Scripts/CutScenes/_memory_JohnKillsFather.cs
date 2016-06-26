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

        if (act.HasAnimation)
        {
            if (act.Time == 0)
                act.Time = act.AnimTime;
            act.Actor.ActorAnimator.Play(act.AnimString);
        }

        if (act.HasLine)
        {
            if (act.HasAnimation == false || act.Time == 0)
                act.Time = act.LineTime;
            act.Actor.SayLine(act.Line, act.DialogBoxType, act.LineTime);
        }

        if (act.NextImmediate)
        {
            currentIndex = act.NextImmediateIndex;
            ContinueCutscene();

            if (act.NextIndex == 0)
                return;
        }

        if (act.EndPoint == false)
            StartCoroutine(WaitForAct(act));
    }

    IEnumerator WaitForAct(ActObject act)
    {
        yield return new WaitForSeconds(act.Time);
        if (act.HasPauseAfter)
            yield return new WaitForSeconds(act.PauseLength);

        if (currentIndex < act.NextIndex)
        {
            currentIndex = act.NextIndex;
            //Debug.Log(act.nextIndex);
            ContinueCutscene();
        }
    }

    void GenerateCutscene()
    {
        acts = new ActObject[20];

        acts[1] = new ActObject
        {
            NextImmediate = true,
            NextImmediateIndex = 2,
            NextIndex = 0,  //  No next index;

            Actor = John,

            HasAnimation = true,
            AnimString = "Act1_John_Idle",
        };

        acts[2] = new ActObject
        {
            NextIndex = 3,

            Actor = Father,

            HasLine = true,
            Line = "John. I've been looking everywhere for you. \n Why haven't you stuck with the plan ?",
            LineTime = Father.ActorAnimator["Act1_Father_Walks_ToScene"].length,
            DialogBoxType = DialogBoxType.LeftSide_2Sentence,

            HasAnimation = true,
            AnimString = "Act1_Father_Walks_ToScene",
            AnimTime = Father.ActorAnimator["Act1_Father_Walks_ToScene"].length,

            HasPauseAfter = true,
            PauseLength = 3f
        };

        acts[3] = new ActObject
        {
            NextIndex = 4,

            Actor = Father,

            HasLine = true,
            Line = "John? ...",
            LineTime = 2.5f,
            DialogBoxType = DialogBoxType.LeftSide_1SmallSentence,

            HasPauseAfter = true,
            PauseLength = 2f
        };

        acts[4] = new ActObject
        {
            NextIndex = 5,

            Actor = John,

            HasLine = true,
            Line = "I had to do it ... I had too",
            LineTime = John.ActorAnimator["Act1_John_Idle_SweatClear"].length,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,

            HasAnimation = true,
            AnimString = "Act1_John_Idle_SweatClear",
            AnimTime = John.ActorAnimator["Act1_John_Idle_SweatClear"].length,

            HasPauseAfter = true,
            PauseLength = 2f
        };

        acts[5] = new ActObject
        {
            NextImmediate = true,
            NextImmediateIndex = 6,
            NextIndex = 7,  // this is immediate so we set nextIndex as the index at wich we want the conversation to continue.

            Actor = John,

            HasAnimation = true,
            AnimString = "Act1_JohnLightsUpACigarette",
            AnimTime = John.ActorAnimator["Act1_JohnLightsUpACigarette"].length,
        };

        acts[6] = new ActObject
        {
            EndPoint = true,

            Actor = Father,

            HasLine = true,
            Line = "What did you do John ?",
            LineTime = John.ActorAnimator["Act1_JohnLightsUpACigarette"].length / 4,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
        };

        acts[7] = new ActObject
        {
            NextImmediate = true,
            NextImmediateIndex = 8,
            NextIndex = 10,

            Actor = John,

            HasAnimation = true,
            AnimString = "Act1_John_Cigarette_Turn_ToFather_Talk",
            AnimTime = John.ActorAnimator["Act1_John_Cigarette_Turn_ToFather_Talk"].length,

            HasPauseAfter = true,
            PauseLength = 1f
        };

        acts[8] = new ActObject
        {
            NextIndex = 9,

            Actor = John,

            HasLine = true,
            Line = "Only we know of it now ...",
            LineTime = 2f,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
 
            HasPauseAfter = true,
            PauseLength = 4.5f
        };

        acts[9] = new ActObject
        {
            EndPoint = true,

            Actor = John,

            HasLine = true,
            Line = "Only you, and me ...",
            LineTime = John.ActorAnimator["Act1_John_Cigarette_Turn_ToFather_Talk"].length,
            DialogBoxType = DialogBoxType.LeftSide_1MediumSentence,
        };
    }
}