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
        None = 11, Default = 0, Ladder_Up = 1, Ladder_Down = 2, Grab = 3
    }
    public enum MouseInput
    {
        LeftClick = 0, RightClick = 1
    }
    public enum CircleActionState
    {
        None, Unavailable, ShowAvailable, Available
    }
    #endregion

    #region Player [State]

    public enum PlayerActionInMind
    {
        Moving = 0, UseAbility = 1, MovingTable = 2
    }

    public enum UnitPrimaryState
    {
        Idle = 0, Walk = 1, Busy = 2
    }

    public enum UnitActionState
    {
        None = 0, ClimbingLadder = 1, ClimbingChair = 2, ClimbingWall = 3, ClimbingTable = 4, MovingTable = 5
    }
    public enum UnitActionInMind
    {
        None = 0, ClimbingLadder = 1, ClimbingChair = 2, ClimbingWall = 3,
        ClimbTable = 4, ClimbDownTable = 5, MovingTable = 6, DropTable = 7
    }

    public enum UnitFeetState
    {
        OnGround = 0, OnStairs = 1, OnChair = 2, OnLadder = 3, OnTable = 4, InAir = 11
    }

    #endregion

    public enum ActionType
    {
        None = 0, Ladder = 1, ChairClimb = 2, ChairGrab = 3, LedgeClimb = 4, TableClimb = 5, GrabTable = 6
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

    public enum LedgeBottomPoint
    {
        Map = 0, Table = 1, Impediment = 2, Nothing = 3
    }

    public enum WallClimb_Animations
    {
        WallClimb_2Metters = 0, WallClimbDown_2Metters = 1
    }

    #endregion

    #region Table

    public enum TableState
    {
        Static = 0, Moving = 1
    }

    public enum TableEdge
    {
        Table_Side_Collider = 0,
        Table_Side_Collider_L = 1,
        Table_End_Collider = 2,
        Table_End_Collider_F = 3,

        Table_Top_Collider = 4
    }

    public enum TableStartPoint
    {
        Bottom = 0, Top = 1, OutOfReach = 2
    }

    public enum TableAnimations
    {
        Climb_Table = 0, ClimbDown_Table = 1,

        LiftTable_FromBack = 2,
        Idle_Table = 3,
        DropTable_FromBack = 4,

        Move_Table = 5,

        Rotate_Table_Left = 6,
        Rotate_Table_Right = 7
    }

    public enum TableActionStartPoint
    {
        Table_StartPos_Forward = 0, Table_StartPos_Back = 1
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

        public static int GetClosestYPos(float UnitYPos, float groundYPos, float Ypos)
        {
            int returnValue = 0;

            if (UnitYPos < groundYPos || UnitYPos > Ypos + 0.5f)
            {
                return 2;   // OutOfReach
            }

            var middlePosY = groundYPos + ((Ypos - groundYPos) / 2);

            if (UnitYPos < middlePosY)
            {
                returnValue = 0;
            }
            else
            {
                returnValue = 1;
            }

            return returnValue;
        }

        public static Vector3 GetEdgePosition(Vector3 lastPosition, Vector3 forward, float Ypos_compare, float Ypos)
        {
            var hitPoint = GetPointHitAtMousePosition();

            hitPoint = new Vector3(Mathf.Round(hitPoint.x * 100f) / 100f, Mathf.Round(hitPoint.y * 100f) / 100f, Mathf.Round(hitPoint.z * 100f) / 100f);
            if (lastPosition != hitPoint)
            {
                lastPosition = hitPoint;

                if (Mathf.Round(hitPoint.y) == Mathf.Round(Ypos_compare))
                {
                    var _UILinePosition = new Vector3(hitPoint.x, Ypos, hitPoint.z);

                    var One_meterInFront = (Vector3)(forward + _UILinePosition);
                    var One_meterInFront_Half_metterDown = One_meterInFront + new Vector3(0, -0.5f, 0);
                    var Half_metterDown = _UILinePosition + new Vector3(0, -0.5f, 0);

                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(One_meterInFront_Half_metterDown, (Half_metterDown - One_meterInFront_Half_metterDown)), out hit, 10))
                    {
                        return hit.point;
                    }
                }
                else
                {
                    return hitPoint;
                }
            }
            return Vector3.zero;
        }

        public static GameObject InstantiateEdgeUI(Vector3 position, Vector3 rotation, Vector3 scale, string name)
        {
            var _object = GameObject.Instantiate(Resources.Load("Prefabs/UI/WallClimb"), position, Quaternion.identity) as GameObject;

            _object.name = name;
            _object.transform.eulerAngles = rotation;

            if (scale != Vector3.zero)
                _object.transform.localScale = scale;

            return _object;
        }

        public static Vector3 GetDirection(Vector3 v_From, Vector3 v_To)
        {
            return v_To - v_From;
        }

        public static Quaternion SmoothLook(Quaternion rotation, Vector3 newDirection, float speed)
        {
            return Quaternion.Lerp(rotation, Quaternion.LookRotation(newDirection), Time.deltaTime * speed);
        }

        public static Vector3 GetPointHitAtMousePosition()
        {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Ray, out Hit, 100))
            {
                return Hit.point;
            }
            return Vector3.zero;
        }
    }
}
