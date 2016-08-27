using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;

public class Guard : MonoBehaviour
{
    private Unit _unitPlaceHolder;
    private Unit Unit
    {
        get
        {
            if (_unitPlaceHolder == null)
                _unitPlaceHolder = GetComponent<Unit>();
            return _unitPlaceHolder;
        }
    }

    private bool CheckJobs(List<Job> states, List<Job> jobs, bool all = false)
    {
        bool exist = false;
        foreach (var state in states)
        {
            foreach (var job in jobs)
            {
                if (state == job)
                {
                    exist = true;
                    if (all)
                        break;
                }
            }
        }

        return exist;
    }
    private bool CheckBehaviourState(List<BehaviourState> behaviourStates, BehaviourState behaviourState)
    {
        foreach (var b in behaviourStates)
        {
            if (behaviourState == b)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckBehaviourStates(List<BehaviourState> behaviourStates, List<BehaviourState> behaviours, bool all = false)
    {
        bool exist = false;
        foreach (var behaviourState in behaviourStates)
        {
            foreach (var key in behaviours)
            {
                if (key == behaviourState)
                {
                    exist = true;
                    if (all)
                        break;
                }
            }
        }
        return exist;
    }
    private bool CheckAlertTypes(AlertType alertType, List<string> keywordList)
    {
        foreach (var key in keywordList)
        {
            if (key == alertType.ToString())
            {
                return true;
            }
        }
        return false;
    }

    public void CheckAlive()
    {
        Unit.UnitInteligence.EvaluateNextNeuron(Unit.UnitInteligence.Alive);
    }

    public bool CheckBehaviourState(BehaviourState behaviour)
    {
        var list = new List<BehaviourState>();
        list.Add(behaviour);
        return CheckBehaviourStates(Unit.UnitInteligence.BehaviourStates, list);
    }

    public void CheckAlertType()
    {
        Unit.UnitInteligence.EvaluateNextNeuron(CheckAlertTypes(Unit.UnitInteligence.AlertType, Unit.UnitInteligence.CurrentNeuron.KeywordList));
    }

    public void CheckEnviorment()
    {
        switch (Unit.UnitInteligence.AlertType)
        {
            case AlertType.Hearing:
                Unit.UnitInteligence.TurnToAlert();
                break;
            case AlertType.InFieldOfView:
                Unit.UnitInteligence.TryToSeeEnemy();
                break;
            case AlertType.Seeing:
                Unit.UnitInteligence.TryToSeeEnemy();
                break;
        }
        Unit.UnitInteligence.FoundSolution();
    }

    public bool CheckJob()
    {
        return (Unit.UnitInteligence.Jobs != null && Unit.UnitInteligence.Jobs.Count > 0);
    }

    public void DoJob()
    {
        Debug.Log("Do Job");
        Unit.UnitInteligence.DoYourJob();
        Unit.UnitInteligence.FoundSolution();
    }

    public void Something()
    {
        Debug.Log(this);
    }
    public void Nothing()
    {
        StopCoroutine(Unit.UnitInteligence.AiReactor.ReactorReaction);
        Debug.Log("Nothing");
    }
}
