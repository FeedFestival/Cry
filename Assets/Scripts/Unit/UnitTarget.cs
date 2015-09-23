using UnityEngine;
using System.Collections;

public class UnitTarget : MonoBehaviour
{

    public bool debug;

    private UnitStats UnitStats;

    // Use this for initialization
    public void Initialize(string Name, UnitStats unitStats)
    {
        this.transform.gameObject.name = Name + "_AITarget";
        this.UnitStats = unitStats;
        this.transform.gameObject.layer = 11;
    }

    void OnTriggerEnter(Collider foreignObjectHit)
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

    void OnTriggerExit()
    {
        UnitStats.UnitController.ResumeMoving();
    }
}
