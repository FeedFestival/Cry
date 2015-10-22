using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

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
    public string ArmatureName;

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

        //Unit.AIPath.speed = this.MovementSpeed;

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
                case "MC":   // HARD_CODED
                    Unit.UnitAnimator = child.gameObject.GetComponent<Animation>();
                    break;
                default:
                    break;
            }
        }
        thisTransform = this.transform;
        thisTransform.tag = Tag;

        Unit.UnitAnimator["Walk"].wrapMode = WrapMode.Loop;
        Unit.UnitAnimator["Idle"].wrapMode = WrapMode.Loop;
        Unit.UnitAnimator[LadderAnimations.Idle_Ladder.ToString()].wrapMode = WrapMode.Loop;

        //  Target initialization
        CreateTarget();
        thisUnitTarget.Initialize(thisTransform.gameObject.name, Unit);
    }

    private Transform CreateTarget()
    {
        var targetPrefab = Resources.Load("Prefabs/UnitTarget") as GameObject;
        var createdTarget = (GameObject)Instantiate(targetPrefab, thisTransform.position, Quaternion.identity);
        thisUnitTarget = createdTarget.GetComponent<UnitTarget>();

        return createdTarget.transform;
    }
}
