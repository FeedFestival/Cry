using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class TableController : MonoBehaviour
{
    [HideInInspector]
    public Table Table;
    [HideInInspector]
    public Unit Unit;

    [HideInInspector]
    public Transform thisTransform;
    [HideInInspector]
    public Vector3 angleToRotateTo = Vector3.zero;

    private Transform TM_RotSign;
    private Transform TM_MovSign;
    private MeshRenderer TM_RightRot;
    private MeshRenderer TM_LeftRot;

    Vector3 RotationPoint = Vector3.zero;
    Vector3 MovementPoint = Vector3.zero;

    private GameObject trailGameObject;

    private float minimumMoveLength = 2.44f;

    [HideInInspector]
    public bool rotateTable;
    [HideInInspector]
    public bool moveTable;

    public TableMovementAction TableMovementAction;

    public void Initialize(Table table, Unit unit, Vector3 rotation)
    {
        Table = table;
        Unit = unit;

        thisTransform = this.transform;

        thisTransform.eulerAngles = rotation;

        Unit.UnitProperties.ThisUnitTransform.parent = thisTransform;
        Table.TableProperties.thisTransform.parent = thisTransform;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "TM_RotSign":
                    TM_RotSign = child.transform;
                    break;
                case "TM_MovSign":
                    TM_MovSign = child.transform;
                    break;
                case "TM_LeftRot":
                    TM_LeftRot = child.transform.GetComponent<MeshRenderer>();
                    break;
                case "TM_RightRot":
                    TM_RightRot = child.transform.GetComponent<MeshRenderer>();
                    break;
                default:
                    break;
            }
        }
        Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Idle_Table.ToString());
        Table.TableAnimation.PlayLoopAction(TableAnimations.Idle_Table);
    }

    public void DestroyInstance()
    {
        Unit.UnitProperties.ThisUnitTransform.parent = null;
        Table.TableProperties.thisTransform.parent = null;

        Destroy(thisTransform.gameObject);
    }

    Vector3 A = Vector3.zero;
    Vector3 C = Vector3.zero;
    Vector3 B = Vector3.zero;
    Vector3 D = Vector3.zero;

    public void CalculateAction(Vector3 b)
    {
        if (rotateTable == false && moveTable == false)
        {
            A = thisTransform.position;
            B = b;
            C = A + thisTransform.forward;
            D = A + thisTransform.right;

            var AC = Vector3.Distance(A, C);
            var AB = Vector3.Distance(A, B);
            var BC = Vector3.Distance(B, C);    // base of the triangle.

            // Law Of The Cousines  ¯\_(ツ)_/¯
            // cos A = (b2 + c2 − a2) / 2bc
            // cos A = (AC^2 + AB^2 − BC^2) / 2 * AC * AB
            var cosA = (Mathf.Pow(AC, 2) + Mathf.Pow(AB, 2) - Mathf.Pow(BC, 2)) / (2 * (AC * AB));

            var radiansA = Mathf.Acos(cosA);

            // this is the angle at wich we have to rotate, we dont know if minus or plus sign right now.
            var angleA = radiansA * Mathf.Rad2Deg;

            if (angleA < 90f && angleA > 11f)
            {
                // we are rotating the table.
                var AD = Vector3.Distance(A, D);
                var BD = Vector3.Distance(B, D);
                cosA = (Mathf.Pow(AD, 2) + Mathf.Pow(AB, 2) - Mathf.Pow(BD, 2)) / (2 * (AD * AB));
                radiansA = Mathf.Acos(cosA);

                // we calculate for another triangle the sign of the angle (+ or -)
                var TableAngle = radiansA * Mathf.Rad2Deg;

                if (TableAngle < 180f && TableAngle > 0f)
                {
                    if (TableAngle < 90)
                    {
                        TableMovementAction = TableMovementAction.RotateLeft;
                        SetRenderers();
                        angleToRotateTo = thisTransform.eulerAngles + new Vector3(0, angleA, 0);
                        TM_RotSign.localScale = new Vector3(-0.44f, 0.7f, 0.7f);
                    }
                    else
                    {
                        TableMovementAction = TableMovementAction.RotateRight;
                        SetRenderers();
                        angleToRotateTo = thisTransform.eulerAngles - new Vector3(0, angleA, 0);
                        TM_RotSign.localScale = new Vector3(0.44f, 0.7f, 0.7f);
                    }

                    var AB_desiredLength = 2f;
                    RotationPoint = Logic.IncreaseOrDecreaseLine(A, B, AB, AB_desiredLength);

                    // with this point we set the rotation to start and make the target  available.
                    TM_RotSign.eulerAngles = angleToRotateTo;
                    TM_RotSign.GetComponent<MeshRenderer>().enabled = true;

                    return;
                }
            }
            else if (angleA < 11f)
            {
                TableMovementAction = TableMovementAction.Move;

                if (AB > minimumMoveLength)
                    MovementPoint = Logic.IncreaseOrDecreaseLine(A, C, AC, AB);
                else
                    MovementPoint = Logic.IncreaseOrDecreaseLine(A, C, AC, minimumMoveLength);

                TM_MovSign.position = MovementPoint;
                SetRenderers();
                return;
            }
            SetRenderers(false);
        }
    }

    void SetRenderers(bool show = true)
    {
        if (show == false)
        {
            TM_LeftRot.enabled = false;
            TM_RightRot.enabled = false;
            TM_RotSign.GetComponent<MeshRenderer>().enabled = false;
            TM_MovSign.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            TM_MovSign.GetComponent<MeshRenderer>().enabled = false;
            if (TableMovementAction == TableMovementAction.RotateRight)
            {
                TM_RightRot.enabled = true;
                TM_LeftRot.enabled = false;
            }
            else if (TableMovementAction == TableMovementAction.RotateLeft)
            {
                TM_LeftRot.enabled = true;
                TM_RightRot.enabled = false;
            }
            else
            {
                TM_LeftRot.enabled = false;
                TM_RightRot.enabled = false;
                TM_RotSign.GetComponent<MeshRenderer>().enabled = false;
                TM_MovSign.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void StopTableMovement()
    {
        Destroy(trailGameObject);

        if (rotateTable)
        {
            rotateTable = false;

            Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Idle_Table.ToString());
            Table.TableAnimation.PlayLoopAction(TableAnimations.Idle_Table);
            return;
        }
        moveTable = false;

        Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Idle_Table.ToString());
        Table.TableAnimation.PlayLoopAction(TableAnimations.Idle_Table);
    }

    public Vector3 MoveTable()
    {
        LeaveTrail();
        SetRenderers(false);

        if (TableMovementAction == TableMovementAction.RotateRight || TableMovementAction == TableMovementAction.RotateLeft)
        {
            rotateTable = true;

            if (TableMovementAction == TableMovementAction.RotateRight)
            {
                Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Rotate_Table_Right.ToString());
                Table.TableAnimation.PlayLoopAction(TableAnimations.Rotate_Table_Right);
            }
            else
            {
                Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Rotate_Table_Left.ToString());
                Table.TableAnimation.PlayLoopAction(TableAnimations.Rotate_Table_Left);
            }
            return RotationPoint;
        }
        moveTable = true;

        Unit.UnitActionAnimation.PlayLoopAction(TableAnimations.Move_Table.ToString());
        Table.TableAnimation.PlayLoopAction(TableAnimations.Move_Table);

        return MovementPoint;
    }

    public void LeaveTrail()
    {
        if (TableMovementAction == TableMovementAction.RotateRight)
        {
            trailGameObject = (GameObject)Instantiate(TM_RotSign.gameObject, TM_RotSign.position, TM_RotSign.rotation);
            trailGameObject.transform.parent = null;
        }
        else if (TableMovementAction == TableMovementAction.RotateLeft)
        {
            trailGameObject = (GameObject)Instantiate(TM_RotSign.gameObject, TM_RotSign.position, TM_RotSign.rotation);
            trailGameObject.transform.parent = null;
        }
        else
        {
            trailGameObject = (GameObject)Instantiate(TM_MovSign.gameObject, TM_MovSign.position, TM_MovSign.rotation);
            trailGameObject.transform.parent = null;
        }
    }

    void Update()
    {
        if (A != Vector3.zero)
        {
            Debug.DrawLine(A, C, Color.blue);
            Debug.DrawLine(B, C, Color.white);

            Debug.DrawLine(A, D, Color.green);
            Debug.DrawLine(B, D, Color.green);

            if (TableMovementAction == TableMovementAction.Move)
            {
                Debug.DrawLine(A, MovementPoint, Color.red);
            }
            else
            {
                Debug.DrawLine(A, RotationPoint, Color.yellow);
            }
        }

        if (rotateTable)
        {
            thisTransform.eulerAngles = Vector3.MoveTowards(thisTransform.eulerAngles, angleToRotateTo, Time.deltaTime * 22f);
        }
        if (moveTable)
        {
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, MovementPoint, Time.deltaTime * 1.11f);
        }
    }
}