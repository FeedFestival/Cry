using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class UnitController : MonoBehaviour
{
    /*
     This script recieves info about where the unit should go, if it should stop or resume its movement.
     */
    private Unit _unit;

    public NavMeshPath NavMeshPath;

    // properties
    private float _walkTurnSpeed;
    private float _lerpTime;

    private float _followRootLerpSpeed;
    private float _followRootRotationSpeed;

    private bool _startLerp;
    private bool _lerpComplete;
    private bool _lerpRotComplete = true;

    private Vector3 _lastTarget;

    private Vector3 _rootPosLerp;

    private float _xRot;
    private float _yRot;

    private float _onTableTurnSpeed;
    private float _onTableMoveSpeed;
    private bool _onTableMove;
    private Vector3 _tableTargetVector;

    private float _turnSpeed;
    private bool _startTurningToTarget;
    private Vector3 _turnToTargetPosition;
    public Vector3 TurnToTargetPosition
    {
        set
        {
            _turnToTargetPosition = value;
            _startTurningToTarget = true;
        }
        get { return _turnToTargetPosition; }
    }

    public void Initialize(Unit unit)
    {
        _unit = unit;
        NavMeshPath = new NavMeshPath();

        //can change depending on import
        _xRot = 90f;    // 90
        _yRot = 90f;    // 180

        _lerpTime = 0.6f;

        _followRootLerpSpeed = 11.0f;
        _followRootRotationSpeed = 10.0f;

        _walkTurnSpeed = 11.0f;

        _onTableMoveSpeed = 3.0f;
        _onTableTurnSpeed = 7.0f;
        _turnSpeed = 4.0f;
    }

    public void StopMoving(bool targetReached = true)
    {
        if (_unit.UnitFeetState == UnitFeetState.OnGround)
        {
            if (_unit.NavMeshAgent)
            {
                if (_unit.NavMeshAgent.enabled == false)
                {
                    _unit.NavMeshAgent.enabled = true;
                }
                _unit.NavMeshAgent.Stop();
            }

            if (targetReached)
            {
                //  If we finish the journey
                //      - And an action is set in mind
                //      - And the Player is not busy with another action.
                //  --> That means we must fire the Action in mind and exit.
                if (_unit.UnitActionInMind != UnitActionInMind.None && _unit.UnitPrimaryState != UnitPrimaryState.Busy)
                {
                    _unit.NavMeshAgent.enabled = false;
                    _unit.UnitActionHandler.StartAction();
                    _unit.ActivateTarget(false);
                    return;
                }

                //  If we finish the journey
                //      - And for some reason the unit is Busy - dont go into Idle.
                if (_unit.UnitPrimaryState != UnitPrimaryState.Busy)
                {
                    _unit.UnitPrimaryState = UnitPrimaryState.Idle;
                    _unit.UnitBasicAnimation.Play(_unit.UnitPrimaryState);
                }
            }
            else
            {
                _unit.UnitPrimaryState = UnitPrimaryState.Idle;
                _unit.UnitBasicAnimation.Play(_unit.UnitPrimaryState);

                _unit.UnitProperties.ThisUnitTarget.thisTransform.position = _unit.UnitProperties.ThisUnitTransform.position;
            }

            _unit.ActivateTarget(false);
        }
        else if (_unit.UnitFeetState == UnitFeetState.OnTable)
        {
            _onTableMove = false;
            _unit.UnitBasicAnimation.Play(UnitPrimaryState.Idle);

            if (_unit.UnitActionInMind == UnitActionInMind.ClimbDownTable)
            {
                _unit.UnitFeetState = UnitFeetState.OnGround;

                _unit.Table.TableActionHandler.PlayActionAnimation();
            }
            else if (_unit.UnitActionInMind == UnitActionInMind.ClimbingWall)
            {
                _unit.UnitActionHandler.StartAction();

                _unit.ActivateTarget(false);
                return;
            }
        }
        _unit.ActivateTarget(false);
    }

    public void ResumeMoving()
    {
        _startTurningToTarget = false;

        if (_unit.NavMeshAgent.enabled == false)
            _unit.NavMeshAgent.enabled = true;
        _unit.NavMeshAgent.SetDestination(_unit.UnitProperties.ThisUnitTarget.transform.position);
        _unit.NavMeshAgent.Resume();

        _unit.UnitPrimaryState = UnitPrimaryState.Walk;
        _unit.UnitBasicAnimation.Play(_unit.UnitPrimaryState);
    }

    public void GoToTarget()
    {
        _unit.ActivateTarget(true);

        ResumeMoving();
    }

    public void SetPathToTarget(Vector3 targetVector)
    {
        switch (_unit.UnitFeetState)
        {
            case UnitFeetState.OnGround:
                _unit.UnitProperties.ThisUnitTarget.transform.position = targetVector;
                GoToTarget();
                break;

            case UnitFeetState.OnTable:
                _unit.ActivateTarget(true);

                _unit.UnitProperties.ThisUnitTarget.transform.position = targetVector;

                _tableTargetVector = targetVector;

                _unit.UnitBasicAnimation.Play(_unit.UnitPrimaryState);
                _onTableMove = true;
                break;
        }
    }

    public Vector3[] GetNavMeshPathCorners(Vector3 start, Vector3 goal)
    {
        NavMesh.CalculatePath(start, goal, NavMesh.AllAreas, NavMeshPath);
        return NavMeshPath.corners;
    }

    private IEnumerator LerpToPosition()
    {
        yield return new WaitForSeconds(_lerpTime);
        _lerpComplete = true;
    }
    private IEnumerator LerpToRotation()
    {
        _unit.NavMeshAgent.updateRotation = false;
        _lerpRotComplete = false;

        yield return new WaitForSeconds(_lerpTime);

        _lerpRotComplete = true;
        _unit.NavMeshAgent.updateRotation = true;
    }

    void Update()
    {
        if (_unit)
        {
            if (_unit.UnitFeetState == UnitFeetState.OnTable && _onTableMove)
            {
                MoveOnTable();
                return;
            }
            if (_unit != null && _unit.UnitProperties.ControllerFollowRoot)
            {
                FollowRoot();
            }

            if (_unit.UnitPrimaryState == UnitPrimaryState.Walk)
            {
                if (_lastTarget != _unit.NavMeshAgent.steeringTarget)
                {
                    _lastTarget = _unit.NavMeshAgent.steeringTarget;

                    if (_lerpRotComplete == true)
                        StartCoroutine(LerpToRotation());
                }
                if (_lerpRotComplete == false && _lastTarget != Vector3.zero)
                {
                    _unit.UnitProperties.ThisUnitTransform.rotation = Logic.SmoothLook(_unit.UnitProperties.ThisUnitTransform.rotation,
                        Logic.GetDirection(_unit.UnitProperties.ThisUnitTransform.position, _lastTarget), _walkTurnSpeed);
                }
            }

            if (_unit.UnitPrimaryState == UnitPrimaryState.Idle && _startTurningToTarget)
            {
                _unit.UnitProperties.ThisUnitTransform.rotation = Logic.SmoothLook(_unit.UnitProperties.ThisUnitTransform.rotation,
                                Logic.GetDirection(_unit.UnitProperties.ThisUnitTransform.position, _turnToTargetPosition), _turnSpeed);
                if (Vector3.Angle(_turnToTargetPosition - transform.position, transform.forward) <= 0.2f)
                {
                    _unit.UnitInteligence.FacingAlert = true;
                    _startTurningToTarget = false;
                }
            }
        }
    }

    private void MoveOnTable()
    {
        _unit.UnitProperties.ThisUnitTransform.position = Vector3.Lerp(_unit.UnitProperties.ThisUnitTransform.position,
                                                                    _tableTargetVector,
                                                                    Time.deltaTime * _onTableMoveSpeed);

        _unit.UnitProperties.ThisUnitTransform.rotation = Logic.SmoothLook(_unit.UnitProperties.ThisUnitTransform.rotation,
            Logic.GetDirection(_unit.UnitProperties.ThisUnitTransform.position, _tableTargetVector), _onTableTurnSpeed);
    }

    private void FollowRoot()
    {
        if (_lerpComplete == false)
        {
            if (_startLerp == false)
            {
                _startLerp = true;
                StartCoroutine(LerpToPosition());
            }
            _rootPosLerp = new Vector3(_unit.UnitProperties.Root.position.x,
                                    _unit.UnitProperties.Root.position.y,
                                    _unit.UnitProperties.Root.position.z);
            _unit.UnitProperties.ThisUnitTransform.position = Vector3.Lerp(_unit.UnitProperties.ThisUnitTransform.position, _rootPosLerp, Time.deltaTime * _followRootLerpSpeed);
        }
        if (_lerpComplete)
        {
            var rootPos = new Vector3(_unit.UnitProperties.Root.position.x,
                                _unit.UnitProperties.Root.position.y,
                                _unit.UnitProperties.Root.position.z);
            _unit.UnitProperties.ThisUnitTransform.position = rootPos;
        }

        var rot = new Quaternion();
        rot.eulerAngles = new Vector3(_unit.UnitProperties.Root.eulerAngles.x + _xRot,
                                    _unit.UnitProperties.Root.eulerAngles.y - _yRot,
                                    _unit.UnitProperties.Root.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(_unit.UnitProperties.ThisUnitTransform.rotation, rot, Time.deltaTime * _followRootRotationSpeed);
    }

    public void ExitAction(bool toAnotherAction = false)
    {
        _startLerp = false;
        _lerpComplete = false;

        _unit.UnitProperties.Root = null;

        if (toAnotherAction == false)
            StopMoving();
    }

    private void KeepOnGround()
    {
        var pos = _unit.UnitProperties.FeetCollider.transform.position;

        var posDown = new Vector3(pos.x, pos.y - 2f, pos.z);

        var direction = posDown - pos;

        //Debug.DrawLine(pos, (pos + direction));

        Ray ray = new Ray(pos, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == "Map")
            {
                //Debug.DrawLine(pos, (pos + direction), Color.red);
                _unit.UnitProperties.ThisUnitTransform.position = new Vector3(_unit.UnitProperties.ThisUnitTransform.position.x,
                                                                        hit.point.y + 1f,
                                                                        _unit.UnitProperties.ThisUnitTransform.position.z);
            }
        }
    }
}
