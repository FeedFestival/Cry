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

        switch (Table.TableStartPoint)
        {
            case TableStartPoint.Bottom:

                Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.Climb_Table.ToString());
                PlayStatic(TableAnimations.Climb_Table.ToString());
                break;
            case TableStartPoint.Top:

                Unit.UnitActionAnimation.PlaySingleAnimation(TableAnimations.ClimbDown_Table.ToString());
                PlayStatic(TableAnimations.ClimbDown_Table.ToString());
                break;
            default:
                break;
        }
    }

    private void PlayStatic(string animationString)
    {
        Table.TableStaticAnimator.CrossFade(animationString);

        float animationLenght = Table.TableStaticAnimator[animationString].length;
        //if (debuging)
        //    Debug.Log("an - " + animationString + " , langht = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);
        //Unit.UnitActionHandler.ExitCurentAction();
    }
}
