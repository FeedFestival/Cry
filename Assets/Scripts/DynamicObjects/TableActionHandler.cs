using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class TableActionHandler : MonoBehaviour
{
    private Table Table;

    //[HideInInspector]
    public Vector3 _lastCirclePosition = new Vector3();

    // Use this for initialization
    public void Initialize(Table table)
    {
        Table = table;
    }

    public void PlayActionAnimation(Unit unit = null)
    {
        if (Table.Unit == null)
            Table.Unit = unit;

        switch (Table.TableState)
        {
            case TableState.Static:

                Table.ResetUI();
                switch (Table.Unit.UnitActionInMind)
                {
                    case UnitActionInMind.ClimbTable:

                        Table.Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.Climb_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.Climb_Table);
                        break;

                    case UnitActionInMind.ClimbDownTable:

                        Table.Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.ClimbDown_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.ClimbDown_Table);
                        break;

                    case UnitActionInMind.MovingTable:

                        Table.Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.LiftTable_FromBack.ToString());
                        Table.TableAnimation.Play(TableAnimations.LiftTable_FromBack);

                        break;

                    default:
                        break;
                }
                break;

            case TableState.Moving:

                switch (Table.Unit.UnitActionInMind)
                {
                    case UnitActionInMind.DropTable:

                        Table.Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.DropTable_FromBack.ToString());
                        Table.TableAnimation.Play(TableAnimations.DropTable_FromBack);
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }

    public void CalculateTableCursor()
    {
        Table.TableProperties.Bottom_YPos = Table.TableProperties.thisTransform.position.y;
        Table.TableProperties.Ypos = Table.TableProperties.thisTransform.position.y + 1f;

        var UnitYPos = GlobalData.Player.UnitProperties.ThisUnitTransform.position.y;

        Table.TableStartPoint = (TableStartPoint)Logic.GetClosestYPos(UnitYPos, Table.TableProperties.Bottom_YPos, Table.TableProperties.Ypos);

        Table.ShowEdgeLines();

        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
            if (Table.TableEdge != TableEdge.Table_Top_Collider)
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.None);
            else
                GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }

    public void CalculateCircleActionPoint(Transform edgeTransform)
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
        {
            if (Table.TableEdge == TableEdge.Table_Side_Collider || Table.TableEdge == TableEdge.Table_Side_Collider_L)
            {
                //var tablePosition = new Vector3(edgeTransform.position.x,)
                var hitPosition = Logic.GetEdgePosition(Table.TableProperties.CircleAction_Position,
                                                        edgeTransform, Table.TableProperties.Ypos);

                if (_lastCirclePosition != hitPosition && hitPosition != Vector3.zero)
                {
                    _lastCirclePosition = hitPosition;
                    _showCircleAction(_lastCirclePosition, edgeTransform);
                }
            }
            else
            {
                if (Table.TableStartPoint == TableStartPoint.Bottom)
                {
                    if (_lastCirclePosition != edgeTransform.position)
                    {
                        _lastCirclePosition = edgeTransform.position;
                        _showCircleAction(edgeTransform.position, edgeTransform);
                    }
                }
                else
                {
                    _lastCirclePosition = edgeTransform.position;
                    _showCircleAction(edgeTransform.position, edgeTransform);
                }
            }
        }
        else
        {
            if (Table.TableEdge == TableEdge.Table_Side_Collider || Table.TableEdge == TableEdge.Table_Side_Collider_L)
            {
                var hitPosition = Logic.GetEdgePosition(Table.TableProperties.CircleAction_Position,
                                                        edgeTransform, Table.TableProperties.Ypos);

                if (hitPosition != Vector3.zero)
                {
                    float[] distances = new float[2];
                    distances[(int)TableActionStartPoint.Table_StartPos_Forward] = Vector3.Distance(hitPosition, Table.TableProperties.Table_StartPos_Forward.position);
                    distances[(int)TableActionStartPoint.Table_StartPos_Back] = Vector3.Distance(hitPosition, Table.TableProperties.Table_StartPos_Back.position);

                    Table.TableActionStartPoint = (TableActionStartPoint)Logic.GetSmallestDistance(distances);

                    if (Table.TableActionStartPoint == TableActionStartPoint.Table_StartPos_Forward)
                    {
                        Table.TableEdge = TableEdge.Table_End_Collider_F;
                        CalculateCircleActionPoint(Table.TableProperties.Table_End_Collider_F.transform);
                    }
                    else
                    {
                        Table.TableEdge = TableEdge.Table_End_Collider;
                        CalculateCircleActionPoint(Table.TableProperties.Table_End_Collider.transform);
                    }
                }
            }
            else
            {
                if (_lastCirclePosition != edgeTransform.position)
                {
                    _lastCirclePosition = edgeTransform.position;
                    _showCircleAction(_lastCirclePosition, edgeTransform);

                    if (Table.TableEdge == TableEdge.Table_End_Collider_F)
                        Table.TableActionStartPoint = TableActionStartPoint.Table_StartPos_Forward;
                    else
                        Table.TableActionStartPoint = TableActionStartPoint.Table_StartPos_Back;
                }
            }
        }
    }

    void _setPositions(Vector3 pos)
    {
        Table.TableProperties.Animator_TopPosition = new Vector3(pos.x, Table.TableProperties.Ypos, pos.z);
        Table.TableProperties.Animator_BottomPosition = new Vector3(pos.x, Table.TableProperties.Bottom_YPos, pos.z);

        Table.TableProperties.CircleAction_Position = new Vector3(pos.x, Table.TableProperties.Ypos, pos.z);
    }
    void _showCircleAction(Vector3 pos, Transform edgeTransform)
    {
        if (Table.TableStartPoint == TableStartPoint.Top)
        {
            _setPositions(pos);
            _calculateBottomPoint(pos, edgeTransform);
        }
        else if (Table.TableStartPoint == TableStartPoint.Bottom)
        {
            if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable || GlobalData.Player.UnitFeetState == UnitFeetState.OnGround)
            {
                _setPositions(pos);
                _calculateBottomPoint(pos, edgeTransform);
            }
        }
    }

    public void CalculateStartPoint(Transform edgeTransform = null)
    {
        if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
        {
            if (Table.TableActionStartPoint == TableActionStartPoint.Table_StartPos_Forward)
            {
                Table.TableProperties.StartPointPosition = Table.TableProperties.Table_StartPos_Forward.position;
            }
            else
            {
                Table.TableProperties.StartPointPosition = Table.TableProperties.Table_StartPos_Back.position;
            }
        }
        else
        {
            if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable)
            {
                if (Table.TableEdge == TableEdge.Table_Side_Collider || Table.TableEdge == TableEdge.Table_Side_Collider_L)
                {
                    Table.TableProperties.StartPointPosition = Table.TableProperties.Animator_TopPosition - (edgeTransform.forward / 2);
                }
                else if (Table.TableEdge == TableEdge.Table_End_Collider)
                {
                    Table.TableProperties.StartPointPosition = Table.TableProperties.Table_BackExit.position;
                }
                else
                {
                    Table.TableProperties.StartPointPosition = Table.TableProperties.Table_ForwardExit.position;
                }
            }
            else // if UnitFeetState.OnGround
            {
                Table.TableProperties.StartPointPosition = Table.TableProperties.Animator_BottomPosition + (edgeTransform.forward / 2);
            }
        }
        _placeAnimator(edgeTransform);
    }

    void _placeAnimator(Transform edgeTransform)
    {
        switch (Table.TableEdge)
        {
            case TableEdge.Table_Side_Collider:

                Table.TableStaticAnimator.transform.position = Table.TableProperties.Animator_BottomPosition - (edgeTransform.forward / 2);
                Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                break;

            case TableEdge.Table_Side_Collider_L:

                Table.TableStaticAnimator.transform.position = Table.TableProperties.Animator_BottomPosition - (edgeTransform.forward / 2);
                Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles;
                break;

            case TableEdge.Table_End_Collider:

                if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
                {
                    Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles;
                    Table.TableAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles;
                }
                else
                {
                    Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position - (Table.TableProperties.thisTransform.forward / 2);
                    Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles + new Vector3(0, -90, 0);
                }
                break;

            case TableEdge.Table_End_Collider_F:

                if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.UseAbility)
                {
                    Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                    Table.TableAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                }
                else
                {
                    Table.TableStaticAnimator.transform.position = Table.TableStaticAnimator.transform.position + (Table.TableProperties.thisTransform.forward / 2);
                    Table.TableStaticAnimator.transform.eulerAngles = Table.TableProperties.thisTransform.eulerAngles + new Vector3(0, 90, 0);
                }
                break;

            default:
                break;
        }
    }

    void _calculateBottomPoint(Vector3 circlePosition, Transform edgeTransform)
    {
        var distanceThreshold = 1.5f;

        var pos1 = Table.TableProperties.CircleAction_Position + (edgeTransform.forward / distanceThreshold);

        var _bottom_Position = new Vector3(Table.TableProperties.CircleAction_Position.x,
                                            Table.TableProperties.Bottom_YPos - 0.5f,
                                            Table.TableProperties.CircleAction_Position.z);
        var pos2 = _bottom_Position + (edgeTransform.forward / distanceThreshold);

        var direction = Logic.GetDirection(pos1, pos2);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(pos1, direction), out hit, 2.5f))
        {
            Debug.DrawRay(pos1, direction);
            if (hit.transform.gameObject.tag == "Map")
            {
                Table.CircleActionState = CircleActionState.Available;
                Table.UI_CircleAction.GoAvailable();
            }
            else if (hit.transform.gameObject.tag == "Player") // We should avoid player collider // FOR_NOW
            {
                Table.CircleActionState = CircleActionState.Available;
                Table.UI_CircleAction.GoAvailable();
            }
            else
            {
                Table.CircleActionState = CircleActionState.Unavailable;
                Table.UI_CircleAction.GoUnavailable();
            }
        }
        else
        {
            Table.CircleActionState = CircleActionState.Unavailable;
            Table.UI_CircleAction.GoUnavailable();
        }
    }
}
