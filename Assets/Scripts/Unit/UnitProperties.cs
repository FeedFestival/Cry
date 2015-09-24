using UnityEngine;
using System.Collections;

public class UnitProperties : MonoBehaviour {

    private Unit Unit;

    [HideInInspector]
    public bool AIControlled;   // TO_DO
    [HideInInspector]
    public bool ControllerFollowRoot;

    [Header("Main Attributes")]
    public float MovementSpeed;

    public float AtackSpeed_Impact;
    public float AtackSpeed_ParriedOrComplete;

    public float DamageRecoverySpeed;
    public float DefenseSpeed;

    [Header("Props")]
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
            if (Unit.UnitActionAnimation)
            {
                if (_root != null)
                {
                    this.ControllerFollowRoot = true;
                }
                else
                {
                    //_root = OriginalRoot;
                    this.ControllerFollowRoot = false;
                }
            }
        }
    }

    [HideInInspector]
    public Transform thisTransform;

    // The feet collider. we will disable it until we get off the event
    [HideInInspector]
    public GameObject FeetCollider;

    public void Initialize(Unit unit)
    {
        Unit = unit;

        MovementSpeed = 2f;
        AtackSpeed_Impact = 0.5f;
        AtackSpeed_ParriedOrComplete = 0.5f;

        Unit.AIPath.speed = this.MovementSpeed;

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
                    child.transform.tag = Tag;
                    FeetCollider = child.gameObject;
                    FeetCollider.layer = 12;
                    break;
                case "Human":   // HARD_CODED
                    Unit.UnitAnimator = child.gameObject.GetComponent<Animation>();
                    break;
                default:
                    break;
            }
        }
        thisTransform = this.transform;
        thisTransform.tag = Tag;

        Idle = "Stealth_Idle";
        Unit.UnitAnimator[Idle].wrapMode = WrapMode.Loop;

        Walk = "Sword_Walk";
        Unit.UnitAnimator[Walk].wrapMode = WrapMode.Loop;

        //  Target initialization
        if (!thisUnitTarget)
            Unit.AIPath.setUnitTarget(CreateTarget());
        else
            Unit.AIPath.setUnitTarget(thisUnitTarget.transform);

        thisUnitTarget.Initialize(thisTransform.gameObject.name, Unit);

        Initialize_LadderAnimations();
    }

    private Transform CreateTarget()
    {
        var targetPrefab = Resources.Load("Prefabs/UnitTarget") as GameObject;
        var createdTarget = (GameObject)Instantiate(targetPrefab, thisTransform.position, Quaternion.identity);
        thisUnitTarget = createdTarget.GetComponent<UnitTarget>();

        return createdTarget.transform;
    }

    [HideInInspector]
    public string Walk;
    [HideInInspector]
    public string Idle;

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

    public void Initialize_LadderAnimations()
    {
        Ladder_Idle = "Ladder_Idle"; Unit.UnitAnimator[Ladder_Idle].wrapMode = WrapMode.PingPong;

        Ladder_Climb = "Ladder_Climb"; Unit.UnitAnimator[Ladder_Climb].wrapMode = WrapMode.PingPong;

        Ladder_Climb_Down = "Ladder_Climb_Down"; Unit.UnitAnimator[Ladder_Climb_Down].wrapMode = WrapMode.PingPong;

        Ladder_Get_On = "Ladder_Get_On"; Unit.UnitAnimator[Ladder_Get_On].wrapMode = WrapMode.PingPong;

        Ladder_Get_On_From_Up = "Ladder_Get_On_From_Up"; Unit.UnitAnimator[Ladder_Get_On_From_Up].wrapMode = WrapMode.PingPong;

        Ladder_Get_Up = "Ladder_Get_Up"; Unit.UnitAnimator[Ladder_Get_Up].wrapMode = WrapMode.PingPong;

        Ladder_Get_Down = "Ladder_Get_Down"; Unit.UnitAnimator[Ladder_Get_Down].wrapMode = WrapMode.PingPong;

        Ladder_Get_Down_Fast = "Ladder_Get_Down_Fast"; Unit.UnitAnimator[Ladder_Get_Down_Fast].wrapMode = WrapMode.PingPong;
    }
}
