using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LedgeActionHandler : MonoBehaviour
{
    private Ledge Ledge;

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

        var UnitYPos = GlobalData.Player.UnitProperties.thisTransform.position.y - 1;

        Ledge.LedgeStartPoint = (LedgeStartPoint)Logic.GetClosestYPos(UnitYPos, Ledge.Bottom_YPos, Ledge.Ypos);

        if (Ledge.LedgeStartPoint != LedgeStartPoint.OutOfReach)
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.None);

        Ledge.LedgeActionHandler.CalculateCircleAction_State();
    }

    public void CalculateCircleAction_State()
    {
        if (Ledge.UI_CircleAction.thisTransform)
        {
            if (Ledge.Unit)
            {
                Ledge.UI_CircleAction.ChangeState(CircleActionState.None);
            }
            else if (Ledge.LedgeStartPoint == LedgeStartPoint.OutOfReach)
            {
                Ledge.UI_CircleAction.ChangeState(CircleActionState.None);
            }
            else
            {
                Ledge.UI_CircleAction.ChangeState(CircleActionState.Available);
            }
            Ledge.UI_CircleAction.thisTransform.position = Ledge.thisTransform.position;
        }
    }
    public void CalculateCircleAction_Point()
    {
        Ledge.CircleAction_Position = Logic.GetEdgePosition(Ledge.CircleAction_Position, Ledge.thisTransform.forward, Ledge.thisTransform.position.y, Ledge.Ypos);
        if (Ledge.CircleAction_Position != Vector3.zero)
        {
            Ledge.UI_CircleAction.thisTransform.position = new Vector3(Ledge.CircleAction_Position.x, Ledge.Ypos, Ledge.CircleAction_Position.z);
            Ledge.UI_EdgeLine.enabled = true;
        }
    }

    public void CreateStartPosition()
    {
        Ledge.ResetLedgeAction();

        var _bottom_Position = new Vector3(Ledge.CircleAction_Position.x, Ledge.Bottom_YPos, Ledge.CircleAction_Position.z);
        var _top_Position = new Vector3(Ledge.CircleAction_Position.x, Ledge.Ypos, Ledge.CircleAction_Position.z);

        if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
        {
            Ledge.StartPointPosition = (_bottom_Position + new Vector3(0, 0, 1f));
        }
        else
        {
            Ledge.StartPointPosition = (_top_Position - new Vector3(0, 0, 0.8f));
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
