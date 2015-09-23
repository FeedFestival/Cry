using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderActionHandler : MonoBehaviour
{
    [HideInInspector]
    public LadderStats LadderStats;

    private UnitActionHandler UnitActionHandler;

    private GameObject LadderGameObject;

    private ActionType LadderActionType;

    // Use this for initialization
    public void Initialize(LadderStats ladderStats, GameObject ladderGameObject)
    {
        LadderStats = ladderStats;

        LadderGameObject = ladderGameObject;

        UnitActionHandler = LadderStats.SceneManager.Player.GetComponent<UnitActionHandler>();  // HARD_CODED

        LadderActionType = ActionType.Ladder;
    }

    public void SetAction(LadderTriggerInput triggerInput)
    {
        switch (triggerInput)
        {
            case LadderTriggerInput.Bottom:
                if (!UnitActionHandler.getIsPlayingAction())
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Bottom);
                }
                else
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Bottom, true);
                }
                break;

            case LadderTriggerInput.Level1:
                if (!UnitActionHandler.getIsPlayingAction())
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level1);
                }
                else
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level1, true);
                }
                break;
            case LadderTriggerInput.Level2_Top:
                if (!UnitActionHandler.getIsPlayingAction())
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level2_Top);
                }
                else
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level2_Top, true);
                }
                break;

            case LadderTriggerInput.Level2:
                if (!UnitActionHandler.getIsPlayingAction())
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level2);
                }
                else
                {
                    UnitActionHandler.SetAction(LadderGameObject, LadderActionType, LadderTriggerInput.Level2, true);
                }
                break;

            default:
                break;
        }
    }
}
