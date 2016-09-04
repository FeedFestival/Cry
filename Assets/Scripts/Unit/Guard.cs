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

    ////--

    private enum waitFor
    {
        Nothing,
        CheckAlert,
        DoJob,
        ReturnSuccess,
        AlertLevel,
        ReduceDistance,
        InvestigateLastKnownLocation,
        ChasePlayer,
        InvestigateSoundLocation
    }

    public bool CheckMainState(MainState mainState)
    {
        return Unit.UnitInteligence.MainState == mainState;
    }

    public IEnumerator<NodeResult> CheckAlert()
    {
        bool exit;
        if (Unit.UnitInteligence.PlayerInFOV && Unit.UnitInteligence.Alerts.Contains(Alert.Seeing))
        {
            exit = true;
        }
        else
        {
            exit = false;
            Unit.UnitInteligence.MainAction = MainAction.CheckingAlert;
            Unit.UnitInteligence.AlertLevel = AlertLevel.Suspicious;
            Unit.UnitController.IsTurningToSound = true;
        }

        SetWaitFor(waitFor.CheckAlert, exit);
        yield return WhenIsDone();
    }

    public bool CheckMainAction(MainAction mainAction)
    {
        return Unit.UnitInteligence.MainAction == mainAction;
    }

    public bool CheckAlertType(Alert alertType)
    {
        return Unit.UnitInteligence.Alerts.Contains(alertType);
    }

    public IEnumerator<NodeResult> WaitSetAlertLevel(AlertLevel alertLevel)
    {
        Unit.UnitInteligence.AlertLevel = alertLevel;

        SetWaitFor(waitFor.AlertLevel);
        yield return WhenIsDone();
    }

    public bool SetAlertLevel(AlertLevel alertLevel)
    {
        Unit.UnitInteligence.AlertLevel = alertLevel;
        return true;
    }

    public IEnumerator<NodeResult> ReduceDistance()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.MoveTowardsPlayer)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            yield return WhenIsDone();
        }
        Unit.UnitInteligence.MainAction = MainAction.MoveTowardsPlayer;
        Unit.UnitInteligence.InitialPlayerPos = GlobalData.Player.transform.position;

        SetWaitFor(waitFor.ReduceDistance);
        yield return WhenIsDone();
    }

    public IEnumerator<NodeResult> DoInvestigateLastKnownLocation()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.InvestigateLastKnownLocation)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            yield return WhenIsDone();
        }
        Unit.UnitInteligence.MainAction = MainAction.InvestigateLastKnownLocation;
        Unit.UnitController.LookingAtDirection = Unit.UnitInteligence.LastPlayerDirection;
        Unit.UnitController.IsLookingAtDirection = true;

        SetWaitFor(waitFor.InvestigateLastKnownLocation);
        yield return WhenIsDone();
    }

    public IEnumerable<NodeResult> DoInvestigateSoundLocation()
    {
        if (_waitFor != waitFor.InvestigateSoundLocation)
        {
            Unit.UnitInteligence.MainAction = MainAction.InvestigateSoundLocation;

            Unit.UnitProperties.ThisUnitTarget.thisTransform.position = Unit.UnitInteligence.SoundPosition;
            Unit.UnitController.GoToTarget();
        }

        SetWaitFor(waitFor.InvestigateSoundLocation);
        yield return WhenIsDone();
    }

    public IEnumerator<NodeResult> ChasePlayer()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.MoveTowardsPlayer)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            yield return WhenIsDone();
        }
        Unit.UnitInteligence.MainAction = MainAction.MoveTowardsPlayer;

        SetWaitFor(waitFor.ChasePlayer);
        yield return WhenIsDone();
    }

    public bool DoJob()
    {
        Unit.UnitInteligence.MainAction = MainAction.DoingJob;
        return _exitValue;
    }

    public IEnumerator<NodeResult> ReturnSuccess()
    {
        SetWaitFor(waitFor.ReturnSuccess, true);
        yield return WhenIsDone();
    }

















    //-----------------------------------------------------------------------


    private waitFor _waitFor;
    private void SetWaitFor(waitFor value, bool returnV = false, float time = 0)
    {
        if (_waitFor != value)
        {
            _waitFor = value;
            _compareWaitFor = value;
            _exitValue = returnV;

            if (time != 0f)
            {
                _runningTask = Wait(time);
                StartCoroutine(_runningTask);
            }
        }
    }
    private waitFor _compareWaitFor;
    private bool _exitValue;
    private IEnumerator _runningTask;
    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        _exitValue = true;
    }

    private NodeResult WhenIsDone()
    {
        var theValue = _waitFor == _compareWaitFor ? (_exitValue ? NodeResult.Success : NodeResult.Failure) : NodeResult.Failure;
        if (theValue == NodeResult.Success)
        {
            _waitFor = waitFor.Nothing;
            _exitValue = false;
        }
        return theValue;
    }

    public void CompleteCurrentTask(bool hasCoroutine = false)
    {
        Debug.Log("Continue");

        _exitValue = true;
        if (hasCoroutine)
            StopCoroutine(_runningTask);
    }
}