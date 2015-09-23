using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class ChairActionHandler : MonoBehaviour {

    public bool debug;

    [HideInInspector]
    public ChairStats ChairStats;

    [HideInInspector]
    private UnitActionHandler UnitActionHandler;

    private ActionType ChairActionType;

    public void Initialize(ChairStats chairStats)
    {
        this.ChairStats = chairStats;
        UnitActionHandler = ChairStats.SceneManager.PlayerStats.UnitActionHandler;
        ChairActionType = ActionType.ChairClimb;
    }

    private bool oldValue = false;
    public void HighlightObject(bool value)
    {
        if (value != oldValue)
        {
            oldValue = value;
            if (value)
                ChairStats.ChairMaterial.SetFloat("_Brightness", 2.0f);
            else
                ChairStats.ChairMaterial.SetFloat("_Brightness", 1.0f);
        }
    }
    public void SetAction(int triggerId)
    {
        if (!UnitActionHandler.getIsPlayingAction())
        {
            UnitActionHandler.SetAction(this.gameObject, ChairActionType);
        }
    }
}
