using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Ledge : MonoBehaviour
{
    private LedgeInputTrigger LedgeInputTrigger;
    [HideInInspector]
    public LedgeActionHandler LedgeActionHandler;
    [HideInInspector]
    public LedgeAnimation LedgeAnimation;

    [HideInInspector]
    public Animation Ledge_Animator;

    [HideInInspector]
    public Unit Unit;

    public LedgeType LedgeType;
    public LedgeStartPoint LedgeStartPoint;

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

        var liftUpPosition = new Vector3(0,0.01f,0);
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

                    LedgeInputTrigger = child.transform.GetComponent<LedgeInputTrigger>();
                    LedgeInputTrigger.Initialize(this);
                    break;

                case "UI_EdgeLine":
                    UI_EdgeLine = child.transform.gameObject.GetComponent<MeshRenderer>();
                    break;

                case "UI_CircleAction":
                    UI_CircleAction = child.transform.GetComponent<CircleAction>();
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
        LedgeActionHandler.CalculateCircleAction_State();
    }

    public void ResetUI()
    {
        if (GlobalData.CameraControl)
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
        if (UI_EdgeLine != null)
        {
            LedgeActionHandler.CalculateCircleAction_State();
            UI_EdgeLine.enabled = false;
        }
    }
}
