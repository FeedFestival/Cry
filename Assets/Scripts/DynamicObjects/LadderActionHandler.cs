using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderActionHandler : MonoBehaviour
{
    private LadderStats LadderStats;

    private Unit Unit;

    private GameObject LadderGameObject;

    private ActionType LadderActionType;

    // Use this for initialization
    public void Initialize(LadderStats ladderStats, GameObject ladderGameObject)
    {
        this.LadderStats = ladderStats;

        this.LadderGameObject = ladderGameObject;

        this.Unit = this.LadderStats.SceneManager.PlayerStats;  // HARD_CODED

        LadderActionType = ActionType.Ladder;
    }

    public void SetAction(LadderTriggerInput triggerInput)
    {
        switch (triggerInput)
        {
            case LadderTriggerInput.Bottom:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType, LadderTriggerInput.Bottom);
                break;

            case LadderTriggerInput.Level1:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType, LadderTriggerInput.Level1);
                break;
            case LadderTriggerInput.Level2_Top:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType, LadderTriggerInput.Level2_Top);
                break;

            case LadderTriggerInput.Level2:
                this.Unit.UnitActionHandler.SetAction(this.LadderGameObject, LadderActionType, LadderTriggerInput.Level2);
                break;

            default:
                break;
        }
    }

    public LadderStartPoint CalculateStartPoint()
    {
        var playerPos = this.Unit.UnitProperties.thisTransform.position;

        float[] distancesLadder = new float[2];
        distancesLadder[(int)LadderStartPoint.Bottom] = Vector3.Distance(playerPos, this.LadderStats.StartPoint_Bottom.position);
        distancesLadder[(int)LadderStartPoint.Level2_Top] = Vector3.Distance(playerPos, this.LadderStats.StartPoint_Level2_Top.position);

        return (LadderStartPoint)Logic.GetSmallestDistance(distancesLadder);
    }

    public void CalculateLadderCursor()
    {
        if (this.Unit.UnitPrimaryState != UnitPrimaryState.Busy)
        {
            if (CalculateStartPoint() == LadderStartPoint.Bottom)
                this.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Up);
            else
                this.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Down);

            return;
        }
        else
        {
            RaycastHit CircleHit;
            Ray CircleRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(CircleRay, out CircleHit, 100))
            {
                var unitPosition = Mathf.RoundToInt(this.Unit.UnitProperties.thisTransform.position.y);
                var AimCircle_YPosition = Mathf.RoundToInt(CircleHit.point.y);

                if (AimCircle_YPosition < unitPosition)
                {
                    this.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Down);
                }
                else
                {
                    this.LadderStats.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Ladder_Up);
                }
            }
        }
    }
}
