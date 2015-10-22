using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using System;

public class LedgeActionHandler : MonoBehaviour
{
    private Ledge Ledge;

    private Vector3 _lastCirclePosition = new Vector3();

    // Use this for initialization
    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
    }

    public void CalculateLedgeState()
    {
        Ledge.Animator_Rotation = new Vector3(270, Ledge.thisTransform.eulerAngles.y, 0);

        Ledge.Ypos = Ledge.thisTransform.position.y - 0.01f;
        Ledge.Bottom_YPos = Ledge.Ypos - 2f;

        var UnitYPos = GlobalData.Player.UnitProperties.thisTransform.position.y;

        Ledge.LedgeStartPoint = (LedgeStartPoint)Logic.GetClosestYPos(UnitYPos, Ledge.Bottom_YPos, Ledge.Ypos);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.OutOfReach)
        {
            Ledge.CircleActionState = CircleActionState.None;
            Ledge.UI_CircleAction.Hide();
        }
    }

    public void CalculateCircleActionPoint()
    {
        var hitPosition = Logic.GetEdgePosition(Ledge.CircleAction_Position, Ledge.thisTransform.forward, Ledge.thisTransform.position.y, Ledge.Ypos);
        if (_lastCirclePosition != hitPosition && hitPosition != Vector3.zero)
        {
            _lastCirclePosition = hitPosition;

            Ledge.CircleAction_Position = new Vector3(hitPosition.x, Ledge.Ypos, hitPosition.z);

            if (Ledge.LedgeStartPoint == LedgeStartPoint.Top)
            {
                CalculateBottomPoint(Ledge.CircleAction_Position);
            }
            else if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
            {
                if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable || GlobalData.Player.UnitFeetState == UnitFeetState.OnGround)
                    CalculateBottomPoint(Ledge.CircleAction_Position);
            }
            else
            {
                return;
            }
            Ledge.UI_CircleAction.thisTransform.position = Ledge.CircleAction_Position;
            Ledge.UI_EdgeLine.enabled = true;
        }
    }

    public void CalculateBottomPoint(Vector3 circlePosition)
    {
        var distanceThreshold = 1.5f;

        var pos1 = Ledge.CircleAction_Position + (Ledge.thisTransform.forward / distanceThreshold);

        var _bottom_Position = new Vector3(Ledge.CircleAction_Position.x, Ledge.Bottom_YPos - 0.5f, Ledge.CircleAction_Position.z);
        var pos2 = _bottom_Position + (Ledge.thisTransform.forward / distanceThreshold);

        var direction = Logic.GetDirection(pos1, pos2);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(pos1, direction), out hit, 2.5f))
        {
            Debug.DrawRay(pos1, direction);
            if (hit.transform.gameObject.tag == "Table")
            {
                Ledge.LedgeBottomPoint = LedgeBottomPoint.Table;
                Ledge.Table = hit.transform.parent.transform.GetComponent<Table>();

                Ledge.CircleActionState = CircleActionState.Available;
                Ledge.UI_CircleAction.GoAvailable();
            }
            else if (hit.transform.gameObject.tag == "Map")
            {
                Ledge.LedgeBottomPoint = LedgeBottomPoint.Map;
                Ledge.CircleActionState = CircleActionState.Available;
                Ledge.UI_CircleAction.GoAvailable();
            }
            else if (hit.transform.gameObject.tag == "Player") // We should avoid player collider // FOR_NOW
            {
                Ledge.CircleActionState = CircleActionState.Available;
                Ledge.UI_CircleAction.GoAvailable();
            }
            else
            {
                Ledge.LedgeBottomPoint = LedgeBottomPoint.Impediment;
                Ledge.CircleActionState = CircleActionState.Unavailable;
                Ledge.UI_CircleAction.GoUnavailable();
            }
        }
        else
        {
            Ledge.LedgeBottomPoint = LedgeBottomPoint.Nothing;
            Ledge.CircleActionState = CircleActionState.Unavailable;
            Ledge.UI_CircleAction.GoUnavailable();
        }
    }

    public void CreateStartPosition()
    {
        Ledge.ResetLedgeAction();

        var _bottom_Position = new Vector3(Ledge.CircleAction_Position.x, Ledge.Bottom_YPos, Ledge.CircleAction_Position.z);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
        {
            Ledge.StartPointPosition = (_bottom_Position + new Vector3(0, 0, 1f));
        }
        else
        {
            Ledge.StartPointPosition = (Ledge.CircleAction_Position - new Vector3(0, 0, 0.8f));
        }

        CreateAnimator(_bottom_Position);
    }
    private void CreateAnimator(Vector3 bottom_Pos)
    {
        Ledge.Ledge_GameObject = GameObject.Instantiate(Resources.Load("Prefabs/Ledge"), bottom_Pos, Quaternion.identity) as GameObject;

        Ledge.Ledge_GameObject.name = "Ledge" + Ledge.thisWallClimb_Name;
        Ledge.Ledge_GameObject.transform.eulerAngles = new Vector3(Ledge.thisTransform.eulerAngles.x, Ledge.Animator_Rotation.y + 180, Ledge.thisTransform.eulerAngles.z);

        Ledge.Ledge_GameObject.transform.position = (bottom_Pos + Ledge.thisTransform.forward);

        Ledge.Ledge_Animator = Ledge.Ledge_GameObject.GetComponent<Animation>();

        Transform[] allChildren = Ledge.Ledge_GameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Root")
            {
                Ledge.Root = child;
            }
        }
        if (Ledge.LedgeStartPoint == LedgeStartPoint.Top)
        {
            Ledge.LedgeAnimation.SetStartupAnimation();
        }
    }

    public void PlayActionAnimation()
    {
        switch (Ledge.LedgeStartPoint)
        {
            case LedgeStartPoint.Bottom:

                Ledge.Unit.UnitActionAnimation.PlaySingleAnimation(WallClimb_Animations.WallClimb_2Metters.ToString());
                Ledge.LedgeAnimation.Play(WallClimb_Animations.WallClimb_2Metters.ToString());
                break;

            case LedgeStartPoint.Top:

                Ledge.Unit.UnitActionAnimation.PlaySingleAnimation(WallClimb_Animations.WallClimbDown_2Metters.ToString());
                Ledge.Ledge_Animator[WallClimb_Animations.WallClimbDown_2Metters.ToString()].speed = 1; // CUSTOM

                Ledge.LedgeAnimation.Play(WallClimb_Animations.WallClimbDown_2Metters.ToString());
                
                break;

            default:
                break;
        }
    }
}