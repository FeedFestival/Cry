using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitChairAction : MonoBehaviour
{
    private Unit Unit;

    [HideInInspector]
    public ChairStartPoint ChairStartPoint;

    public void Initialize(Unit unit)
    {
        this.Unit = unit;
    }

    public void SetPathToStartPoint(ChairStartPoint chairStartPoint)
    {
        this.Unit.UnitActionInMind = UnitActionInMind.ClimbingChair;

        ChairStartPoint = chairStartPoint;
        switch (chairStartPoint)
        {
            case ChairStartPoint.Front:

                this.Unit.UnitController.SetPathToTarget(Unit.ChairStats.StartPoint_Front.position);
                break;

            case ChairStartPoint.Left:

                this.Unit.UnitController.SetPathToTarget(Unit.ChairStats.StartPoint_Left.position);
                break;

            case ChairStartPoint.Right:

                this.Unit.UnitController.SetPathToTarget(Unit.ChairStats.StartPoint_Right.position);
                break;

            case ChairStartPoint.Back:

                this.Unit.UnitController.SetPathToTarget(Unit.ChairStats.StartPoint_Back.position);
                break;

            default: break;
        }
    }

    public void PlayActionAnimation()
    {
        // Check if we are on the chair allready
        if (Unit.UnitFeetState == UnitFeetState.OnGround)
        {
            switch (ChairStartPoint)
            {
                case ChairStartPoint.Front:

                    this.Unit.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromFront);
                    break;

                case ChairStartPoint.Left:

                    this.Unit.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromLeft);
                    break;

                case ChairStartPoint.Right:

                    this.Unit.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromRight);
                    break;

                case ChairStartPoint.Back:
                    
                    break;
                default: break;
            }
        }
    }
}
