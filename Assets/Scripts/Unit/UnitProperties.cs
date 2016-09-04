using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;

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
            switch ((MovementSpeedStates)value)
            {
                case MovementSpeedStates.Calm:
                    _movementSpeed = 0.4f;
                    break;
                case MovementSpeedStates.Normal:
                    _movementSpeed = 1.8f;
                    break;
                case MovementSpeedStates.Hurry:
                    _movementSpeed = 2f;
                    break;
            }
            _unit.NavMeshAgent.speed = _movementSpeed;
        }
    }

    [HideInInspector]
    public Vector3 HeadPosition
    {
        get { return new Vector3(ThisUnitTransform.position.x, ThisUnitTransform.position.y + 1.8f, ThisUnitTransform.position.z); }
    }

    public float DamageRecoverySpeed;
    public float DefenseSpeed;

    [Header("Props")] [HideInInspector] public string ArmatureName;

    [HideInInspector] public string Tag;

    [HideInInspector] public UnitTarget ThisUnitTarget;

    private Transform _root;

    [HideInInspector]
    public Transform Root
    {
        get { return _root; }
        set
        {
            _root = value;
            if (_unit.UnitActionAnimation)
            {
                ControllerFollowRoot = (_root != null) ? true : false;
            }
        }
    }

    [HideInInspector] public Transform ThisUnitTransform;

    // The feet collider. we will disable it until we get off the event
    [HideInInspector] public GameObject FeetCollider;

    [HideInInspector] public List<GameObject> BodyParts;

    // AI
    // --------------------------------------
    public int GuardPlayerLayerMask;

    public void Initialize(Unit unit)
    {
        _unit = unit;

        BodyParts = new List<GameObject>();

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
        FeetCollider.layer = (int) Layer.UnitInteraction;
        FeetCollider.transform.parent = ThisUnitTransform;
        FeetCollider.transform.position = new Vector3(FeetCollider.transform.position.x, FeetCollider.transform.position.y + 0.2f, FeetCollider.transform.position.z);

        if (_unit.UnitType == UnitType.Player)
        {
            var visionCollider = Logic.CreateFromPrefab("Prefabs/3DComponents/FeetCollider", ThisUnitTransform.position);
            visionCollider.tag = Tag;
            visionCollider.name = "VisionCollider";
            visionCollider.layer = (int) Layer.Vision;
            visionCollider.transform.parent = ThisUnitTransform;
            visionCollider.transform.position = new Vector3(visionCollider.transform.position.x, visionCollider.transform.position.y - 0.2f, visionCollider.transform.position.z);
            visionCollider.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        ThisUnitTarget.Initialize(_unit);

        if (_unit.UnitType == UnitType.Player)
        {
            for (var i = 0; i < 4; i++)
            {
                BodyParts.Add(Logic.CreateFromPrefab("Prefabs/3DComponents/FeetCollider", ThisUnitTransform.position));
            }

            Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                switch (child.gameObject.name)
                {
                    case "Belly":
                        BodyParts[(int) BodyPart.Torso].tag = Tag;
                        BodyParts[(int) BodyPart.Torso].name = "Torso";
                        BodyParts[(int) BodyPart.Torso].layer = (int) Layer.SensoryVision;
                        BodyParts[(int) BodyPart.Torso].transform.parent = child;
                        BodyParts[(int) BodyPart.Torso].transform.localPosition = Vector3.zero;
                        BodyParts[(int) BodyPart.Torso].transform.localScale = new Vector3(0.3f, 0.4f, 0.25f);
                        break;
                    case "F_Palm_L":
                        BodyParts[(int) BodyPart.HandL].tag = Tag;
                        BodyParts[(int) BodyPart.HandL].name = "HandL";
                        BodyParts[(int) BodyPart.HandL].layer = (int) Layer.SensoryVision;
                        BodyParts[(int) BodyPart.HandL].transform.parent = child;
                        BodyParts[(int) BodyPart.HandL].transform.localPosition = Vector3.zero;
                        BodyParts[(int) BodyPart.HandL].transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
                        break;
                    case "F_Palm_R":
                        BodyParts[(int) BodyPart.HandR].tag = Tag;
                        BodyParts[(int) BodyPart.HandR].name = "HandR";
                        BodyParts[(int) BodyPart.HandR].layer = (int) Layer.SensoryVision;
                        BodyParts[(int) BodyPart.HandR].transform.parent = child;
                        BodyParts[(int) BodyPart.HandR].transform.localPosition = Vector3.zero;
                        BodyParts[(int) BodyPart.HandR].transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
                        break;
                    case "Head":
                        BodyParts[(int) BodyPart.Head].tag = Tag;
                        BodyParts[(int) BodyPart.Head].name = "Head";
                        BodyParts[(int) BodyPart.Head].layer = (int) Layer.SensoryVision;
                        BodyParts[(int) BodyPart.Head].transform.parent = child;
                        BodyParts[(int) BodyPart.Head].transform.localPosition = Vector3.zero;
                        BodyParts[(int) BodyPart.Head].transform.localScale = new Vector3(0.15f, 0.3f, 0.2f);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            GuardPlayerLayerMask = GlobalData.WallLayerMask | GlobalData.SensoryVisionLayerMask; // Shoot ray only on wallLayer or unitLayer
        }
    }
}