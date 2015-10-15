using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LedgeInputTrigger : MonoBehaviour
{
    private Ledge Ledge;

    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
    }

    void OnMouseEnter()
    {
        Ledge.LedgeActionHandler.CalculateLedgeState();
    }

    void OnMouseOver()
    {
        if (Ledge.LedgeStartPoint != LedgeStartPoint.OutOfReach)
        {
            if (GlobalData.Player.UnitPrimaryState != UnitPrimaryState.Busy)
            {
                if (GlobalData.Player.UnitActionInMind == UnitActionInMind.None
                    || GlobalData.Player.UnitActionInMind == UnitActionInMind.ClimbingWall)
                {
                    Ledge.LedgeActionHandler.CalculateCircleAction_Point();

                    if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                    {
                        if (Ledge.UI_CircleAction.CircleActionState == CircleActionState.Available)
                        {
                            Ledge.LedgeActionHandler.CreateStartPosition();

                            GlobalData.Player.UnitActionHandler.SetAction(Ledge.thisTransform.gameObject, ActionType.LedgeClimb);
                        }
                    }
                }
            }
            else if (GlobalData.Player.UnitActionState == UnitActionState.ClimbingTable)
            {
                if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
                {
                    Ledge.LedgeActionHandler.CalculateCircleAction_Point();

                    if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
                    {
                        if (Ledge.UI_CircleAction.CircleActionState == CircleActionState.Available)
                        {
                            Ledge.LedgeActionHandler.CreateStartPosition();

                            GlobalData.Player.UnitActionHandler.SetAction(Ledge.thisTransform.gameObject, ActionType.LedgeClimb);
                        }
                    }
                }
            }
        }
    }

    void OnMouseExit()
    {
        Ledge.ResetUI();
    }
}