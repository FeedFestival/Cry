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

    private GameObject UI_TableClimb;
    private GameObject UI_TableClimbDown;


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

        groundYPos = Table.thisTransform.position.y;
        Ypos = Table.thisTransform.position.y + 1f;

        if (Table.SceneManager.PlayerStats.PlayerActionInMind != PlayerActionInMind.UseAbility)
            CalculateTableCursor();
        else
            CalculateStartPoint();
    }

    void OnMouseOver()
    {
        if (Table.SceneManager.PlayerStats.PlayerActionInMind != PlayerActionInMind.UseAbility)
        {
            CalculateStartPoint();

            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                Table.StartPointPosition = UI_TableClimb_Bottom_Position + (thisTransform.forward / 2);
                Table.TableStartPoint_Edge = this.TableEdge;

                switch (TableEdge)
                {
                    case TableEdge.Table_Side_Collider:

                        Table.TableStaticAnimator.transform.position = UI_TableClimb_Bottom_Position - (thisTransform.forward / 2);
                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                        break;

                    case TableEdge.Table_Side_Collider_L:

                        Table.TableStaticAnimator.transform.position = UI_TableClimb_Bottom_Position - (thisTransform.forward / 2);
                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles;
                        break;

                    case TableEdge.Table_End_Collider:

                        Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position - (Table.thisTransform.forward / 2);
                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, -90, 0);
                        break;

                    case TableEdge.Table_End_Collider_F:

                        Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position + (Table.thisTransform.forward / 2);
                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 90, 0);
                        break;

                    default:
                        break;
                }

                Table.SceneManager.PlayerStats.UnitActionHandler.SetAction(Table.thisTransform.gameObject, ActionType.TableClimb);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown((int)MouseInput.LeftClick))
            {
                Table.SceneManager.PlayerStats.UnitActionHandler.SetAction(Table.thisTransform.gameObject, ActionType.GrabTable);
            }
        }
    }

    void OnMouseExit()
    {
        ResetUI();
    }

    void CalculateTableCursor()
    {
        var UnitYPos = Table.SceneManager.PlayerStats.UnitProperties.thisTransform.position.y - 1;

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

    Vector3 lastPosition;

    void CalculateStartPoint()
    {
        if (Table.SceneManager.PlayerStats.PlayerActionInMind != PlayerActionInMind.UseAbility)
        {
            if (TableEdge == TableEdge.Table_Side_Collider || TableEdge == TableEdge.Table_Side_Collider_L)
            {
                lastPosition = Logic.GetEdgePosition(lastPosition, thisTransform.forward, Ypos, Ypos);

                ShowLine(lastPosition);
            }
            else
            {
                ShowLine(thisTransform.position);
            }
        }
        else
        {
            var playerPos = Table.SceneManager.PlayerStats.UnitProperties.thisTransform.position;

            float[] distances = new float[2];
            distances[(int)TableActionStartPoint.Table_StartPos_Forward] = Vector3.Distance(playerPos, Table.Table_StartPos_Forward);
            distances[(int)TableActionStartPoint.Table_StartPos_Back] = Vector3.Distance(playerPos, Table.Table_StartPos_Back);

            Table.TableActionStartPoint = (TableActionStartPoint)Logic.GetSmallestDistance(distances);

        }
    }

    private void ShowLine(Vector3 pos)
    {
        UI_TableClimb_Top_Position = new Vector3(pos.x, Ypos, pos.z);
        UI_TableClimb_Bottom_Position = new Vector3(pos.x, groundYPos, pos.z);

        if (Table.TableStartPoint == TableStartPoint.Bottom)
        {
            if (UI_TableClimb == null)
            {
                UI_TableClimb = Logic.InstantiateEdgeUI(UI_TableClimb_Bottom_Position, thisTableCLimb_Rotation, new Vector3(0.32f, 0.8f, 0.8f), "TableClimb" + Table.thisTableClimb_Name);
            }
            UI_TableClimb.transform.position = UI_TableClimb_Top_Position;
        }
        else
        {
            if (UI_TableClimbDown == null)
            {
                UI_TableClimbDown = Logic.InstantiateEdgeUI(UI_TableClimb_Bottom_Position, thisTableCLimb_Rotation, new Vector3(0.32f, 0.8f, 0.8f), "TableClimbDown" + Table.thisTableClimb_Name);
            }
            UI_TableClimbDown.transform.position = UI_TableClimb_Bottom_Position;
        }
    }

    public void ResetUI()
    {
        if (UI_TableClimb != null)
        {
            Destroy(UI_TableClimb);
            UI_TableClimb = null;
        }
        if (UI_TableClimbDown != null)
        {
            Destroy(UI_TableClimbDown);
            UI_TableClimbDown = null;
        }
    }
}
