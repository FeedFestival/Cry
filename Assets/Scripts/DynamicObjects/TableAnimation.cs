using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class TableAnimation : MonoBehaviour {

    Table Table;

	// Use this for initialization
    public void Initialize(Table table)
    {
        Table = table;
	}

    public void PlayStatic(TableAnimations animation)
    {
        Table.TableStaticAnimator.CrossFade(animation.ToString());

        float animationLenght = Table.TableStaticAnimator[animation.ToString()].length;

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    public void Play(TableAnimations animation)
    {
        Table.TableAnimator.CrossFade(animation.ToString());

        PlayStatic(animation);

        float animationLenght = Table.TableAnimator[animation.ToString()].length;

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);

        if (Table.Unit.UnitActionInMind == UnitActionInMind.DropTable)
        {
            Table.ExitTableAction();
        }
        else if (Table.Unit.UnitActionInMind == UnitActionInMind.ClimbDownTable)
        {
            Table.ExitTableAction();
        }
        else if (Table.Unit.UnitActionInMind == UnitActionInMind.ClimbTable)
        {
            Table.SetUnitOnTable();
        }
        else if (Table.Unit.UnitActionInMind == UnitActionInMind.MovingTable)    // QUESTIONABLE PLACE
        {
            Table.TableState = TableState.Moving;
        }
    }
}
