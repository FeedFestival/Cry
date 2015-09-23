using UnityEngine;
using System.Collections;
using Pathfinding;

public class BaseAI : MonoBehaviour
{
    /*
        This script deals with finding the enemy of this Unit party.
     * - It can control the patrol of this unit.
     * - It controls what happens when it spots a unit.
     * - It sends atack signals to the BASIC ATACK script.
     */

    public bool debuging;

    UnitStats UnitStats;

    Transform thisTransform;

    // Bool representing whether or not to shot the ray to the enemy so we can know if we can see him or not.
    bool foundEnemy;
    bool pursueTarget;
    bool isEnemyNear;

    public bool ignoreEnemy;

    public GameObject mainEnemy;

    // Use this for initialization
    void Awake()
    {
        UnitStats = this.gameObject.GetComponent<UnitStats>();

        thisTransform = this.transform;

        SearchAndDestroy();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (foundEnemy)
        {
            Vector3 dir = (mainEnemy.transform.position - thisTransform.position);
            Debug.DrawRay(thisTransform.position, dir, Color.red);
            if (pursueTarget && !ignoreEnemy)
            {
                UnitStats.UnitController.SetPathToTarget(new Vector3(mainEnemy.transform.position.x, mainEnemy.transform.position.y - 1, mainEnemy.transform.position.z));
            }
        }

        //if (mainEnemy)
        //{
        //    Vector3 direct = (mainEnemy.transform.position - thisTransform.position).normalized;
        //    var angle = Vector3.Angle(direct, thisTransform.forward);
        //    Debug.Log(angle);
        //}
    }

    void SearchAndDestroy()
    {

    }

    public void EnemyFound(GameObject enemy)
    {
        foundEnemy = true;
        pursueTarget = true;
        mainEnemy = enemy;

        if (debuging)
            Debug.Log(" (ScriptEnter) Enemy Found - " + foundEnemy);
    }

    public void EnemyNear()
    {
        if (!ignoreEnemy)
        {
            isEnemyNear = true;
            pursueTarget = false;

            Vector3 dir = (mainEnemy.transform.position - thisTransform.position).normalized;
            var angle = Vector3.Angle(dir, thisTransform.forward);
            if (angle > -45f && angle < 45f)
            {
                if (debuging)
                    Debug.Log(" Atack in front - " + dir);
                //UnitStats.BASICATACK.Atack(1);
            }
            else
                if (angle > 45f && angle < 135f)
                {
                    if (debuging)
                        Debug.Log(" Atack in Right - " + dir);
                    //UnitStats.BASICATACK.Atack(2);
                }
        }
    }

    // After an atack has been issued check if the enemy is still near.
    public void CheckIfEnemyNear()
    {
        float distance = Vector3.Distance(thisTransform.position, mainEnemy.transform.position);
        Debug.Log(distance);
        if (distance < 1.5f)
        {
            EnemyNear();
        }
        else
        {
            isEnemyNear = false;
            pursueTarget = true;
            UnitStats.UnitController.ResumeMoving();
        }
    }
}