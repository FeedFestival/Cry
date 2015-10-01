using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LedgeInputTrigger : MonoBehaviour
{
    private Ledge Ledge;

    // Transforms
    private Vector3 UI_WallClimb_Bottom_Position;
    private Vector3 UI_WallClimb_Top_Position;

    private Vector3 thisWallCLimb_Rotation;

    private float Ypos;
    private float groundYPos;

    [HideInInspector]
    public GameObject Ledge_GameObject;

    // UI
    private GameObject UI_WallClimb;
    private GameObject UI_WallClimbDown;
    
    private RaycastHit Hit;

    private Vector3 lastPosition = new Vector3();

    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;

        thisWallCLimb_Rotation = new Vector3(270, Ledge.thisTransform.eulerAngles.y, 0);

        Ypos = Ledge.thisTransform.position.y;

        switch (Ledge.LedgeType)
        {
            case LedgeType.TwoMetters:
                groundYPos = Ypos - 2f;
                break;
            case LedgeType.ThreeMetters:
                groundYPos = Ypos - 3f;
                break;
            case LedgeType.FourMetters:
                groundYPos = Ypos - 4f;
                break;
            default:
                break;
        }
    }

    void OnMouseEnter()
    {
        CalculateLedgeCursor();
    }

    void OnMouseOver()
    {
        if (Ledge.LedgeStartPoint != LedgeStartPoint.OutOfReach && Ledge.SceneManager.PlayerStats.UnitPrimaryState != UnitPrimaryState.Busy
            && (Ledge.SceneManager.PlayerStats.UnitActionInMind == UnitActionInMind.None 
                || Ledge.SceneManager.PlayerStats.UnitActionInMind == UnitActionInMind.ClimbingWall)
            )
        {
            CalculateStartPoint();

            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                Ledge.ResetLedgeAction();
                SetAction();
            }
        }
    }

    void OnMouseExit()
    {
        ResetUI();
    }

    void CalculateLedgeCursor()
    {
        float[] values = new float[2];
        values[(int)LedgeStartPoint.Bottom] = groundYPos;
        values[(int)LedgeStartPoint.Top] = Ypos;

        Ledge.LedgeStartPoint = (LedgeStartPoint)Logic.GetClosestYPos(Ledge.SceneManager.PlayerStats.UnitProperties.thisTransform.position.y - 1, values, 0.9f);

        switch (Ledge.LedgeStartPoint)
        {
            case LedgeStartPoint.Bottom:
                // Change cursor to wall climb DOWN cursor
                break;
            case LedgeStartPoint.Top:
                // Change cursor to wall climb cursor
                break;
            case LedgeStartPoint.OutOfReach:
                // Change cursor to Default
                break;
            default:
                break;
        }
    }

    public void ResetUI()
    {
        if (UI_WallClimb != null)
        {
            Destroy(UI_WallClimb);
            UI_WallClimb = null;
        }
        if (UI_WallClimbDown != null)
        {
            Destroy(UI_WallClimbDown);
            UI_WallClimbDown = null;
        }
    }

    void SetAction()
    {
        Ledge_GameObject = GameObject.Instantiate(Resources.Load("Prefabs/Ledge"), UI_WallClimb_Bottom_Position, Quaternion.identity) as GameObject;

        Ledge_GameObject.name = "Ledge" + Ledge.thisWallClimb_Name;
        Ledge_GameObject.transform.eulerAngles = new Vector3(Ledge.thisTransform.eulerAngles.x, thisWallCLimb_Rotation.y + 180, Ledge.thisTransform.eulerAngles.z);

        Ledge_GameObject.transform.position = (UI_WallClimb_Bottom_Position + Ledge.thisTransform.forward);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
        {
            Ledge.StartPointPosition = (UI_WallClimb_Bottom_Position + new Vector3(0, 0, 1f));
        }
        else
        {

            Ledge.StartPointPosition = (UI_WallClimb_Top_Position - new Vector3(0, 0, 0.8f));
        }

        Ledge.Ledge_Animator = Ledge_GameObject.GetComponent<Animation>();

        Transform[] allChildren = Ledge_GameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Root")
            {
                Ledge.Root = child;
            }
        }

        // CUSTOM
        if (Ledge.LedgeStartPoint == LedgeStartPoint.Top)
        {
            Ledge.Ledge_Animator.Play(WallClimb_Animations.WallClimbDown_2Metters.ToString());
            Ledge.Ledge_Animator[WallClimb_Animations.WallClimbDown_2Metters.ToString()].speed = 0;
        }

        Ledge.SceneManager.PlayerStats.UnitActionHandler.SetAction(this.gameObject, ActionType.LedgeClimb);
    }

    void CalculateStartPoint()
    {
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Ray, out Hit, 100))
        {
            Hit.point = new Vector3(Mathf.Round(Hit.point.x * 100f) / 100f, Mathf.Round(Hit.point.y * 100f) / 100f, Mathf.Round(Hit.point.z * 100f) / 100f);
            if (lastPosition != Hit.point)
            {
                lastPosition = Hit.point;

                if (Hit.point.y == Ledge.thisTransform.position.y)
                {
                    var _UILinePosition = new Vector3(Hit.point.x, Ypos, Hit.point.z);

                    var One_meterInFront = (Vector3)(Ledge.thisTransform.forward + _UILinePosition);
                    var One_meterInFront_Half_metterDown = One_meterInFront + new Vector3(0, -0.5f, 0);
                    var Half_metterDown = _UILinePosition + new Vector3(0, -0.5f, 0);

                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(One_meterInFront_Half_metterDown, (Half_metterDown - One_meterInFront_Half_metterDown)), out hit, 10))
                    {
                        ShowLine(hit);
                    }
                }
                else
                {
                    ShowLine(Hit);
                }
            }
        }
    }

    private void ShowLine(RaycastHit hit)
    {
        UI_WallClimb_Top_Position = new Vector3(hit.point.x, Ypos, hit.point.z);
        UI_WallClimb_Bottom_Position = new Vector3(hit.point.x, groundYPos, hit.point.z);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
        {
            if (UI_WallClimb == null)
            {
                UI_WallClimb = GameObject.Instantiate(Resources.Load("Prefabs/UI/WallClimb"), UI_WallClimb_Top_Position, Quaternion.identity) as GameObject;

                UI_WallClimb.name = "WallClimb" + Ledge.thisWallClimb_Name;
                UI_WallClimb.transform.eulerAngles = thisWallCLimb_Rotation;
            }
            UI_WallClimb.transform.position = UI_WallClimb_Top_Position;
        }
        else
        {
            if (UI_WallClimbDown == null)
            {
                UI_WallClimbDown = GameObject.Instantiate(Resources.Load("Prefabs/UI/WallClimbDown"), UI_WallClimb_Bottom_Position, Quaternion.identity) as GameObject;

                UI_WallClimbDown.name = "WallClimbDown" + Ledge.thisWallClimb_Name;
                UI_WallClimbDown.transform.eulerAngles = thisWallCLimb_Rotation;
            }
            UI_WallClimbDown.transform.position = UI_WallClimb_Bottom_Position;
        }
    }
}
