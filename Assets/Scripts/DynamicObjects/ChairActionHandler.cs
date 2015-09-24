using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class ChairActionHandler : MonoBehaviour
{
    public bool debug;

    [HideInInspector]
    public ChairStats ChairStats;

    [HideInInspector]
    private UnitActionHandler UnitActionHandler;

    private Unit Unit;

    private ActionType ChairActionType;

    public void Initialize(ChairStats chairStats)
    {
        this.ChairStats = chairStats;
        this.Unit = ChairStats.SceneManager.PlayerStats;
        UnitActionHandler = ChairStats.SceneManager.PlayerStats.UnitActionHandler;
        ChairActionType = ActionType.ChairClimb;
    }

    public ChairStartPoint CalculateStartPoint()
    {
        var playerPos = this.Unit.UnitProperties.thisTransform.position;

        float[] distancesChair = new float[3];
        distancesChair[(int)ChairStartPoint.Front] = Vector3.Distance(playerPos, this.Unit.ChairStats.StartPoint_Front.position);
        distancesChair[(int)ChairStartPoint.Left] = Vector3.Distance(playerPos, this.Unit.ChairStats.StartPoint_Left.position);
        distancesChair[(int)ChairStartPoint.Right] = Vector3.Distance(playerPos, this.Unit.ChairStats.StartPoint_Right.position);

        //distances[(int)ChairStartPoint.Back] = Vector3.Distance(playerPos, Unit.ChairStats.StartPoint_Back.position);

        return (ChairStartPoint)Logic.GetSmallestDistance(distancesChair);
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
        if (ChairStats.SceneManager.PlayerStats.UnitActionState == UnitActionState.None)
        {
            UnitActionHandler.SetAction(this.gameObject, ChairActionType);
        }
    }
}
