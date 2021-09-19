using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;

public class TableProperties : MonoBehaviour
{
    private Table Table;

    [HideInInspector]
    public string thisTableClimb_Name;

    // Transforms
    [HideInInspector]
    public Transform thisTransform;

    [HideInInspector]
    public Transform Table_BackExit;
    [HideInInspector]
    public Transform Table_ForwardExit;

    [HideInInspector]
    public Transform Table_RotationBack;
    [HideInInspector]
    public Transform Table_RotationForward;

    [HideInInspector]
    public Transform Table_StartPos_Forward;
    [HideInInspector]
    public Transform Table_StartPos_Back;

    [HideInInspector]
    public Vector3 StartPointPosition;

    [HideInInspector]
    public Vector3 Animator_BottomPosition;
    [HideInInspector]
    public Vector3 Animator_TopPosition;

    // UI
    [HideInInspector]
    public List<MeshRenderer> UI_EdgeLines;

    private Vector3 _circleAction_Position;
    [HideInInspector]
    public Vector3 CircleAction_Position
    {
        get { return _circleAction_Position; }
        set
        {
            _circleAction_Position = value;
            Table.UI_CircleAction.thisTransform.position = CircleAction_Position;
        }
    }

    // Transforms variables
    [HideInInspector]
    public float Bottom_YPos;
    [HideInInspector]
    public float Ypos;

    [HideInInspector]
    public Transform Root;
    [HideInInspector]
    public Transform StaticRoot;

    // Coliders
    [HideInInspector]
    public TableInputTrigger Table_Top_Collider;
    [HideInInspector]
    public TableInputTrigger Table_End_Collider_F;
    [HideInInspector]
    public TableInputTrigger Table_End_Collider;
    [HideInInspector]
    public TableInputTrigger Table_Side_Collider;
    [HideInInspector]
    public TableInputTrigger Table_Side_Collider_L;

    public void Initialize(Table table)
    {
        Table = table;

        // Transforms initialization
        thisTransform = this.transform;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "Table_BackExit":
                    Table_BackExit = child.transform;
                    break;
                case "Table_ForwardExit":
                    Table_ForwardExit = child.transform;
                    break;

                case "Table_Rotation_Back":
                    Table_RotationBack = child.transform;
                    break;
                case "Table_Rotation_Forward":
                    Table_RotationForward = child.transform;
                    break;

                case "Table_StartPos_Forward":
                    Table_StartPos_Forward = child.transform;
                    break;
                case "Table_StartPos_Back":
                    Table_StartPos_Back = child.transform;
                    break;

                case "Table_Top_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Top_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Top_Collider.Initialize(Table, TableEdge.Table_Top_Collider);
                    break;
                case "Table_End_Collider_F":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_End_Collider_F = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_End_Collider_F.Initialize(Table, TableEdge.Table_End_Collider_F);
                    break;
                case "Table_End_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_End_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_End_Collider.Initialize(Table, TableEdge.Table_End_Collider);
                    break;
                case "Table_Side_Collider":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Side_Collider = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Side_Collider.Initialize(Table, TableEdge.Table_Side_Collider);
                    break;
                case "Table_Side_Collider_L":
                    child.gameObject.AddComponent<TableInputTrigger>();
                    Table_Side_Collider_L = child.gameObject.GetComponent<TableInputTrigger>();
                    Table_Side_Collider_L.Initialize(Table, TableEdge.Table_Side_Collider_L);
                    break;

                case "UI_EdgeLine":
                    if (UI_EdgeLines == null)
                        UI_EdgeLines = new List<MeshRenderer>();
                    UI_EdgeLines.Add(child.gameObject.GetComponent<MeshRenderer>());
                    break;

                case "UI_ActionCircle":
                    Table.UI_CircleAction = child.transform.GetComponent<CircleAction>();
                    Table.UI_CircleAction.Initialize(Table);
                    break;

                case "TableAnimator":

                    Table.TableAnimator = child.GetComponent<Animation>();
                    break;

                case "TableStaticAnimator":

                    Table.TableStaticAnimator = child.GetComponent<Animation>();
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

        thisTableClimb_Name = "[" + thisTransform.position.x + "," + thisTransform.position.y + "," + thisTransform.position.z + "]";
    }
}