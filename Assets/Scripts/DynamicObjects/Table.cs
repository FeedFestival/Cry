using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    //  Action variables
    [HideInInspector]
    public TableProperties TableProperties;
    [HideInInspector]
    public TableActionHandler TableActionHandler;
    [HideInInspector]
    public TableAnimation TableAnimation;
    [HideInInspector]
    public TableController TableController;
    [HideInInspector]
    public NavMeshObstacle NavMeshObstacle;

    // relations
    public Unit Unit;

    [HideInInspector]
    public CircleAction UI_CircleAction;

    //  Animator
    [HideInInspector]
    public Animation TableAnimator;
    [HideInInspector]
    public Animation TableStaticAnimator;

    private TableState _TableState;
    public TableState TableState
    {
        get
        {
            return _TableState;
        }
        set
        {
            _TableState = value;

            TableProperties.Table_Top_Collider.gameObject.SetActive(false);
            TableProperties.Table_End_Collider_F.gameObject.SetActive(false);
            TableProperties.Table_End_Collider.gameObject.SetActive(false);
            TableProperties.Table_Side_Collider.gameObject.SetActive(false);
            TableProperties.Table_Side_Collider_L.gameObject.SetActive(false);

            if (_TableState == TableState.Static)
            {
                if (TableController)
                    TableController.DestroyInstance();

                TableProperties.Table_End_Collider_F.gameObject.SetActive(true);
                TableProperties.Table_End_Collider.gameObject.SetActive(true);
                TableProperties.Table_Side_Collider.gameObject.SetActive(true);
                TableProperties.Table_Side_Collider_L.gameObject.SetActive(true);
            }
            else if (_TableState == TableState.Moving)
            {
                var position = Vector3.zero;
                var rotation = Vector3.zero;
                if (TableEdge == TableEdge.Table_End_Collider_F)
                {
                    position = TableProperties.Table_RotationBack.position;
                    rotation = TableProperties.thisTransform.eulerAngles;
                }
                else
                {
                    position = TableProperties.Table_RotationForward.position;

                    rotation = TableProperties.thisTransform.eulerAngles + new Vector3(0,180,0);
                }

                var tableControllerPrefab = Resources.Load("Prefabs/TableController") as GameObject;
                var createdTableController = (GameObject)Instantiate(tableControllerPrefab, position, Quaternion.identity);
                TableController = createdTableController.GetComponent<TableController>();
                TableController.Initialize(this,Unit,rotation);
            }
            else
            {
                TableProperties.Table_Top_Collider.gameObject.SetActive(false);
                TableProperties.Table_End_Collider_F.gameObject.SetActive(false);
                TableProperties.Table_End_Collider.gameObject.SetActive(false);
                TableProperties.Table_Side_Collider.gameObject.SetActive(false);
                TableProperties.Table_Side_Collider_L.gameObject.SetActive(false);
            }
        }
    }

    public TableStartPoint TableStartPoint;

    public TableEdge TableEdge;

    public ColliderMouseState ColliderMouseState;

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

    public TableActionStartPoint TableActionStartPoint;

    // Use this for initialization
    void Start()
    {
        TableProperties = this.GetComponent<TableProperties>();
        if (TableProperties)
            TableProperties.Initialize(this);

        TableActionHandler = this.GetComponent<TableActionHandler>();
        if (TableActionHandler)
            TableActionHandler.Initialize(this);

        TableAnimation = this.GetComponent<TableAnimation>();
        if (TableAnimation)
            TableAnimation.Initialize(this);

        NavMeshObstacle = this.GetComponent<NavMeshObstacle>();

        TableState = TableState.Static;

        ResetUI();
    }

    public void SetUnitOnTable()
    {
        if (!Unit.Table)
            Unit.Table = this;

        Unit.UnitPrimaryState = UnitPrimaryState.Busy;

        Unit.UnitActionState = UnitActionState.ClimbingTable;
        Unit.UnitActionInMind = UnitActionInMind.ClimbTable;

        Unit.UnitProperties.Root = TableProperties.StaticRoot;

        Unit.UnitFeetState = UnitFeetState.OnTable;
        TableProperties.Table_Top_Collider.gameObject.SetActive(true);
        TableStaticAnimator.transform.localPosition = Vector3.zero;
    }

    public void ExitTableAction(bool ledgeClimb = false)
    {
        if (ledgeClimb)
        {
            Unit.UnitActionHandler.ExitSpecificAction(ActionType.TableClimb);
        }
        else
        {
            Unit.UnitActionHandler.ExitCurentAction();
        }
        Unit.Table = null;
        Unit = null;
        TableStartPoint = TableStartPoint.OutOfReach;
    }

    public void ResetUI()
    {
        TableActionHandler._lastCirclePosition = new Vector3();

        if (ColliderMouseState == ColliderMouseState.Out)
        {
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
            if (TableProperties.UI_EdgeLines != null)
            {
                CloseActionCircle();
                ShowEdgeLines(false);
            }
        }
    }

    public void ShowEdgeLines(bool value = true)
    {
        foreach (MeshRenderer edgeLine in TableProperties.UI_EdgeLines)
        {
            edgeLine.enabled = value;
        }
    }

    public void CloseActionCircle()
    {
        if (CircleActionState != CircleActionState.Unavailable)
        {
            CircleActionState = CircleActionState.Unavailable;
            UI_CircleAction.GoUnavailable();
        }
    }
}