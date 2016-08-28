using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BAD;
using UnityEngine;

public class UnitInteligence : MonoBehaviour
{
    private Unit _unit;

    public UnitUI UnitUi;

    public AiReactor AiReactor;
    
    public Branch GuardNeuronRoot
    {
        get
        {
            if (_guardNeuronBranch == null)
                _guardNeuronBranch = Parser.Parse(gameObject, GlobalData.SceneManager.AIUtils.GuardNeurons.text);
            return _guardNeuronBranch;
        }
    }

    public Vector3 AlertPosition;

    //private List<ToDo> ThingsToDo;

    public List<Job> Jobs;

    [SerializeField]
    public MainState MainState;

    [SerializeField]
    public BehaviourState PreviousBehaviourState;
    
    [SerializeField]
    public BehaviourState BehaviourState
    {
        get { return _behaviourState; }
        set
        {
            PreviousBehaviourState = _behaviourState;
            _behaviourState = value;
            UnitUi.ChangeState(_behaviourState);
        }
    }
    
    public Unit Enemy;

    [SerializeField]
    public List<AlertType> AlertsType;
    
    [SerializeField]
    public ActionTowardsAlert ActionTowardsAlert
    {
        get { return _actionTowardsAlert; }
        set
        {
            _actionTowardsAlert = value;
            // try to see enemy before ai tree kicks in.
            if (_actionTowardsAlert == ActionTowardsAlert.FacingAlertPosition)
            {
                GetComponent<Guard>().TryToSeeEnemy();
            }
        }
    }

    [SerializeField]
    public EnemyState EnemyState;

    [SerializeField]
    public AggressionLevel AggressionLevel;

    [SerializeField]
    public ActionTowardsEnemy ActionTowardsEnemy;

    [HideInInspector]
    public float TryToSeeEnemyTime = 1f;

    // this has to be moved in UnitProperties (but im lazy :} )
    public int LayerMask;

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

        // layermasks
        
        LayerMask = GlobalData.WallLayerMask | GlobalData.SensoryVisionLayerMask; // Shoot ray only on wallLayer or unitLayer

        StartAI();
    }

    private void StartAI()
    {
        Jobs.Add(Job.Guard); // main ocupation

        AiReactor.Initialize(_unit);
    }

    public void Alert(Unit unit, AlertType alertType, Vector3 position = new Vector3())
    {
        if (unit != null)
        {
            Enemy = unit;
            AlertPosition = unit.transform.position;
        }
        else
        {
            AlertPosition = position;
        }
        MainState = MainState.Alerted;
        AddAlert(alertType);
    }
    
    public void AddAlert(AlertType alertType)
    {
        if (AlertsType.Contains(alertType) == false)
            AlertsType.Add(alertType);
        switch (alertType)
        {
            case AlertType.Hearing:
                BehaviourState = BehaviourState.Suspicious;
                break;
            case AlertType.InFieldOfView:
                //BehaviourState = BehaviourState.Aggressive;
                break;
            case AlertType.Seeing:
                BehaviourState = BehaviourState.Aggressive;
                break;
        }
    }

    public void RemoveAlert(AlertType alertType)
    {
        if (alertType == AlertType.InFieldOfView)
            Enemy = null;
        if (AlertsType.Contains(alertType))
            AlertsType.Remove(alertType);
    }

    private Branch _guardNeuronBranch;
    [SerializeField]
    private BehaviourState _behaviourState;
    [SerializeField]
    private ActionTowardsAlert _actionTowardsAlert;
}