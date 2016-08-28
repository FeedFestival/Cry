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

    private Vector3 _lastPlayerPosition;

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

    private bool CheckBehaviour(BehaviourState behaviour)
    {
        if (Unit.UnitInteligence.BehaviourState == behaviour)
        {
            return true;
        }
        return false;
    }
    private bool CheckBehaviourState(List<BehaviourState> behaviourStates)
    {
        foreach (var b in behaviourStates)
        {
            if (Unit.UnitInteligence.BehaviourState == b)
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
    private bool CheckAlertTypes(AlertType alertType)
    {
        foreach (var key in Unit.UnitInteligence.AlertsType)
        {
            if (key == alertType)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckMainState(MainState mainState)
    {
        if (Unit.UnitInteligence.MainState == mainState)
            return true;
        return false;
    }

    public bool CheckBehaviourState(BehaviourState behaviour)
    {
        return CheckBehaviour(behaviour);
    }

    public bool CheckActionTowardsAlert(ActionTowardsAlert actionTowardsAlert)
    {
        if (Unit.UnitInteligence.ActionTowardsAlert == actionTowardsAlert)
            return true;
        return false;
    }

    public bool CheckEnemyState(EnemyState enemyState)
    {
        if (Unit.UnitInteligence.EnemyState == enemyState)
            return true;
        return false;
    }

    public bool CheckActionTowardsEnemy(ActionTowardsEnemy actionTowardsEnemy)
    {
        if (Unit.UnitInteligence.ActionTowardsEnemy == actionTowardsEnemy)
            return true;
        return false;
    }

    public void CheckAlertType()
    {
        //Unit.UnitInteligence.EvaluateNextNeuron(CheckAlertTypes(Unit.UnitInteligence.AlertType, Unit.UnitInteligence.CurrentNeuron.KeywordList));
    }

    public void InvestigateLastKnownLocation()
    {
        //Debug.Log("Move to last knows location of the alert.");
    }

    public void DoYourJob()
    {
        Debug.Log("Just doing my job.");
    }

    public void TurnToAlert()
    {
        Unit.UnitController.TurnToTargetPosition = Unit.UnitInteligence.AlertPosition;
        //StartCoroutine("WaitStatus");
    }

    // See Sensory System
    //------------------------------------------------------

    private bool _tryToSeeEnemy_Running;
    public void TryToSeeEnemy()
    {
        if (_tryToSeeEnemy_Running == false)
        {
            _tryToSeeEnemy_Running = true;
            if (CheckIfYouCanSeePlayer())
            {
                _lastPlayerPosition = Unit.UnitInteligence.AlertPosition;
                Unit.UnitInteligence.ActionTowardsEnemy = ActionTowardsEnemy.LookAtEnemy;

                if (_waitToSeeEnemy_Running == false)
                    StartCoroutine(WaitToSeeEnemy(Unit.UnitInteligence.TryToSeeEnemyTime));
            }
        }
    }

    private bool _waitToSeeEnemy_Running;
    public IEnumerator WaitToSeeEnemy(float time)
    {
        _waitToSeeEnemy_Running = true;
        yield return new WaitForSeconds(time);
        _waitToSeeEnemy_Running = false;
        _tryToSeeEnemy_Running = false;
        if (CheckIfYouCanSeePlayer())
        {
            Unit.UnitInteligence.AddAlert(AlertType.Seeing);
        }
        // else the WaitStatus for Behaviour.Suspicious is going to end the alert.
    }

    public bool CheckIfYouCanSeePlayer()
    {
        bool hitOneBodyPart = false;
        if (Unit.UnitInteligence.Enemy != null && Unit.UnitInteligence.Enemy.UnitProperties.BodyParts != null)
            for (var i = 0; i < Unit.UnitInteligence.Enemy.UnitProperties.BodyParts.Count; i++)
            {
                var direction = Logic.GetDirection(Unit.UnitProperties.HeadPosition,
                    Unit.UnitInteligence.Enemy.UnitProperties.BodyParts[i].transform.position);
                RaycastHit hit;
                if (Physics.Raycast(new Ray(Unit.UnitProperties.HeadPosition, direction), out hit,
                    float.PositiveInfinity,
                    Unit.UnitInteligence.LayerMask))
                {
                    //Debug.Log("- looking for " + (BodyPart)i + " and hit " + hit.transform.gameObject.name);
                    if (hit.transform.tag == "Player")
                    {
                        Unit.UnitInteligence.AlertPosition = hit.transform.position;
                        hitOneBodyPart = true;
                        break;
                    }
                }
            }
        //else
        //{
        //    var direction = Logic.GetDirection(Unit.UnitProperties.HeadPosition, Unit.UnitInteligence.AlertPosition + new Vector3(0, 1f, 0));
        //    RaycastHit hit;
        //    if (Physics.Raycast(new Ray(Unit.UnitProperties.HeadPosition, direction), out hit, float.PositiveInfinity, Unit.UnitInteligence.LayerMask))
        //    {
        //        Debug.DrawRay(Unit.UnitProperties.HeadPosition, direction, Color.blue, 5f);
        //        return hit.transform.tag == "Player";
        //    }
        //}

        if (hitOneBodyPart == false)
        {
            if (Unit.UnitInteligence.ActionTowardsEnemy == ActionTowardsEnemy.LookAtEnemy)
                Unit.UnitInteligence.ActionTowardsEnemy = ActionTowardsEnemy.None;
            Unit.UnitInteligence.RemoveAlert(AlertType.Seeing);
        }

        return hitOneBodyPart;
    }

    void Update()
    {
        if (Unit.UnitInteligence.Enemy != null)
            for (var i = 0; i < Unit.UnitInteligence.Enemy.UnitProperties.BodyParts.Count; i++)
            {

                var direction = Logic.GetDirection(Unit.UnitProperties.HeadPosition, Unit.UnitInteligence.Enemy.UnitProperties.BodyParts[i].transform.position);
                Debug.DrawRay(Unit.UnitProperties.HeadPosition, direction, Color.red);
            }
    }













    public void ReduceDistanceToEnemy()
    {
        Unit.UnitInteligence.ActionTowardsEnemy = ActionTowardsEnemy.ReduceDistance;

        // while player is in field of view and you can see him.

        // based on the agression level. reduce distance.
    }






    //--
    public bool CheckJob()
    {
        if (Unit.UnitInteligence.Jobs != null && Unit.UnitInteligence.Jobs.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void DoJob()
    {
        Unit.UnitInteligence.BehaviourState = BehaviourState.DoingJob;
        Debug.Log("Do Job");
    }

    public IEnumerator<BAD.NodeResult> Nothing()
    {
        yield return BAD.NodeResult.Success;
    }
}