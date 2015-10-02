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

    public bool exitMoveTableAction;

    public void PlayStatic(TableAnimations animation)
    {
        Table.TableStaticAnimator.CrossFade(animation.ToString());

        float animationLenght = Table.TableStaticAnimator[animation.ToString()].length;

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    public void Play(TableAnimations animation)
    {
        exitMoveTableAction = false;

        if (animation == TableAnimations.DropTable_FromBack)
            exitMoveTableAction = true;

        Table.TableAnimator.CrossFade(animation.ToString());

        PlayStatic(animation);

        float animationLenght = Table.TableAnimator[animation.ToString()].length;

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);

        if (exitMoveTableAction)
            Table.TableActionHandler.ExitMovingTableAction();
    }
}
