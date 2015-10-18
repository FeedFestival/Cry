using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableInputTrigger : MonoBehaviour
{

    private Table Table;

    private TableEdge TableEdge;

    private Transform thisTransform;

    public void Initialize(Table table, TableEdge tableEdge)
    {
        Table = table;

        TableEdge = tableEdge;
    }

    void OnMouseEnter()
    {
        if (TableEdge != TableEdge.Table_Top_Collider)
            Table.ColliderMouseState = ColliderMouseState.Hover;
        else
            Table.TableEdge = TableEdge;

        thisTransform = this.transform;

        Table.TableActionHandler.CalculateTableCursor();
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
                Table.ResetUI();
                Table.TableEdge = TableEdge;
                Table.TableActionHandler.CalculateCircleActionPoint(thisTransform);

                if (Input.GetMouseButtonDown((int)MouseInput.LeftClick))
                {
                    Table.TableActionHandler.CalculateStartPoint();
                    GlobalData.Player.UnitActionHandler.SetAction(Table.TableProperties.thisTransform.gameObject, ActionType.GrabTable);
                }
            }
            else
            {
                if ((GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Idle
                    || GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Walk)
                    || (GlobalData.Player.UnitActionState == UnitActionState.ClimbingTable && GlobalData.Player.UnitFeetState == UnitFeetState.OnTable))
                {
                    Table.TableEdge = TableEdge;
                    Table.TableActionHandler.CalculateCircleActionPoint(thisTransform);  // If OnTop also calculates ExitPoint
                }

                if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                {
                    Table.TableActionHandler.CalculateStartPoint(thisTransform);

                    if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable)
                    {
                        Table.Unit.UnitActionInMind = UnitActionInMind.ClimbDownTable;

                        GlobalData.Player.UnitController.SetPathToTarget(Table.TableProperties.StartPointPosition);
                    }
                    else // if UnitFeetState.OnGround
                    {
                        GlobalData.Player.UnitActionHandler.SetAction(Table.TableProperties.thisTransform.gameObject, ActionType.TableClimb);
                    }
                }
            }
        }
    }

    void OnMouseExit()
    {
        Table.ColliderMouseState = ColliderMouseState.Out;
        StartCoroutine(Wait(0.1f));    // Wait a bit before closing the CircleAction_UI
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Table.ResetUI();
    }
}
