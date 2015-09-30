using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class ChairActionHandler : MonoBehaviour
{
    public bool debug;

    [HideInInspector]
    public Chair Chair;

    [HideInInspector]
    private UnitActionHandler UnitActionHandler;

    private Unit Unit;

    private ActionType ChairActionType;

    public void Initialize(Chair chair)
    {
        this.Chair = chair;
        this.Unit = Chair.SceneManager.PlayerStats;
        UnitActionHandler = Chair.SceneManager.PlayerStats.UnitActionHandler;
        ChairActionType = ActionType.ChairClimb;
    }

    public ChairStartPoint CalculateStartPoint()
    {
        var playerPos = this.Unit.UnitProperties.thisTransform.position;

        float[] distancesChair = new float[3];
        distancesChair[(int)ChairStartPoint.Front] = Vector3.Distance(playerPos, this.Unit.Chair.StartPoint_Front.position);
        distancesChair[(int)ChairStartPoint.Left] = Vector3.Distance(playerPos, this.Unit.Chair.StartPoint_Left.position);
        distancesChair[(int)ChairStartPoint.Right] = Vector3.Distance(playerPos, this.Unit.Chair.StartPoint_Right.position);

        //distances[(int)ChairStartPoint.Back] = Vector3.Distance(playerPos, Unit.Chair.StartPoint_Back.position);

        return (ChairStartPoint)Logic.GetSmallestDistance(distancesChair);
    }

    private bool oldValue = false;
    public void HighlightObject(bool value)
    {
        if (value != oldValue)
        {
            oldValue = value;
            if (value)
                Chair.ChairMaterial.SetFloat("_Brightness", 2.0f);
            else
                Chair.ChairMaterial.SetFloat("_Brightness", 1.0f);
        }
    }
    public void SetAction(int triggerId)
    {
        if (Chair.SceneManager.PlayerStats.UnitActionState == UnitActionState.None)
        {
            UnitActionHandler.SetAction(this.gameObject, ChairActionType);
        }
    }

    public void PlayActionAnimation()
    {
        // Check if we are on the chair allready
        if (Unit.UnitFeetState == UnitFeetState.OnGround)
        {
            switch (Chair.ChairStartPoint)
            {
                case ChairStartPoint.Front:

                    this.Unit.Chair.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromFront);
                    break;

                case ChairStartPoint.Left:

                    this.Unit.Chair.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromLeft);
                    break;

                case ChairStartPoint.Right:

                    this.Unit.Chair.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromRight);
                    break;

                case ChairStartPoint.Back:

                    break;
                default: break;
            }
        }
    }
}
