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

        if (Table.TableActionHandler.Unit.UnitActionInMind == UnitActionInMind.DropTable)
        {
            Table.TableActionHandler.Unit.UnitActionHandler.ExitCurentAction();
        }
        else if (Table.TableActionHandler.Unit.UnitActionInMind == UnitActionInMind.ClimbDownTable)
        {
            Table.TableActionHandler.Unit.UnitActionHandler.ExitCurentAction();
        }
        else if (Table.TableActionHandler.Unit.UnitActionInMind == UnitActionInMind.ClimbTable)
        {
            Table.TableActionHandler.Unit.UnitFeetState = UnitFeetState.OnTable;
            Table.Table_Top_Collider.gameObject.SetActive(true);
            Table.TableStaticAnimator.transform.localPosition = Vector3.zero;
        }
        else if (Table.TableActionHandler.Unit.UnitActionInMind == UnitActionInMind.MovingTable)    // QUESTIONABLE PLACE
        {
            Table.TableState = TableState.Moving;
        }
    }
}
