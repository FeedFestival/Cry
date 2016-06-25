using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Unit : MonoBehaviour
{
    /*
        This script is the component manager of the unit it sits on.
     * - it holds the big SceneManager script that holds the world, and many component script that are about 'this' Player.
     * - It also holds the Base Stats of the unit, like : HitPoints, AtackSpeed, MovementSpeed ...
     * - It also initializes the components and its dependencies on 'this' Player.
     */

    [Header("Just for development")]
    //  --------------------------------------------------------------------------------
    //public SceneManager SceneManager;

    [Header("Player States")]
    //  --------------------------------------------------------------------------------

    [HideInInspector]
    public UnitType UnitType;

    public PlayerActionInMind PlayerActionInMind;

    private UnitPrimaryState _unitPrimaryState;
    public UnitPrimaryState UnitPrimaryState
    {
        get
        {
            return _unitPrimaryState;
        }
        set
        {
            _unitPrimaryState = value;

            if (_unitPrimaryState != UnitPrimaryState.Idle)
            {
                GlobalData.CameraControl.CenterCamera = true;
                GlobalData.CameraControl.HUD.MovementPoints.gameObject.SetActive(false);
            }
            else
            {
                if (GlobalData.CameraControl != null && GlobalData.CameraControl.HUD != null)
                    GlobalData.CameraControl.HUD.MovementPoints.gameObject.SetActive(true);
            }
        }
    }

    private UnitActionState _unitActionState;
    public UnitActionState UnitActionState
    {
        get
        {
            return _unitActionState;
        }
        set
        {
            _unitActionState = value;
            switch (UnitActionState)
            {
                case UnitActionState.None:
                    UnitFeetState = UnitFeetState.OnGround;
                    break;
                case UnitActionState.ClimbingLadder:
                    UnitFeetState = UnitFeetState.OnLadder;
                    break;
                case UnitActionState.ClimbingChair:
                    UnitFeetState = UnitFeetState.OnChair;
                    break;
                case UnitActionState.ClimbingWall:
                    UnitFeetState = UnitFeetState.InAir;
                    break;
                default:
                    break;
            }
        }
    }
    public UnitActionInMind UnitActionInMind;
    public UnitFeetState UnitFeetState;

    public bool IsMouseOverMap;

    [HideInInspector]
    public MainCharacterProperties[] MainCharacterProperties;
    // This variable tell how much inventory space does the player have;

    //[HideInInspector]
    //public AIPath AIPath;
    [HideInInspector]
    public UnitController UnitController;
    [HideInInspector]
    public NavMeshAgent NavMeshAgent;
    [HideInInspector]
    public UnitActionAnimation UnitActionAnimation;
    [HideInInspector]
    public UnitActionHandler UnitActionHandler;
    [HideInInspector]
    public UnitBasicAnimation UnitBasicAnimation;
    [HideInInspector]
    public UnitProperties UnitProperties;

    [HideInInspector]
    public UnitInventory UnitInventory;

    [Header("Model")]
    //  --------------------------------------------------------------------------------
    public Animation UnitAnimator;

    [Header("Actions Connections")]
    //  --------------------------------------------------------------------------------

    public Ladder Ladder;
    public Chair Chair;
    public Ledge Ledge;
    public Table Table;
    public Item Item;
    public Door Door;

    // Use this for initialization
    public void Initialize(UnitType unitType)
    {
        UnitType = unitType;
        UnitPrimaryState = UnitPrimaryState.Idle;
        UnitActionState = UnitActionState.None;
        UnitActionInMind = UnitActionInMind.None;
        UnitFeetState = UnitFeetState.OnGround;

        transform.gameObject.AddComponent<NavMeshAgent>();
        transform.gameObject.AddComponent<UnitProperties>();
        transform.gameObject.AddComponent<UnitController>();
        transform.gameObject.AddComponent<UnitBasicAnimation>();

        UnitController = GetComponent<UnitController>();
        UnitController.Initialize(this);

        NavMeshAgent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgen();

        UnitProperties = GetComponent<UnitProperties>();
        UnitProperties.Initialize(this);

        UnitBasicAnimation = GetComponent<UnitBasicAnimation>();
        UnitBasicAnimation.Initialize(this);

        switch (UnitType)
        {
            case UnitType.Player:
                transform.gameObject.AddComponent<UnitInventory>();
                transform.gameObject.AddComponent<UnitActionHandler>();
                transform.gameObject.AddComponent<UnitActionAnimation>();

                UnitInventory = GetComponent<UnitInventory>();
                UnitInventory.Initialize(this);

                UnitActionHandler = GetComponent<UnitActionHandler>();
                UnitActionHandler.Initialize(this);

                UnitActionAnimation = GetComponent<UnitActionAnimation>();
                UnitActionAnimation.Initialize(this);
                break;

            case UnitType.Enemy:

                break;
        }

        UnitController.StopMoving();
    }

    private void SetupNavMeshAgen()
    {
        NavMeshAgent.updateRotation = false;

        NavMeshAgent.radius = 0.3f;
        NavMeshAgent.height = 2.0f;
        NavMeshAgent.baseOffset = 0;

        NavMeshAgent.speed = 1.8f;
        NavMeshAgent.angularSpeed = 999999;
        NavMeshAgent.acceleration = 44;
        NavMeshAgent.stoppingDistance = 0;
        NavMeshAgent.autoBraking = false;

        NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        NavMeshAgent.avoidancePriority = 50;

        NavMeshAgent.autoTraverseOffMeshLink = true;
        NavMeshAgent.autoRepath = true;
    }

    public void ActivateTarget(bool value)
    {
        UnitProperties.ThisUnitTarget.gameObject.SetActive(value);
    }
}