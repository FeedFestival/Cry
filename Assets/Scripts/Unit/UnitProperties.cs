using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitProperties : MonoBehaviour
{
    private Unit _unit;

    [HideInInspector]
    public bool ControllerFollowRoot;

    [Header("Main Attributes")]

    private float _movementSpeed;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            _movementSpeed = value;
            _unit.NavMeshAgent.speed = _movementSpeed;
        }
    }

    public float DamageRecoverySpeed;
    public float DefenseSpeed;

    [Header("Props")]

    [HideInInspector]
    public string ArmatureName;

    [HideInInspector]
    public string Tag;

    [HideInInspector]
    public UnitTarget ThisUnitTarget;

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
            if (_unit.UnitActionAnimation)
            {
                ControllerFollowRoot = (_root != null) ? true : false;
            }
        }
    }

    [HideInInspector]
    public Transform ThisUnitTransform;

    // The feet collider. we will disable it until we get off the event
    [HideInInspector]
    public GameObject FeetCollider;

    public void Initialize(Unit unit)
    {
        _unit = unit;

        MovementSpeed = 1.8f;
        
        Tag = _unit.UnitType.ToString();

        ThisUnitTransform = transform;
        ThisUnitTransform.tag = Tag;

        ThisUnitTarget = Logic.CreateFromPrefab("Prefabs/UnitTarget", ThisUnitTransform.position).GetComponent<UnitTarget>();
        ThisUnitTarget.gameObject.name = _unit.gameObject.name + "Target";
        ThisUnitTarget.gameObject.layer = 11;

        FeetCollider = Logic.CreateFromPrefab("Prefabs/3DComponents/FeetCollider", ThisUnitTransform.position);
        FeetCollider.tag = Tag;
        FeetCollider.name = "FeetCollider";
        FeetCollider.layer = 12;
        FeetCollider.transform.parent = ThisUnitTransform;
        FeetCollider.transform.position = new Vector3(FeetCollider.transform.position.x, FeetCollider.transform.position.y + 0.2f, FeetCollider.transform.position.z);

        var visionCollider = Logic.CreateFromPrefab("Prefabs/3DComponents/FeetCollider", ThisUnitTransform.position);
        visionCollider.name = "VisionCollider";
        visionCollider.layer = 0;
        visionCollider.transform.parent = ThisUnitTransform;
        visionCollider.transform.position = new Vector3(visionCollider.transform.position.x, visionCollider.transform.position.y - 0.2f, visionCollider.transform.position.z);
        visionCollider.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        ThisUnitTarget.Initialize(_unit);
    }
}