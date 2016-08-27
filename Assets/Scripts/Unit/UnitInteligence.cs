using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitInteligence : MonoBehaviour
{
    private Unit _unit;

    private UnitUI _unitUi;

    public AiReactor AiReactor;

    public bool Alive;

    private List<Neuron> _neurons;
    public Neuron CurrentNeuron;

    private List<ToDo> ThingsToDo;

    public List<Job> Jobs;

    [SerializeField]
    public List<BehaviourState> BehaviourStates;

    [SerializeField]
    private AlertType _alertType;
    [SerializeField]
    public AlertType AlertType
    {
        get { return _alertType; }
        set
        {
            _alertType = value;
            switch (_alertType)
            {
                case AlertType.Hearing:

                    if (BehaviourStates.Contains(BehaviourState.Suspicious) == false)
                        BehaviourStates.Add(BehaviourState.Suspicious);
                    break;

                case AlertType.InFieldOfView:

                    if (BehaviourStates.Contains(BehaviourState.Alerted) == false)
                    {
                        if (BehaviourStates.Contains(BehaviourState.Suspicious) == false)
                            BehaviourStates.Add(BehaviourState.Suspicious);
                        BehaviourStates.Add(BehaviourState.Alerted);
                    }
                    break;

                case AlertType.Seeing:

                    if (BehaviourStates.Contains(BehaviourState.Agressive) == false)
                    {
                        if (BehaviourStates.Contains(BehaviourState.Alerted) == false)
                        {
                            if (BehaviourStates.Contains(BehaviourState.Suspicious) == false)
                                BehaviourStates.Add(BehaviourState.Suspicious);
                            BehaviourStates.Add(BehaviourState.Alerted);
                        }
                        BehaviourStates.Add(BehaviourState.Agressive);
                    }
                    break;
            }

            EvaluateNeuron();
        }
    }

    private bool _facingAlert;
    public bool FacingAlert
    {
        get { return _facingAlert; }
        set
        {
            _facingAlert = value;
            EvaluateNeuron();
        }
    }

    public Vector3 AlertPosition;
    private Vector3 _lastPlayerPosition;

    // this has to be moved in UnitProperties (but im lazy :} )
    private int _layerMask;
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
        _unitUi = GetComponent<UnitUI>();
        _unitUi.Initialize(_unit);

        AiReactor = GetComponent<AiReactor>();
        AiReactor.Initialize();

        // layermasks
        var wallLayerMask = 1 << LayerMask.NameToLayer("WallOrObstacle");
        var unitLayerMask = 1 << LayerMask.NameToLayer("UnitInteraction");
        _layerMask = wallLayerMask | unitLayerMask; // Shoot ray only on wallLayer or unitLayer

        //StartAI();
    }

    private void StartAI()
    {
        Alive = true;
        Jobs.Add(Job.Guard); // main ocupation

        _neurons = Logic.Neurons;

        ThingsToDo = Logic.GetToDos(Jobs.FirstOrDefault());

        CurrentNeuron = _neurons.FirstOrDefault();

        EvaluateNeuron();
    }

    private void EvaluateNeuron()
    {
        Debug.Log(CurrentNeuron.Self + " do Action: " + CurrentNeuron.Method);
        CurrentNeuron.Action(_unit);
    }

    public void EvaluateNextNeuron(bool edge)
    {
        CurrentNeuron = Logic.GetChildWithEdge(CurrentNeuron.Id.Value, edge);

        if (CurrentNeuron == null)
            Debug.LogError("I'm losing my mind.");
        else
            EvaluateNeuron();
    }

    public void FoundSolution()
    {
        CurrentNeuron = _neurons.FirstOrDefault();
    }

    public void DoYourJob()
    {
        Debug.Log("Just doing my job.");
    }

    public void TurnToAlert()
    {
        _unit.UnitController.TurnToTargetPosition = AlertPosition;
        StartCoroutine("WaitStatus");
    }

    public void TryToSeeEnemy()
    {
        if (CheckIfYouCanSeePlayer())
        {
            _lastPlayerPosition = AlertPosition;
            StartCoroutine(Wait(1.2f));
        }
    }

    public bool CheckIfYouCanSeePlayer()
    {
        var direction = Logic.GetDirection(_unit.UnitProperties.HeadPosition, AlertPosition);
        RaycastHit hit;
        if (Physics.Raycast(new Ray(_unit.UnitProperties.HeadPosition, direction), out hit, float.PositiveInfinity, _layerMask))
        {
            return hit.transform.tag == "Player";
        }
        return false;
    }

    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

        if (CheckIfYouCanSeePlayer())
        {
            AlertType = AlertType.Seeing;
            if (BehaviourStates.Contains(BehaviourState.Agressive) == false)
            {
                if (BehaviourStates.Contains(BehaviourState.Alerted) == false)
                {
                    if (BehaviourStates.Contains(BehaviourState.Suspicious) == false)
                        BehaviourStates.Add(BehaviourState.Suspicious);
                    BehaviourStates.Add(BehaviourState.Alerted);
                }
                BehaviourStates.Add(BehaviourState.Agressive);
            }
        }
        // else the WaitStatus for Behaviour.Suspicious is going to end the alert.
    }

    public IEnumerator WaitStatus()
    {
        yield return new WaitForSeconds(_unitUi.StatusBarTime);

        //switch (BehaviourStates)
        //{
        //    case BehaviourState.Suspicious:

        //        BehaviourStates = BehaviourState.Idle;
        //        break;

        //    case BehaviourState.Agressive:
        //        BehaviourStates = BehaviourState.Suspicious;
        //        break;
        //}

        _unitUi.CancelInvoke("UpdateStatusBar");
    }

    public void Alert(Vector3 position, AlertType alertType)
    {
        AlertPosition = position;
        AlertType = alertType;
    }
}