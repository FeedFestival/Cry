using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableActionHandler : MonoBehaviour
{

    private Table Table;
    public Unit Unit;

    // Use this for initialization
    public void Initialize(Table table)
    {
        Table = table;
    }

    public void PlayActionAnimation(Unit unit = null)
    {
        if (Unit == null)
            Unit = unit;

        switch (Table.TableState)
        {
            case TableState.Static:

                Table.ResetUI();
                switch (Unit.UnitActionInMind)
                {
                    case UnitActionInMind.ClimbTable:

                        Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.Climb_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.Climb_Table);
                        break;

                    case UnitActionInMind.ClimbDownTable:

                        Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.ClimbDown_Table.ToString());
                        Table.TableAnimation.PlayStatic(TableAnimations.ClimbDown_Table);
                        break;

                    case UnitActionInMind.MovingTable:

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
                break;

            case TableState.Moving:

                switch (Unit.UnitActionInMind)
                {
                    case UnitActionInMind.DropTable:

                        Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.DropTable_FromBack.ToString());
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
}
