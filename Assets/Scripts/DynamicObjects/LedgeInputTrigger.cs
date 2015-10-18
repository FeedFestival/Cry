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
        if (Ledge.LedgeState == LedgeState.Static)
        {
            Ledge.LedgeActionHandler.CalculateLedgeState();
        }
    }

    void OnMouseOver()
    {
        if (Ledge.LedgeState == LedgeState.Static)
        {
            if (Ledge.LedgeStartPoint != LedgeStartPoint.OutOfReach)
            {
                if (GlobalData.Player.UnitPrimaryState != UnitPrimaryState.Busy)
                {
                    if (GlobalData.Player.UnitActionInMind == UnitActionInMind.None
                        || GlobalData.Player.UnitActionInMind == UnitActionInMind.ClimbingWall)
                    {
                        Ledge.LedgeActionHandler.CalculateCircleActionPoint();

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
                else if (GlobalData.Player.UnitFeetState == UnitFeetState.OnTable)
                {
                    if (Ledge.LedgeStartPoint == LedgeStartPoint.Bottom)
                    {
                        Ledge.LedgeActionHandler.CalculateCircleActionPoint();

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
    }

    void OnMouseExit()
    {
        if (Ledge.LedgeState == LedgeState.Static)
        {
            Ledge.ResetUI();
        }
    }
}