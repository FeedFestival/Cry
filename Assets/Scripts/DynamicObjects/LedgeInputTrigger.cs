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
    private float startPointYPos;
    private float groundYPos;

    [HideInInspector]
    public GameObject Ledge_GameObject;

    // UI
    private GameObject UI_WallClimb;
    private GameObject UI_WallClimbDown;

    private Vector3 lastPosition = new Vector3();

    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
    }

    void OnMouseEnter()
    {
        thisWallCLimb_Rotation = new Vector3(270, Ledge.thisTransform.eulerAngles.y, 0);

        Ypos = Ledge.thisTransform.position.y - 0.01f;
        groundYPos = Ypos - 2f;

        //switch (Ledge.LedgeType)
        //{
        //    case LedgeType.TwoMetters:
        //        groundYPos = Ypos - 2.0f;
        //        break;
        //    case LedgeType.ThreeMetters:
        //        groundYPos = Ypos - 3.0f;
        //        break;
        //    case LedgeType.FourMetters:
        //        groundYPos = Ypos - 4.0f;
        //        break;
        //    default:
        //        break;
        //}

        CalculateLedgeCursor();
    }

    void OnMouseOver()
    {
        if (Ledge.LedgeStartPoint != LedgeStartPoint.OutOfReach)
        {
            if (Ledge.SceneManager.PlayerStats.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                if (Ledge.SceneManager.PlayerStats.UnitActionInMind == UnitActionInMind.None
                    || Ledge.SceneManager.PlayerStats.UnitActionInMind == UnitActionInMind.ClimbingWall)
                {
                    CalculateStartPoint();

                    if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                    {
                        Ledge.ResetLedgeAction();
                        SetAction();
                    }
                }
            }
            else if (Ledge.SceneManager.PlayerStats.UnitActionState == UnitActionState.ClimbingTable)
            {
                if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
                {
                    CalculateStartPoint();

                    if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                    {
                        Ledge.ResetLedgeAction();
                        SetAction();
                    }
                }
            }

        }
    }

    void OnMouseExit()
    {
        ResetUI();
    }

    void CalculateLedgeCursor()
    {
        var UnitYPos = Ledge.SceneManager.PlayerStats.UnitProperties.thisTransform.position.y - 1;

        Ledge.LedgeStartPoint = (LedgeStartPoint)Logic.GetClosestYPos(UnitYPos, groundYPos, Ypos);

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
        lastPosition = Logic.GetEdgePosition(lastPosition, Ledge.thisTransform.forward, Ledge.thisTransform.position.y, Ypos);
        if (lastPosition != Vector3.zero)
            ShowLine(lastPosition);
    }

    private void ShowLine(Vector3 position)
    {
        UI_WallClimb_Top_Position = new Vector3(position.x, Ypos, position.z);
        UI_WallClimb_Bottom_Position = new Vector3(position.x, groundYPos, position.z);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
        {
            if (UI_WallClimb == null)
            {
                UI_WallClimb = Logic.InstantiateEdgeUI(UI_WallClimb_Top_Position, thisWallCLimb_Rotation, Vector3.zero, "WallClimb" + Ledge.thisWallClimb_Name);
            }
            UI_WallClimb.transform.position = UI_WallClimb_Top_Position;
        }
        else
        {
            if (UI_WallClimbDown == null)
            {
                UI_WallClimbDown = Logic.InstantiateEdgeUI(UI_WallClimb_Bottom_Position, thisWallCLimb_Rotation, Vector3.zero, "WallClimbDown" + Ledge.thisWallClimb_Name);
            }
            UI_WallClimbDown.transform.position = UI_WallClimb_Bottom_Position;
        }
    }
}
