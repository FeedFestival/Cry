using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableActionHandler : MonoBehaviour
{

    private Table Table;
    private Unit Unit;

    // Use this for initialization
    public void Initialize(Table table)
    {
        Table = table;
    }

    public void PlayActionAnimation(Unit unit)
    {
        Unit = unit;

        switch (Table.TableState)
        {
            case TableState.ToBeClimbed:
                switch (Table.TableStartPoint)
                {
                    case TableStartPoint.Bottom:

                        Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.Climb_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.Climb_Table);
                        break;
                    case TableStartPoint.Top:

                        Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.ClimbDown_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.ClimbDown_Table);
                        break;
                    default:
                        break;
                }
                break;

            case TableState.UnitOn:

                break;

            case TableState.ToBeMoved:

                switch (Table.TableActionStartPoint)
                {
                    case TableActionStartPoint.Table_StartPos_Forward:

                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                        Table.TableAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles + new Vector3(0, 180, 0);
                        break;

                    case TableActionStartPoint.Table_StartPos_Back:

                        Table.TableStaticAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles;
                        Table.TableAnimator.transform.eulerAngles = Table.thisTransform.eulerAngles;
                        break;

                    default:
                        break;
                }

                Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.LiftTable_FromBack.ToString());
                Table.TableAnimation.Play(TableAnimations.LiftTable_FromBack);

                break;

            default:
                break;
        }
    }

    public void PlayActionAnimation(TableAnimations action)
    {
        switch (action)
        {
            case TableAnimations.Climb_Table:
                break;
            case TableAnimations.ClimbDown_Table:
                break;
            case TableAnimations.LiftTable_FromBack:
                break;
            case TableAnimations.Idle_Table:
                break;
            case TableAnimations.DropTable_FromBack:

                Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.DropTable_FromBack.ToString());
                Table.TableAnimation.Play(TableAnimations.DropTable_FromBack);
                break;

            case TableAnimations.Move_Table:
                break;
            case TableAnimations.Rotate_Table_Left:
                break;
            case TableAnimations.Rotate_Table_Right:
                break;
            default:
                break;
        }
    }

    public void ExitMovingTableAction()
    {
        Unit.UnitActionHandler.ExitCurentAction();
    }
}
