using UnityEngine;
using System.Collections;

public class BasicAtack : MonoBehaviour {

    Unit Unit;
    BaseAI BASEAI;

    Transform thisTransform;

    public DebugProgressBar atackFrontCoolDown;
    public DebugProgressBar atackRightCoolDown;

	// Use this for initialization
	public void Initialize (Unit stats , UnitController uc , BaseAI baseai) {
        Unit = stats;

        thisTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        #region Atack
        if (Input.GetKey(KeyCode.A))
        {
            Atack(1);
        }
        #endregion
	}

    public void Atack(int type)
    {
        Debug.Log(" Atack");
        switch (type)
        {
            case 1:
                // Atack in Front
                Unit.UnitController.StopMoving();
                atackFrontCoolDown.StartCoolDown(Unit.UnitProperties.AtackSpeed_Impact, Unit.UnitProperties.AtackSpeed_ParriedOrComplete, this);

                Unit.UnitController.SetPathToTarget(thisTransform.position + thisTransform.forward);
                Unit.UnitController.ResumeMoving();
                break;
            case 2:
                Unit.UnitController.StopMoving();
                atackRightCoolDown.StartCoolDown(Unit.UnitProperties.AtackSpeed_Impact, Unit.UnitProperties.AtackSpeed_ParriedOrComplete, this);
                break;
            default:
                break;
        }

    }

    public void AtackEnded() {
        BASEAI.CheckIfEnemyNear();
    }
}
