using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitChairAction : MonoBehaviour
{
    private UnitStats UnitStats;

    [HideInInspector]
    public ChairStartPoint ChairStartPoint;

    public void Initialize(UnitStats unitStats)
    {
        this.UnitStats = unitStats;
    }

    public void SetPathToStartPoint(ChairStartPoint chairStartPoint)
    {
        this.UnitStats.UnitActionInMind = UnitActionInMind.ClimbingChair;

        ChairStartPoint = chairStartPoint;
        switch (chairStartPoint)
        {
            case ChairStartPoint.Front:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Front.position);
                break;

            case ChairStartPoint.Left:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Left.position);
                break;

            case ChairStartPoint.Right:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Right.position);
                break;

            case ChairStartPoint.Back:

                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Back.position);
                break;

            default: break;
        }
    }

    public void PlayActionAnimation()
    {
        // Check if we are on the chair allready
        if (UnitStats.UnitFeetState == UnitFeetState.OnGround)
        {
            switch (ChairStartPoint)
            {
                case ChairStartPoint.Front:

                    this.UnitStats.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromFront);
                    break;

                case ChairStartPoint.Left:

                    this.UnitStats.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromLeft);
                    break;

                case ChairStartPoint.Right:

                    this.UnitStats.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromRight);
                    break;

                case ChairStartPoint.Back:
                    
                    break;
                default: break;
            }
        }
    }
}
