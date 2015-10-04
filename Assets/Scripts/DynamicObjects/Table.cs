﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Table : MonoBehaviour
{
    public SceneManager SceneManager;

    [HideInInspector]
    public string thisTableClimb_Name;

    // Transforms
    [HideInInspector]
    public Transform thisTransform;

    [HideInInspector]
    public Vector3 Table_BackExit;
    [HideInInspector]
    public Vector3 Table_CenterExit;
    [HideInInspector]
    public Vector3 Table_ForwardExit;

    [HideInInspector]
    public Vector3 Table_RotationBack;
    [HideInInspector]
    public Vector3 Table_RotationForward;

    [HideInInspector]
    public Vector3 Table_StartPos_Forward;
    [HideInInspector]
    public Vector3 Table_StartPos_Back;

    [HideInInspector]
    public Vector3 StartPointPosition;

    //  Animator
    [HideInInspector]
    public Animation TableAnimator;
    [HideInInspector]
    public Animation TableStaticAnimator;

    [HideInInspector]
    public Transform Root;
    [HideInInspector]
    public Transform StaticRoot;

    // TableState.ToBeClimbed
    [HideInInspector]
    public TableInputTrigger Table_Top_Collider;

    private TableInputTrigger Table_End_Collider_F;
    private TableInputTrigger Table_End_Collider;
    private TableInputTrigger Table_Side_Collider;
    private TableInputTrigger Table_Side_Collider_L;

    //  Action variables
    [HideInInspector]
    public TableActionHandler TableActionHandler;
    [HideInInspector]
    public TableAnimation TableAnimation;

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
            if (_TableState == TableState.Static)
            {
                Table_Top_Collider.gameObject.SetActive(false);
                Table_End_Collider_F.gameObject.SetActive(true);
                Table_End_Collider.gameObject.SetActive(true);
                Table_Side_Collider.gameObject.SetActive(true);
                Table_Side_Collider_L.gameObject.SetActive(true);
            }
            else
            {
                Table_Top_Collider.gameObject.SetActive(false);
                Table_End_Collider_F.gameObject.SetActive(false);
                Table_End_Collider.gameObject.SetActive(false);
                Table_Side_Collider.gameObject.SetActive(false);
                Table_Side_Collider_L.gameObject.SetActive(false);
            }
        }
    }

    public TableStartPoint TableStartPoint;
    public TableEdge TableStartPoint_Edge;

    public TableActionStartPoint TableActionStartPoint;

    // Use this for initialization
    void Start()
    {
        // Transforms initialization
        thisTransform = this.transform;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "Table_BackExit":
                    Table_BackExit = child.transform.position;
                    break;
                case "Table_ForwardExit":
                    Table_ForwardExit = child.transform.position;
                    break;

                case "Table_RotationBack":
                    Table_RotationBack = child.transform.position;
                    break;
                case "Table_RotationForward":
                    Table_RotationForward = child.transform.position;
                    break;

                case "Table_StartPos_Forward":
                    Table_StartPos_Forward = child.transform.position;
                    break;
                case "Table_StartPos_Back":
                    Table_StartPos_Back = child.transform.position;
                    break;

                case "Table_Top_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Top_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Top_Collider.Initialize(this, TableEdge.Table_Top_Collider);
                    break;
                case "Table_End_Collider_F":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_End_Collider_F = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_End_Collider_F.Initialize(this, TableEdge.Table_End_Collider_F);
                    break;
                case "Table_End_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_End_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_End_Collider.Initialize(this, TableEdge.Table_End_Collider);
                    break;
                case "Table_Side_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Side_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Side_Collider.Initialize(this, TableEdge.Table_Side_Collider);
                    break;
                case "Table_Side_Collider_L":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Side_Collider_L = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Side_Collider_L.Initialize(this, TableEdge.Table_Side_Collider_L);
                    break;

                case "TableAnimator":

                    TableAnimator = child.GetComponent<Animation>();
                    break;

                case "TableStaticAnimator":

                    TableStaticAnimator = child.GetComponent<Animation>();
                    break;

                case "Root":

                    if (child.transform.parent.transform.parent.transform.gameObject.name == "TableStaticArmature")
                        StaticRoot = child.transform;
                    else
                        Root = child.transform;
                    break;

                default:
                    break;
            }
        }

        TableActionHandler = this.GetComponent<TableActionHandler>();
        if (TableActionHandler)
            TableActionHandler.Initialize(this);

        TableAnimation = this.GetComponent<TableAnimation>();
        if (TableAnimation)
            TableAnimation.Initialize(this);

        TableState = TableState.Static;

        thisTableClimb_Name = "[" + thisTransform.position.x + "," + thisTransform.position.y + "," + thisTransform.position.z + "]";
    }


}
