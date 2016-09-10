using System;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BAD;
using UnityEngine;

public class UnitInteligence : MonoBehaviour
{
    public bool DebugThis;

    private Unit _unit;
    [HideInInspector]
    public UnitUI UnitUi;
    [HideInInspector]
    public AiReactor AiReactor;
    private Branch _guardNeuronBranch;
    [HideInInspector]
    public Branch GuardNeuronRoot
    {
        get
        {
            return _guardNeuronBranch ??
                   (_guardNeuronBranch = Parser.Parse(gameObject, GlobalData.SceneManager.AIUtils.GuardNeurons.text));
        }
    }
    private Guard _guard;
    [HideInInspector]
    public Guard Guard
    {
        get
        {
            if (_guard == null)
                _guard = GetComponent<Guard>();
            return _guard;
        }
    }

    public Unit Player;

    public List<Vector3> HidingSpotsPositions;

    //--
    //  Bools

    public bool PlayerInFOV;

    public Vector3 SoundPosition;

    private Vector3 _lastPlayerPos;
    public Vector3 LastPlayerDirection;
    public Vector3 InitialPlayerPos;

    public void Initialize(Unit unit)
    {
        _unit = unit;

        var go = Logic.CreateFromPrefab("Prefabs/UI/FieldOfView", transform.position);

        go.transform.parent = transform;
        go.name = "AI_FieldOfView";
        go.layer = 11;
        go.transform.eulerAngles = new Vector3(270, 0, 0);

        go.GetComponent<FieldOfView>().Initialize(this);
        //--
        go = Logic.CreateFromPrefab("Prefabs/UI/FieldOfHearing", transform.position);

        go.transform.parent = transform;
        go.name = "AI_FieldOfHearing";
        go.layer = 11;
        go.transform.eulerAngles = new Vector3(0, 0, 0);

        go.GetComponent<FieldOfHearing>().Initialize(this);

        // UI
        UnitUi = GetComponent<UnitUI>();
        UnitUi.Initialize(_unit);

        AiReactor = GetComponent<AiReactor>();

        StartAI();
    }

    private void StartAI()
    {
        //Jobs.Add(Job.Guard); // main ocupation

        AiReactor.Initialize(_unit);

        InvokeRepeating("RunSomeChecks", 1f, 0.4f);
    }

    public void Restart()
    {
        AiReactor.StopReactor();

        gameObject.AddComponent<AiReactor>();
        AiReactor = GetComponent<AiReactor>();

        AiReactor.Initialize(_unit);
    }

    [SerializeField]
    public MainState MainState;

    [SerializeField]
    public MainAction MainAction;

    // Alert
    public List<Alert> Alerts;
    [SerializeField]
    private AlertLevel _alertLevel;
    [SerializeField]
    public AlertLevel AlertLevel
    {
        get { return _alertLevel; }
        set
        {
            UnitUi.PreviousAlertLevel = _alertLevel;
            _alertLevel = value;
            switch (_alertLevel)
            {
                case AlertLevel.None:
                    break;
                case AlertLevel.Suspicious:
                    break;
                case AlertLevel.Talkative:
                    _unit.UnitProperties.MovementSpeed = (int)MovementSpeedStates.Calm;
                    break;
                case AlertLevel.Aggressive:
                    _unit.UnitProperties.MovementSpeed = (int)MovementSpeedStates.Hurry;
                    break;
            }

            UnitUi.ChangeState(_alertLevel);
        }
    }

    public ActionTowardsAlert ActionTowardsAlert;

    public void SetAlert(Unit unit, Alert alertType, Vector3 position = new Vector3())
    {
        if (unit != null)
            Player = unit;

        if (alertType == Alert.Hearing)
            SoundPosition = unit != null ? unit.transform.position : position;

        MainState = MainState.Alerted;
        MainAction = MainAction.CheckingAlert;

        // here we are calling the entire AI tree to be re-evaluated
        Guard.CompleteCurrentTask();
    }

    public void AddAlert(Alert alertType)
    {
        if (Alerts.Contains(alertType) == false)
            Alerts.Add(alertType);
    }

    public void RemoveAlert(Alert alertType)
    {
        if (Alerts.Contains(alertType))
            Alerts.Remove(alertType);
    }

    // TODO - move in a coroutine !! some need to stay in Update though
    void FixedUpdate()
    {
        if (PlayerInFOV && Player != null)
        {
            if (DebugThis)
                for (var i = 0; i < Player.UnitProperties.BodyParts.Count; i++)
                {
                    var direction = Logic.GetDirection(_unit.UnitProperties.HeadPosition, Player.UnitProperties.BodyParts[i].transform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(_unit.UnitProperties.HeadPosition, direction), out hit, float.PositiveInfinity, _unit.UnitProperties.GuardPlayerLayerMask))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            Debug.DrawRay(_unit.UnitProperties.HeadPosition, direction, Color.blue);
                        }
                        else
                        {
                            Debug.DrawRay(_unit.UnitProperties.HeadPosition, direction, Color.red);
                        }
                    }
                }

            if (SeePlayer())
            {
                if (_unit.UnitController.IsLookingAtPlayer == false)
                {
                    AddAlert(Alert.Seeing);
                    _unit.UnitController.IsLookingAtPlayer = true;
                }

                if (MainAction == MainAction.MoveTowardsPlayer)
                {
                    _lastPlayerPos = Player.transform.position;
                    _unit.UnitProperties.ThisUnitTarget.thisTransform.position = _lastPlayerPos;
                    _unit.UnitController.GoToTarget();
                }
            }
            else
            {
                if (_unit.UnitController.IsLookingAtPlayer)
                {
                    RemoveAlert(Alert.Seeing);
                    _unit.UnitController.IsLookingAtPlayer = false;
                }
            }
        }
    }

    void RunSomeChecks()
    {
        if (Alerts.Contains(Alert.Seeing))
        {
            if (AlertLevel == AlertLevel.None)
                Guard.CompleteCurrentTask();
            if (MainState == MainState.Calm)
                MainState = MainState.Alerted;

            if (MainAction == MainAction.MoveTowardsPlayer)
            {
                LastPlayerDirection = Player.transform.forward;

                if (AlertLevel == AlertLevel.Talkative)
                {
                    if (Vector3.Distance(InitialPlayerPos, _lastPlayerPos) > 3.5f)
                    {
                        SetAggressive();
                    }
                }
            }
        }
        else
        {
            if (MainState == MainState.Alerted)
                if (AlertLevel == AlertLevel.Talkative)
                    SetAggressive();
        }

        if (AlertLevel == AlertLevel.Aggressive)
        {
            if (MainAction == MainAction.MoveTowardsPlayer)
                if (Vector3.Distance(transform.position, _lastPlayerPos) < 0.5f)
                {
                    Guard.CompleteCurrentTask();
                }
        }
    }

    private bool SeePlayer()
    {
        if (Player != null && Player.UnitProperties.BodyParts != null)
            for (var i = 0; i < Player.UnitProperties.BodyParts.Count; i++)
            {
                var direction = Logic.GetDirection(_unit.UnitProperties.HeadPosition, Player.UnitProperties.BodyParts[i].transform.position);
                RaycastHit hit;
                if (Physics.Raycast(new Ray(_unit.UnitProperties.HeadPosition, direction), out hit, float.PositiveInfinity, _unit.UnitProperties.GuardPlayerLayerMask))
                {
                    if (hit.transform.tag == "Player")
                    {
                        return true;
                    }
                }
            }
        return false;
    }

    public void SetAggressive()
    {
        MainState = MainState.Aggressive;
        Guard.CompleteCurrentTask();
    }
}