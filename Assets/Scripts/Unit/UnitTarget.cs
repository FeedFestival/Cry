using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class UnitTarget : MonoBehaviour
{
    public bool debug;

    private Unit Unit;

    [HideInInspector]
    public Transform thisTransform;

    // Use this for initialization
    public void Initialize(Unit unit)
    {
        this.Unit = unit;

        thisTransform = this.transform;
    }

    void OnTriggerEnter(Collider foreignObjectHit)
    {
        if (this.Unit)
        {
            if (this.Unit.UnitPrimaryState == UnitPrimaryState.Busy && this.Unit.UnitFeetState != UnitFeetState.OnTable)
            {
                if (debug)
                    Debug.Log("Player is busy with an action and doesnt care about its path target (sad face)");

                if (this.Unit.UnitActionState == UnitActionState.MovingTable)
                {
                    this.Unit.Table.TableController.StopTableMovement();
                }

                return;
            }

            if (debug)
                Debug.Log(foreignObjectHit.transform.gameObject.tag + " = " + this.Unit.UnitProperties.Tag + " ; "
                        + foreignObjectHit.transform.gameObject.name + " = " + this.Unit.UnitProperties.FeetCollider.name);

            //  When the feet reach the target we stop the unit.
            if (foreignObjectHit.transform.gameObject.tag == this.Unit.UnitProperties.Tag
                && foreignObjectHit.transform.gameObject.name == this.Unit.UnitProperties.FeetCollider.name)
            {
                this.Unit.UnitController.StopMoving();

            }

        }
    }

    void OnTriggerExit()
    {
        if (this.Unit)
        {
            if (this.Unit.UnitPrimaryState == UnitPrimaryState.Busy)
            {
                if (debug)
                    Debug.Log("Player is busy with an action and doesnt care about its path target (sad face)");
            }
            else
            {
                this.Unit.UnitController.ResumeMoving();
            }
        }
    }
}
