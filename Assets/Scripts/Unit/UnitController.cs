using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitController : MonoBehaviour
{
    public bool debuging = false;
    /*
     This script recieves info about where the unit should go, if it should stop or resume its movement.
     */
    private Unit Unit;

    public void Initialize(Unit unit)
    {
        Unit = unit;
    }

    public void StopMoving(bool targetReached = true)
    {
        if (Unit.UnitFeetState == UnitFeetState.OnGround)
        {
            if (debuging)
                Debug.Log("Reaching (Unit action in mind = " + Unit.UnitActionInMind + ")");

            Unit.AIPath.stopMoving();

            if (targetReached)
            {
                //  If we finish the journey
                //      - And an action is set in mind
                //      - And the Unit is not busy with another action.
                //  --> That means we must fire the Action in mind and exit.
                if (Unit.UnitActionInMind != UnitActionInMind.None && Unit.UnitPrimaryState != UnitPrimaryState.Busy)
                {
                    Unit.UnitActionHandler.StartAction();

                    return;
                }

                //  If we finish the journey
                //      - And for some reason the unit is Busy - dont go into Idle.
                if (Unit.UnitPrimaryState != UnitPrimaryState.Busy)
                {
                    Unit.UnitPrimaryState = UnitPrimaryState.Idle;
                    Unit.UnitBasicAnimation.GoIdle();
                }
            }
            else
            {
                Unit.UnitPrimaryState = UnitPrimaryState.Idle;
                Unit.UnitBasicAnimation.GoIdle();

                Unit.UnitProperties.thisUnitTarget.thisTransform.position = Unit.UnitProperties.thisTransform.position;
            }
        }
        else if (Unit.UnitFeetState == UnitFeetState.OnTable)
        {
            OnTable_Move = false;
            Unit.UnitBasicAnimation.GoIdle();

            if (Unit.UnitActionInMind == UnitActionInMind.ClimbDownTable)
            {
                Unit.UnitFeetState = UnitFeetState.OnGround;
                
                Unit.Table.TableActionHandler.PlayActionAnimation();
            }
        }
    }

    public void ResumeMoving()
    {
        Unit.AIPath.resumeMoving();
        Unit.UnitPrimaryState = UnitPrimaryState.Walk;
        Unit.UnitBasicAnimation.GoWalk();
    }

    public void GoToTarget()
    {
        ResumeMoving();
        Unit.AIPath.SearchPath();
    }

    public void SetPathToTarget(Vector3 targetVector)
    {
        if (Unit.UnitFeetState == UnitFeetState.OnGround)
        {
            Unit.UnitProperties.thisUnitTarget.transform.position = targetVector;
            this.GoToTarget();
        }
        else if (Unit.UnitFeetState == UnitFeetState.OnTable)
        {
            Unit.UnitProperties.thisUnitTarget.transform.position = targetVector;

            OnTable_targetVector = targetVector + new Vector3(0, 1, 0);

            Unit.UnitBasicAnimation.GoWalk();
            OnTable_Move = true;
        }
    }

    void Update()
    {
        if (Unit.UnitFeetState == UnitFeetState.OnTable)
        {
            MoveOnTable();
            return;
        }
        if (Unit != null && Unit.UnitProperties.ControllerFollowRoot)
        {
            FollowRoot();
        }

        if (Unit.UnitPrimaryState == UnitPrimaryState.Walk)
        {
            KeepOnGround();
        }
    }

    bool OnTable_Move;
    Vector3 OnTable_targetVector;

    void MoveOnTable()
    {
        if (OnTable_Move)
        {
            Unit.UnitProperties.thisTransform.position = Vector3.Lerp(Unit.UnitProperties.thisTransform.position,
                                                                        OnTable_targetVector,
                                                                        Time.deltaTime * Unit.UnitProperties.MovementSpeed);

            Unit.UnitProperties.thisTransform.rotation = Logic.SmoothLook(Unit.UnitProperties.thisTransform.rotation,
                Logic.GetDirection(Unit.UnitProperties.thisTransform.position, OnTable_targetVector),
                7f);
        }
    }

    bool startLerp;
    bool lerpComplete;

    Vector3 rootPos_lerp = new Vector3();

    float xRot;
    float yRot;

    void FollowRoot()
    {
        if (lerpComplete == false)
        {
            if (startLerp == false)
            {
                xRot = 90f;
                yRot = 90f;

                startLerp = true;
                StartCoroutine(LerpToPosition(0.6f));
            }
            rootPos_lerp = new Vector3(Unit.UnitProperties.Root.position.x,
                                    Unit.UnitProperties.Root.position.y + 1,
                                    Unit.UnitProperties.Root.position.z);
            Unit.UnitProperties.thisTransform.position = Vector3.Lerp(Unit.UnitProperties.thisTransform.position, rootPos_lerp, Time.deltaTime * 11);
        }
        if (lerpComplete)
        {
            var rootPos = new Vector3(Unit.UnitProperties.Root.position.x,
                                Unit.UnitProperties.Root.position.y + 1,
                                Unit.UnitProperties.Root.position.z);
            Unit.UnitProperties.thisTransform.position = rootPos;
        }

        var rot = new Quaternion();
        rot.eulerAngles = new Vector3(Unit.UnitProperties.Root.eulerAngles.x + xRot,  //90
                                    Unit.UnitProperties.Root.eulerAngles.y - yRot,   //180
                                    Unit.UnitProperties.Root.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(Unit.UnitProperties.thisTransform.rotation, rot, Time.deltaTime * 10);
    }

    public void ExitAction()
    {
        startLerp = false;
        lerpComplete = false;

        Unit.UnitProperties.Root = null;
        StopMoving();
    }

    IEnumerator LerpToPosition(float lerpTime)
    {
        yield return new WaitForSeconds(lerpTime);
        lerpComplete = true;
    }

    void KeepOnGround()
    {
        var pos = Unit.UnitProperties.FeetCollider.transform.position;

        var posDown = new Vector3(pos.x, pos.y - 2f, pos.z);

        var direction = posDown - pos;

        if (debuging)
            Debug.DrawLine(pos, (pos + direction));

        Ray Ray = new Ray(pos, direction);
        RaycastHit Hit;
        if (Physics.Raycast(Ray, out Hit, 100))
        {
            if (Hit.transform.tag == "Map")
            {
                if (debuging)
                    Debug.DrawLine(pos, (pos + direction), Color.red);
                Unit.UnitProperties.thisTransform.position = new Vector3(Unit.UnitProperties.thisTransform.position.x,
                                                                        Hit.point.y + 1f,
                                                                        Unit.UnitProperties.thisTransform.position.z);
            }
        }
    }
}
