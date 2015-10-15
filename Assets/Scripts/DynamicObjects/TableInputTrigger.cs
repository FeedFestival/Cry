using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableInputTrigger : MonoBehaviour
{

    private Table Table;

    private TableEdge TableEdge;

    private Transform thisTransform;

    private float groundYPos;
    private float Ypos;

    private Vector3 UI_TableClimb_Bottom_Position;
    private Vector3 UI_TableClimb_Top_Position;

    private Vector3 thisTableCLimb_Rotation;
    private Vector3 thisTableCLimbDown_Rotation;

    public void Initialize(Table table, TableEdge tableEdge)
    {
        Table = table;

        TableEdge = tableEdge;
    }

    void OnMouseEnter()
    {
        thisTransform = this.transform;
        thisTableCLimb_Rotation = thisTransform.eulerAngles;
        thisTableCLimb_Rotation.Set(270, thisTableCLimb_Rotation.y, thisTableCLimb_Rotation.z);
        thisTableCLimbDown_Rotation = thisTransform.eulerAngles;
        thisTableCLimbDown_Rotation.Set(270, thisTableCLimbDown_Rotation.y + 180, thisTableCLimbDown_Rotation.z);

        groundYPos = Table.thisTransform.position.y;
        Ypos = Table.thisTransform.position.y + 1f;

        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
            CalculateTableCursor();
    }

    void OnMouseOver()
    {
        if (TableEdge == TableEdge.Table_Top_Collider)
        {
            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                var pos = Logic.GetPointHitAtMousePosition();
                if (pos != Vector3.zero)
                    GlobalData.Player.UnitController.SetPathToTarget(pos);
            }
        }
        else
        {
            if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
            {
                if (GlobalData.CameraControl.CameraCursor.lastCursor == CursorType.Grab)
                {
                    Table.ResetUI();
                    if (Input.GetMouseButtonDown((int)MouseInput.LeftClick))
                    {
                        CalculateStartPoint();
                        GlobalData.Player.UnitActionHandler.SetAction(Table.thisTransform.gameObject, ActionType.GrabTable);
                    }
                }
            }
            else
            {
                if ((GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Idle 
                    || GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Walk)
                    || (GlobalData.Player.UnitActionState == UnitActionState.ClimbingTable && GlobalData.Player.UnitFeetState == UnitFeetState.OnTable))
                    CalculateStartPoint();  // If OnTop also calculates ExitPoint

                if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                {
                    if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable)
                    {
                        if (TableEdge == TableEdge.Table_Side_Collider || TableEdge == TableEdge.Table_Side_Collider_L)
                        {
                            Table.StartPointPosition = UI_TableClimb_Top_Position - (thisTransform.forward / 2);
                        }
                        else if (TableEdge == TableEdge.Table_End_Collider)
                        {
                            Table.StartPointPosition = Table.Table_BackExit;
                        }
                        else
                        {
                            Table.StartPointPosition = Table.Table_ForwardExit;
                        }
                        PlaceAnimator();
                        CalculateRotationPoint();

                        Table.Unit.UnitActionInMind = UnitActionInMind.ClimbDownTable;

                        GlobalData.Player.UnitController.SetPathToTarget(Table.StartPointPosition);
                    }
                    else // if UnitFeetState.OnGround
                    {
                        Table.StartPointPosition = UI_TableClimb_Bottom_Position + (thisTransform.forward / 2);

                        PlaceAnimator();
                        CalculateRotationPoint();

                        GlobalData.Player.UnitActionHandler.SetAction(Table.thisTransform.gameObject, ActionType.TableClimb);
                    }
                }
            }
        }
    }

    void OnMouseExit()
    {
        Table.ResetUI();
    }

    void CalculateTableCursor()
    {
        var UnitYPos = GlobalData.Player.UnitProperties.thisTransform.position.y - 1;

        Table.TableStartPoint = (TableStartPoint)Logic.GetClosestYPos(UnitYPos, groundYPos, Ypos);

        switch (Table.TableStartPoint)
        {
            case TableStartPoint.Bottom:
                // Change cursor to wall climb DOWN cursor
                break;
            case TableStartPoint.Top:
                // Change cursor to wall climb cursor
                break;
            case TableStartPoint.OutOfReach:
                // Change cursor to Default
                break;
            default:
                break;
        }
    }

    private Vector3 lastPosition;
    private void CalculateStartPoint()
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
        {
            if (TableEdge == TableEdge.Table_Side_Collider || TableEdge == TableEdge.Table_Side_Collider_L)
            {
                lastPosition = Logic.GetEdgePosition(lastPosition, thisTransform.forward, Ypos, Ypos);

                if (lastPosition != Vector3.zero)
                    ShowLine(lastPosition);
            }
            else
            {
                ShowLine(thisTransform.position);
            }
        }
        else
        {
            var playerPos = GlobalData.Player.UnitProperties.thisTransform.position;

            float[] distances = new float[2];
            distances[(int)TableActionStartPoint.Table_StartPos_Forward] = Vector3.Distance(playerPos, Table.Table_StartPos_Forward);
            distances[(int)TableActionStartPoint.Table_StartPos_Back] = Vector3.Distance(playerPos, Table.Table_StartPos_Back);

            Table.TableActionStartPoint = (TableActionStartPoint)Logic.GetSmallestDistance(distances);

        }
    }
    private void CalculateRotationPoint()
    {
        switch (TableEdge)
        {
            case TableEdge.Table_Side_Collider:

                Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                break;

            case TableEdge.Table_Side_Collider_L:

                Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles;
                break;

            case TableEdge.Table_End_Collider:

                Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, -90, 0);
                break;

            case TableEdge.Table_End_Collider_F:

                Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 90, 0);
                break;

            default:
                break;
        }
    }
    private void PlaceAnimator()
    {
        switch (TableEdge)
        {
            case TableEdge.Table_Side_Collider:

                Table.TableStaticAnimator.transform.position = UI_TableClimb_Bottom_Position - (thisTransform.forward / 2);
                break;

            case TableEdge.Table_Side_Collider_L:

                Table.TableStaticAnimator.transform.position = UI_TableClimb_Bottom_Position - (thisTransform.forward / 2);
                break;

            case TableEdge.Table_End_Collider:

                Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position - (Table.thisTransform.forward / 2);
                break;

            case TableEdge.Table_End_Collider_F:

                Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position + (Table.thisTransform.forward / 2);
                break;

            default:
                break;
        }
    }

    private void ShowLine(Vector3 pos)
    {
        UI_TableClimb_Top_Position = new Vector3(pos.x, Ypos, pos.z);
        UI_TableClimb_Bottom_Position = new Vector3(pos.x, groundYPos, pos.z);
        if (Table.TableStartPoint == TableStartPoint.Bottom)
        {
            if (Table.UI_TableClimb == null)
            {
                Table.UI_TableClimb = Logic.InstantiateEdgeUI(UI_TableClimb_Bottom_Position, thisTableCLimb_Rotation, new Vector3(0.32f, 0.8f, 0.8f), "TableClimb" + Table.thisTableClimb_Name);
            }
            Table.UI_TableClimb.transform.position = UI_TableClimb_Top_Position;
        }
        else
        {
            if (Table.UI_TableClimbDown == null)
            {
                Table.UI_TableClimbDown = Logic.InstantiateEdgeUI(UI_TableClimb_Bottom_Position, thisTableCLimbDown_Rotation, new Vector3(0.32f, 0.8f, 0.8f), "TableClimbDown" + Table.thisTableClimb_Name);
            }
            Table.UI_TableClimbDown.transform.position = UI_TableClimb_Bottom_Position;
        }
    }
}
