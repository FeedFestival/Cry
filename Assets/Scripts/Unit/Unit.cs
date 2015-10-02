using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Unit : MonoBehaviour
{
    /*
        This script is the component manager of the unit it sits on.
     * - it holds the big SceneManager script that holds the world, and many component script that are about 'this' Unit.
     * - It also holds the Base Stats of the unit, like : HitPoints, AtackSpeed, MovementSpeed ...
     * - It also initializes the components and its dependencies on 'this' Unit.
     */

    [Header("Just for development")]
    //  --------------------------------------------------------------------------------
    public SceneManager SceneManager;

    [Header("Unit States")]
    //  --------------------------------------------------------------------------------

    public PlayerActionInMind PlayerActionInMind;

    public UnitPrimaryState UnitPrimaryState;

    public UnitActionState _unitActionState;
    public UnitActionState UnitActionState 
    {
        get {
            return _unitActionState;
        }
        set 
        {
            _unitActionState = value;
            switch (UnitActionState)
            {
                case UnitActionState.None:
                    this.UnitFeetState = UnitFeetState.OnGround;
                    break;
                case UnitActionState.ClimbingLadder:
                    this.UnitFeetState = UnitFeetState.OnLadder;
                    break;
                case UnitActionState.ClimbingChair:
                    this.UnitFeetState = UnitFeetState.OnChair;
                    break;
                case UnitActionState.ClimbingWall:
                    
                    break;
                default:
                    break;
            }
        }
    }
    public UnitActionInMind UnitActionInMind;
    public UnitFeetState UnitFeetState;

    [HideInInspector]
    public AIPath AIPath;
    [HideInInspector]
    public UnitController UnitController;
    [HideInInspector]
    public UnitActionAnimation UnitActionAnimation;
    [HideInInspector]
    public UnitActionHandler UnitActionHandler;
    [HideInInspector]
    public UnitBasicAnimation UnitBasicAnimation;
    [HideInInspector]
    public UnitProperties UnitProperties;

    [HideInInspector]
    public Animation UnitAnimator;

    [Header("Actions Connections")]
    //  --------------------------------------------------------------------------------

    public Ladder Ladder;
    public Chair Chair;
    public Ledge Ledge;
    public Table Table;

    // Use this for initialization
    public void Initialize(SceneManager sceneManager)
    {
        SceneManager = sceneManager;

        // State Initialization
        UnitPrimaryState = UnitPrimaryState.Idle;

        UnitActionState = UnitActionState.None;
        UnitActionInMind = UnitActionInMind.None;

        UnitFeetState = UnitFeetState.OnGround;

        // Scripts initialization
        AIPath = this.GetComponent<AIPath>();

        UnitController = this.GetComponent<UnitController>();
        if (UnitController)
            UnitController.Initialize(this);

        UnitActionHandler = this.GetComponent<UnitActionHandler>();
        if (UnitActionHandler)
            UnitActionHandler.Initialize(this);

        UnitProperties = this.GetComponent<UnitProperties>();
        if (UnitProperties)
            UnitProperties.Initialize(this);

        UnitBasicAnimation = this.GetComponent<UnitBasicAnimation>();
        if (UnitBasicAnimation)
            UnitBasicAnimation.Initialize(this);

        UnitActionAnimation = this.GetComponent<UnitActionAnimation>();
        if (UnitActionAnimation)
            UnitActionAnimation.Initialize(this);

        UnitController.StopMoving();
    }

    public void SetTeam(bool controlledByAI = false)
    {
        UnitProperties.AIControlled = controlledByAI;
    }
}