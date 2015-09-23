using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitStats : MonoBehaviour
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

    public UnitPrimaryState UnitPrimaryState;

    public UnitActionState UnitActionState;
    public UnitActionInMind UnitActionInMind;

    public UnitFeetState UnitFeetState;

    //  Unit States - End
    //  --------------------------------------------------------------------------------

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
    public BaseAI UnitBaseAI;

    
    [Header("Props")]
    //  --------------------------------------------------------------------------------
    [HideInInspector]
    public string Tag;

    [HideInInspector]
    public UnitTarget thisUnitTarget;

    private Transform _root;
    [HideInInspector]
    public Transform Root
    {
        get
        {
            return _root;
        }
        set
        {
            _root = value;
            if (UnitActionAnimation)
            {
                if (_root != null)
                {
                    UnitActionAnimation.ControllerFollowRoot = true;
                }
                else
                {
                    //_root = OriginalRoot;
                    UnitActionAnimation.ControllerFollowRoot = false;
                }
            }
        }
    }

    [HideInInspector]
    public Transform thisTransform;
    [HideInInspector]
    public Animation UnitAnimator;

    // The feet collider. we will disable it until we get off the event
    [HideInInspector]
    public GameObject FeetCollider;

    [Header("Actions Connections")]
    //  --------------------------------------------------------------------------------

    [HideInInspector]
    public UnitLadderAction UnitLadderAction;
    [HideInInspector]
    public UnitChairAction UnitChairAction;

    public LadderStats LadderStats;
    public ChairStats ChairStats;

    [HideInInspector]
    public bool AIControlled;   // TO_DO

    [Header("Main Attributes")]
    //  --------------------------------------------------------------------------------
    public float MovementSpeed;

    public float AtackSpeed_Impact;
    public float AtackSpeed_ParriedOrComplete;

    public float DamageRecoverySpeed;
    public float DefenseSpeed;

    // Use this for initialization
    public void Initialize(SceneManager sceneManager)
    {
        SceneManager = sceneManager;

        // State Initialization
        UnitPrimaryState = UnitPrimaryState.Idle;

        UnitActionState = UnitActionState.None;
        UnitActionInMind = UnitActionInMind.None;

        UnitFeetState = UnitFeetState.OnGround;

        // Transforms initialization

        this.Tag = "Player"; // HARD_CODED

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "Root":
                    //Root = child;
                    //OriginalRoot = Root;
                    break;
                case "FeetCollider":    // HARD_CODED
                    child.transform.tag = this.Tag;
                    FeetCollider = child.gameObject;
                    FeetCollider.layer = 12;
                    break;
                case "Human":   // HARD_CODED
                    UnitAnimator = child.gameObject.GetComponent<Animation>();
                    break;
                default:
                    break;
            }
        }
        thisTransform = this.transform;
        thisTransform.tag = this.Tag;

        // Scripts initialization

        AIPath = thisTransform.GetComponent<AIPath>();

        UnitController = this.GetComponent<UnitController>();
        if (UnitController)
            UnitController.Initialize(this);

        UnitBasicAnimation = this.GetComponent<UnitBasicAnimation>();
        if (UnitBasicAnimation)
            UnitBasicAnimation.Initialize(this);

        UnitController.StopMoving();

        UnitActionHandler = this.GetComponent<UnitActionHandler>();
        if (UnitActionHandler)
            UnitActionHandler.Initialize(this);

        UnitActionAnimation = this.GetComponent<UnitActionAnimation>();
        if (UnitActionAnimation)
            UnitActionAnimation.Initialize(this);

        UnitLadderAction = this.GetComponent<UnitLadderAction>();
        if (UnitLadderAction)
            UnitLadderAction.Initialize(this);

        UnitChairAction = this.GetComponent<UnitChairAction>();
        if (UnitChairAction)
            UnitChairAction.Initialize(this);

        //  Target initialization
        if (!thisUnitTarget)
            AIPath.setUnitTarget(CreateTarget());
        else
            AIPath.setUnitTarget(thisUnitTarget.transform);
        thisUnitTarget.Initialize(thisTransform.gameObject.name, this);

        //  Animations Initialization
        Initialize_LadderAnimations();

        // Stats initialization
        MovementSpeed = 2f;
        AtackSpeed_Impact = 0.5f;
        AtackSpeed_ParriedOrComplete = 0.5f;

        AIPath.speed = MovementSpeed;
    }

    public void SetTeam(bool controlledByAI = false)
    {
        AIControlled = controlledByAI;
        UnitBaseAI = this.GetComponent<BaseAI>();

        //this.Tag = "Enemy"; // HARD_CODED
    }

    private Transform CreateTarget()
    {
        var targetPrefab = Resources.Load("Prefabs/UnitTarget") as GameObject;
        var createdTarget = (GameObject)Instantiate(targetPrefab, thisTransform.position, Quaternion.identity);
        thisUnitTarget = createdTarget.GetComponent<UnitTarget>();

        return createdTarget.transform;
    }

    #region Animations Setup

    [HideInInspector]
    public string Ladder_Idle;
    [HideInInspector]
    public string Ladder_Climb;
    [HideInInspector]
    public string Ladder_Climb_Down;
    [HideInInspector]
    public string Ladder_Get_On;
    [HideInInspector]
    public string Ladder_Get_On_From_Up;
    [HideInInspector]
    public string Ladder_Get_Up;
    [HideInInspector]
    public string Ladder_Get_Down;
    [HideInInspector]
    public string Ladder_Get_Down_Fast;

    private void Initialize_LadderAnimations()
    {
        Ladder_Idle = "Ladder_Idle";

        Ladder_Climb = "Ladder_Climb";

        Ladder_Climb_Down = "Ladder_Climb_Down";

        Ladder_Get_On = "Ladder_Get_On";

        Ladder_Get_On_From_Up = "Ladder_Get_On_From_Up";

        Ladder_Get_Up = "Ladder_Get_Up";

        Ladder_Get_Down = "Ladder_Get_Down";

        Ladder_Get_Down_Fast = "Ladder_Get_Down_Fast";
    }

    #endregion
}