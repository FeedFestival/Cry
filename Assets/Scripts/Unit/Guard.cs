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

    //-----------------------------------------------------------------------
    //-----------------------------------------------------------------------

    public Neuron GuardNeuron;  // also Root

    public void Initialize()
    {
        GuardNeuron = MindMap.GetGuardNeuron(this);
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
        InvestigateSoundLocation,
        CheckSuroundings
    }

    public NeuronResult CheckStance(Stance stance)
    {
        return Unit.Stance == stance ? NeuronResult.Success : NeuronResult.Fail;
    }

    public NeuronResult CheckMainState(MainState mainState)
    {
        return Unit.UnitInteligence.MainState == mainState ? NeuronResult.Success : NeuronResult.Fail;
    }

    public NeuronResult CheckAlert()
    {
        if (Unit.UnitInteligence.PlayerInFOV && Unit.UnitInteligence.Alerts.Contains(Alert.Seeing))
            return NeuronResult.Success;

        Unit.UnitInteligence.MainAction = MainAction.CheckingAlert;
        Unit.UnitInteligence.AlertLevel = AlertLevel.Suspicious;
        Unit.UnitController.IsTurningToSound = true;

        return NeuronResult.WaitFor;
    }

    public bool CheckMainAction(MainAction mainAction)
    {
        return Unit.UnitInteligence.MainAction == mainAction;
    }

    public NeuronResult CheckAlertType(Alert alertType)
    {
        if (Unit.UnitInteligence.Alerts.Contains(alertType))
            return NeuronResult.Success;
        return NeuronResult.Fail;
    }

    public NeuronResult WaitSetAlertLevel(AlertLevel alertLevel)
    {
        Unit.UnitInteligence.AlertLevel = alertLevel;

        SetWaitFor(waitFor.AlertLevel);
        
        return NeuronResult.WaitFor;
    }

    public NeuronResult SetAlertLevel(AlertLevel alertLevel)
    {
        Unit.UnitInteligence.AlertLevel = alertLevel;

        return NeuronResult.Success;
    }

    public NeuronResult ReduceDistance()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.MoveTowardsPlayer)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            return NeuronResult.WaitFor;
        }
        Unit.UnitInteligence.MainAction = MainAction.MoveTowardsPlayer;
        Unit.UnitInteligence.InitialPlayerPos = GlobalData.Player.transform.position;

        SetWaitFor(waitFor.ReduceDistance);

        return NeuronResult.WaitFor;
    }

    public NeuronResult ChasePlayer()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.MoveTowardsPlayer)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            return NeuronResult.WaitFor;
        }
        Unit.UnitInteligence.MainAction = MainAction.MoveTowardsPlayer;

        SetWaitFor(waitFor.ChasePlayer);

        return NeuronResult.WaitFor;
    }

    public NeuronResult DoInvestigateLastKnownLocation()
    {
        if (Unit.UnitInteligence.MainAction == MainAction.InvestigateLastKnownLocation)
        {
            if (_exitValue)
                Unit.UnitInteligence.MainAction = MainAction.DoingNothing;
            return NeuronResult.WaitFor;
        }
        Unit.UnitInteligence.MainAction = MainAction.InvestigateLastKnownLocation;
        Unit.UnitController.LookingAtDirection = Unit.UnitInteligence.LastPlayerDirection;
        Unit.UnitController.IsLookingAtDirection = true;

        SetWaitFor(waitFor.InvestigateLastKnownLocation);

        return NeuronResult.WaitFor;
    }

    /*
     
    Check Surrounding after looking at the direction the Player was looking.
        - take this time to analize hiding spots and add them to the blackboard.
         *///------------------------
    public NeuronResult CheckSuroundings()
    {
        if (_waitFor != waitFor.CheckSuroundings)
        {
            Unit.UnitInteligence.AlertLevel = AlertLevel.Talkative;

            Unit.UnitInteligence.MainAction = MainAction.CheckHidingSpots;
        }

        //SetWaitFor(waitFor.CheckSuroundings, false, 3.5f);
        
        return NeuronResult.WaitFor;
    }

    public NeuronResult InvestigateSoundLocation()
    {
        Unit.UnitInteligence.Restart();

        if (_waitFor != waitFor.InvestigateSoundLocation)
        {
            Unit.UnitInteligence.MainAction = MainAction.InvestigateSoundLocation;

            Unit.UnitProperties.ThisUnitTarget.thisTransform.position = Unit.UnitInteligence.SoundPosition;
            Unit.UnitController.GoToTarget();
        }

        SetWaitFor(waitFor.InvestigateSoundLocation);
        
        return NeuronResult.WaitFor;
    }

    public NeuronResult DoJob()
    {
        Unit.UnitInteligence.MainAction = MainAction.DoingJob;

        return NeuronResult.WaitFor;
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
        Debug.Log("waited " + time);
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

    public void DisplayErrors(List<string> errors)
    {
        Debug.Log(" Errors on " + Unit.gameObject.name);
        Debug.Log(" ^ ^ ^ ");
        foreach (var error in errors)
        {
            Debug.Log(error);
        }
    }
}