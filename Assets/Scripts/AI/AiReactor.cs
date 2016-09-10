using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using BAD;

public class AiReactor : MonoBehaviour
{
    private Unit _unit;

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

    public Blackboard Blackboard = new Blackboard();

    public IEnumerator ReactorReaction;

    public void Initialize(Unit unit)
    {
        _unit = unit;

        Parser.Debug = false;
        StartReactor(_unit.UnitInteligence.GuardNeuronRoot);
    }

    private Branch _currentBranch;

    public void StartReactor(Branch branch)
    {
        if (ReactorReaction != null)
            StopCoroutine(ReactorReaction);
        RunningGraphs.Clear();

        _currentBranch = branch;
        Parse(_currentBranch);

        RunningGraphs.Add(_currentBranch);
        ReactorReaction = RunReactor(_currentBranch.GetNodeTask());

        StartCoroutine(ReactorReaction);
    }

    public void StopReactor()
    {
        //StopCoroutine(ReactorReaction);
        //RunningGraphs.Clear();

        //Parse(_currentBranch, true);
        //Destroy(this);
    }
    
    void Parse(Node node, bool reset = false)
    {
        if (reset)
        {
            node.state = null;
            node.reactor = null;
            node.running = false;
        }
        else
        {
            node.reactor = this;
        }
        var branch = node as Branch;
        if (branch != null)
        {
            foreach (var child in branch.children)
            {
                Parse(child, reset);
            }
        }
    }

    //void deactivateNodes(Node node)
    //{
    //    node.arguments = null;
    //    node.enabled = false;
    //    node.running = false;
    //    node.state = null;

    //    var branch = node as Branch;
    //    if (branch != null)
    //    {
    //        foreach (var child in branch.children)
    //        {
    //            deactivateNodes(child);
    //        }
    //    }
    //}

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