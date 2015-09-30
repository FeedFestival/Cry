using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Types
{
    #region UI

    public enum CursorType
    {
        Default = 0, Ladder_Up = 1, Ladder_Down = 2, Grab = 3
    }
    public enum MouseInput
    {
        LeftClick = 0, RightClick = 1
    }

    #endregion

    #region Unit [State]

    public enum UnitPrimaryState
    {
        Idle = 0, Walk = 1, Busy = 2
    }

    public enum UnitActionState
    {
        None = 0, ClimbingLadder = 1, ClimbingChair = 2, ClimbingWall = 3
    }
    public enum UnitActionInMind
    {
        None = 0, ClimbingLadder = 1, ClimbingChair = 2, ClimbingWall = 3
    }

    public enum UnitFeetState
    {
        OnGround = 0, OnStairs = 1, OnChair = 2, OnLadder = 3
    }

    #endregion

    public enum ActionType
    {
        None = 0, Ladder = 1, ChairClimb = 2, ChairGrab = 3, LedgeClimb = 4
    }

    #region Chair

    public enum ChairStartPoint
    {
        Front = 0, Left = 1, Right = 2, Back = 3
    }
    public enum ChairStaticAnimation
    {
        GetOn_FromFront = 0,
        GetOn_FromLeft = 1,
        GetOn_FromRight = 2,
        GetOff_ToFront = 3,
        GetOff_ToLeft = 4,
        GetOff_ToRight = 5,
    }

    #endregion

    #region Ladder

    public enum LadderTriggerInput
    {
        Bottom = 0, Level1 = 1, Level2_Top = 2,
        Level2 = 3
            , Level3_Top = 4, Level3 = 5, Level4_Top = 6
    }
    public enum LadderStartPoint
    {
        Bottom = 0, Level2_Top = 1
    }

    public class LadderPath
    {
        public bool Played { get; set; }
        public bool IsLastAction { get; set; }
        public bool ExitAction { get; set; }
        public LadderAnimations LadderAnimation { get; set; }
    }

    public enum LadderAnimations
    {
        GetOn_From_Bottom = 0,
        //Climb_Exit_To_Level1_Top = 2,
        Climb_From_Level1_To_Level2 = 1,
        Climb_Exit_To_Level2_Top = 2,

        GetOn_From_Level2_Top = 3,
        ClimbDown_From_Level1_To_Bottom = 4,
        ClimbDown_Exit_To_Bottom = 5,

        Idle_Ladder = 10,
        Jump_Exit_To_Bottom = 11
    }

    #endregion

    #region Wall Ledge

    public enum LedgeType
    {
        HidePoint = 1, TwoMetters = 2, ThreeMetters = 3, FourMetters = 4
    }

    public enum LedgeStartPoint
    {
        Bottom = 0, Top = 1, OutOfReach = 2
    }

    public enum WallClimb_Animations
    {
        WallClimb_2Metters = 0, WallClimbDown_2Metters = 1
    }

    #endregion

    public static class Logic
    {
        public static int GetSmallestDistance(float[] distances)
        {
            int smallestIndex = 0;
            if (distances.Length > 2)
            {


                float smallest = distances[0];
                for (var i = 0; i < distances.Length; i++)
                {
                    if (distances[i] < smallest)
                    {
                        smallest = distances[i];
                        smallestIndex = i;
                    }
                }
            }
            else
            {
                if (distances[1] < distances[0])
                    smallestIndex = 1;
            }
            return smallestIndex;
        }

        public static int GetClosestYPos(float unitYPos, float[] values, float threshold = 0.5f)
        {
            int returnValue = 2;
            if (values.Length > 2)
            {

            }
            else
            {
                if (Mathf.Round(unitYPos - threshold) <= Mathf.Round(values[0]) && Mathf.Round(unitYPos + threshold) >= Mathf.Round(values[0])
                    )
                {
                    returnValue = 0;
                }
                if (Mathf.Round(unitYPos - threshold) <= Mathf.Round(values[1]) && Mathf.Round(unitYPos + threshold) >= Mathf.Round(values[1])
                    )
                {
                    returnValue = 1;
                }
            }
            return returnValue;
        }
    }
}
