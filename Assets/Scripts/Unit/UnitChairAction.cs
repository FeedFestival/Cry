using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitChairAction : MonoBehaviour
{

    [HideInInspector]
    public UnitStats UnitStats;

    [HideInInspector]
    public ChairStartPoint ChairStartPoint;

    public void Initialize(UnitStats unitStats)
    {
        this.UnitStats = unitStats;
    }

    public void SetPathToStartPoint(ChairStartPoint chairStartPoint)
    {
        ChairStartPoint = chairStartPoint;
        switch (chairStartPoint)
        {
            case ChairStartPoint.Front:
                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Front.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                break;
            case ChairStartPoint.Left:
                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Left.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                break;
            case ChairStartPoint.Right:
                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Right.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                break;
            case ChairStartPoint.Back:
                this.UnitStats.UnitController.SetPathToTarget(UnitStats.ChairStats.StartPoint_Back.position);
                this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
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
                    //this.UnitStats.UnitAnim.PlayAnimation(ChairAnimation.GetOn_FromFront);
                    break;
                case ChairStartPoint.Left:
                    this.UnitStats.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromLeft);
                    break;
                case ChairStartPoint.Right:
                    this.UnitStats.ChairStats.ChairAnimation.PlayAnimation(ChairStaticAnimation.GetOn_FromRight);
                    this.UnitStats.UnitBasicAnimation.SetIsDoingAction(true);
                    break;
                case ChairStartPoint.Back:
                    
                    break;
                default: break;
            }
        }
    }
}
