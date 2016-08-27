using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class Ledge : MonoBehaviour
{
    private LedgeInputTrigger LedgeInputTrigger;
    [HideInInspector]
    public LedgeActionHandler LedgeActionHandler;
    [HideInInspector]
    public LedgeAnimation LedgeAnimation;

    [HideInInspector]
    public Animation Ledge_Animator;

    private Unit _unit;
    [HideInInspector]
    public Unit Unit
    {
        get { return _unit; }
        set
        {
            _unit = value;
            if (_unit)
            {
                LedgeState = LedgeState.UnitOn;
                WallPrimitive.enabled = false;
            }
            else
            {
                LedgeState = LedgeState.Static;
                WallPrimitive.enabled = true;
            }
        }
    }

    public LedgeState LedgeState;

    public LedgeType LedgeType;
    public LedgeStartPoint LedgeStartPoint;
    public LedgeBottomPoint LedgeBottomPoint;

    private CircleActionState _circleActionState;
    public CircleActionState CircleActionState
    {
        get
        {
            return _circleActionState;
        }
        set 
        {
            _circleActionState = value;
            if (_circleActionState == CircleActionState.Available)
            {
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.None);
            }
            else if (_circleActionState == CircleActionState.None)
            {
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
            }
        }
    }

    [HideInInspector]
    public Table Table;

    // transforms
    [HideInInspector]
    public float Ypos;
    [HideInInspector]
    public float Bottom_YPos;

    [HideInInspector]
    public Vector3 Animator_Rotation;

    [HideInInspector]
    public Vector3 CircleAction_Position;
    [HideInInspector]
    public Transform thisTransform;
    [HideInInspector]
    public Vector3 StartPointPosition;
    [HideInInspector]
    public Transform Root;

    [HideInInspector]
    public GameObject Ledge_GameObject;

    private MeshCollider WallPrimitive;

    // UI
    [HideInInspector]
    public MeshRenderer UI_EdgeLine;
    [HideInInspector]
    public CircleAction UI_CircleAction;

    [HideInInspector]
    public string thisWallClimb_Name;

    // Use this for initialization
    void Start()
    {
        thisTransform = this.transform;

        var liftUpPosition = new Vector3(0, 0.01f, 0);
        var forwardLiftUp = new Vector3(0, 0, 0.01f);

        thisTransform.position = thisTransform.position + liftUpPosition;
        thisTransform.position = thisTransform.position + forwardLiftUp;

        thisWallClimb_Name = "[" + thisTransform.position.x + "," + thisTransform.position.y + "," + thisTransform.position.z + "]";

        LedgeActionHandler = thisTransform.GetComponent<LedgeActionHandler>();
        LedgeActionHandler.Initialize(this);

        LedgeAnimation = thisTransform.GetComponent<LedgeAnimation>();
        LedgeAnimation.Initialize(this);

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "WallPrimitive":
                    WallPrimitive = child.transform.GetComponent<MeshCollider>();
                    LedgeInputTrigger = child.transform.GetComponent<LedgeInputTrigger>();
                    LedgeInputTrigger.Initialize(this);
                    break;

                case "UI_EdgeLine":
                    UI_EdgeLine = child.transform.gameObject.GetComponent<MeshRenderer>();
                    break;

                case "UI_ActionCircle":
                    UI_CircleAction = child.transform.GetComponent<CircleAction>();
                    UI_CircleAction.Initialize(this);
                    break;
                default:
                    break;
            }
        }
        ResetUI();
    }

    public void ResetLedgeAction()
    {
        Unit = null;
        Ledge_Animator = null;
        Destroy(Ledge_GameObject);
        CloseActionCircle();
    }

    public void ResetUI()
    {
        if (UI_EdgeLine != null)
        {
            CloseActionCircle();
            UI_EdgeLine.enabled = false;
        }
    }

    public void CloseActionCircle()
    {
        if (CircleActionState == CircleActionState.Unavailable)
        {
            UI_CircleAction.Hide();
            return;
        }
        CircleActionState = CircleActionState.Unavailable;
        UI_CircleAction.GoUnavailable();
    }
}