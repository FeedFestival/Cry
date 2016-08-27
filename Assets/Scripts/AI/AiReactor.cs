using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BAD;

public class AiReactor : MonoBehaviour
{
    public float TickDuration = 0.1f;
    [System.NonSerialized]
    public bool Fastforward;
    [System.NonSerialized]
    public bool Pause;
    [System.NonSerialized]
    public bool Step;
    [System.NonSerialized]
    public bool Debug = false;

    public float DeltaTime { get; private set; }

    public List<Node> RunningGraphs = new List<Node>();

    public TextAsset BadCode;

    public Blackboard Blackboard = new Blackboard();

    public IEnumerator ReactorReaction;

    public void Initialize()
    {
        var root = Parser.Parse(gameObject, BadCode.text);
        Parse(root);
        RunningGraphs.Add(root);
        ReactorReaction = RunReactor(root.GetNodeTask());
        StartCoroutine(ReactorReaction);
    }

    void Parse(Node node)
    {
        node.reactor = this;
        var branch = node as Branch;
        if (branch != null)
        {
            foreach (var child in branch.children)
            {
                Parse(child);
            }
        }
    }

    IEnumerator RunReactor(IEnumerator<NodeResult> task)
    {
        //This helps mitigate the stampeding herd problem when the level starts with many AI's waiting to start.
        yield return new WaitForSeconds(Random.Range(0f, 0.15f));
        var delay = new WaitForSeconds(TickDuration);
        if (TickDuration <= 0)
            delay = null;
        while (true)
        {
            var startTick = Time.time;
#if UNITY_EDITOR
            if (Step)
            {
                Step = false;
                Pause = true;
                yield return delay;
                task.MoveNext();
            }

            if (Pause)
            {
                yield return null;
                continue;
            }

            if (Fastforward)
            {
                Fastforward = false;
                yield return null;
            }
            else
            {
                yield return delay;
            }
            task.MoveNext();

#else
                if (fastforward) {
                    fastforward = false;
                    yield return null;
                } else {
                    yield return delay;
                } 
                task.MoveNext ();
#endif
            DeltaTime = Time.time - startTick;
        }
    }
}