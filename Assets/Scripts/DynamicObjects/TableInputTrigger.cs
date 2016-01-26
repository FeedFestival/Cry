using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableInputTrigger : MonoBehaviour
{

    private Table Table;

    private TableEdge TableEdge;

    private Transform thisEdgeTransform;

    public void Initialize(Table table, TableEdge tableEdge)
    {
        Table = table;

        TableEdge = tableEdge;
    }

    void OnMouseEnter()
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.LookInInventory)
        {
            if (TableEdge != TableEdge.Table_Top_Collider)
                Table.ColliderMouseState = ColliderMouseState.Hover;
            else
                Table.TableEdge = TableEdge;

            thisEdgeTransform = this.transform;

            Table.TableActionHandler.CalculateTableCursor();
        }
    }

    void OnMouseOver()
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.LookInInventory)
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
                    if (GlobalData.Player.UnitFeetState != UnitFeetState.OnTable)
                    {
                        Table.ResetUI();
                        Table.TableEdge = TableEdge;
                        Table.TableActionHandler.CalculateCircleActionPoint(thisEdgeTransform);

                        if (Input.GetMouseButtonDown((int)MouseInput.LeftClick))
                        {
                            if (Table.UI_CircleAction.CircleActionState == CircleActionState.Available)
                            {
                                Table.TableActionHandler.CalculateStartPoint();
                                GlobalData.Player.UnitActionHandler.SetAction(Table.TableProperties.thisTransform.gameObject, ActionType.GrabTable);
                            }
                        }
                    }
                }
                else
                {
                    if ((GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Idle
                        || GlobalData.Player.UnitPrimaryState == UnitPrimaryState.Walk)
                        || (GlobalData.Player.UnitActionState == UnitActionState.ClimbingTable && GlobalData.Player.UnitFeetState == UnitFeetState.OnTable))
                    {
                        Table.TableEdge = TableEdge;
                        Table.TableActionHandler.CalculateCircleActionPoint(thisEdgeTransform);  // If OnTop also calculates ExitPoint
                    }

                    if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                    {
                        if (Table.UI_CircleAction.CircleActionState == CircleActionState.Available)
                        {
                            Table.TableActionHandler.CalculateStartPoint(thisEdgeTransform);

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
