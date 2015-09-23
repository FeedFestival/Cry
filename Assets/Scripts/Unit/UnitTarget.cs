using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitTarget : MonoBehaviour
{
    public bool debug;

    private UnitStats UnitStats;

    [HideInInspector]
    public Transform thisTransform;

    // Use this for initialization
    public void Initialize(string Name, UnitStats unitStats)
    {
        this.UnitStats = unitStats;

        thisTransform = this.transform;

        thisTransform.gameObject.name = UnitStats.gameObject.name + "Target";
        thisTransform.gameObject.layer = 11;
    }

    void OnTriggerEnter(Collider foreignObjectHit)
    {
        if (this.UnitStats)
        {
            if (this.UnitStats.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (debug)
                    Debug.Log("Unit is busy with an action and doesnt care about its path target (sad face)");
            }
            else
            {
                if (debug)
                    Debug.Log(foreignObjectHit.transform.gameObject.tag + " = " + this.UnitStats.Tag + " ; "
                            + foreignObjectHit.transform.gameObject.name + " = " + this.UnitStats.FeetCollider.name);

                //  When the feet reach the target we stop the unit.
                if (foreignObjectHit.transform.gameObject.tag == this.UnitStats.Tag && foreignObjectHit.transform.gameObject.name == this.UnitStats.FeetCollider.name)
                {
                    this.UnitStats.UnitController.StopMoving();

                }
            }
        }
    }

    void OnTriggerExit()
    {
        if (this.UnitStats)
        {
            if (this.UnitStats.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (debug)
                    Debug.Log("Unit is busy with an action and doesnt care about its path target (sad face)");
            }
            else
            {
                this.UnitStats.UnitController.ResumeMoving();
            }
        }
    }
}
