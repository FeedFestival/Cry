using UnityEngine;
using System.Collections;

public class BasicAtack : MonoBehaviour {

    UnitStats UnitStats;
    BaseAI BASEAI;

    Transform thisTransform;

    public DebugProgressBar atackFrontCoolDown;
    public DebugProgressBar atackRightCoolDown;

	// Use this for initialization
	public void Initialize (UnitStats stats , UnitController uc , BaseAI baseai) {
        UnitStats = stats;

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
                UnitStats.UnitController.StopMoving();
                atackFrontCoolDown.StartCoolDown(UnitStats.AtackSpeed_Impact ,UnitStats.AtackSpeed_ParriedOrComplete , this);

                UnitStats.UnitController.SetPathToTarget(thisTransform.position + thisTransform.forward);
                UnitStats.UnitController.ResumeMoving();
                break;
            case 2:
                UnitStats.UnitController.StopMoving();
                atackRightCoolDown.StartCoolDown(UnitStats.AtackSpeed_Impact, UnitStats.AtackSpeed_ParriedOrComplete, this);
                break;
            default:
                break;
        }

    }

    public void AtackEnded() {
        BASEAI.CheckIfEnemyNear();
    }
}
