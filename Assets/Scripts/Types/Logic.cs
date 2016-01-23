using System;
using System.Collections;
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
    public enum CameraEdge
    {
        T, TR, R, DR, D, DL, L, TL
    }
    public enum CameraEdgeSpeed
    {
        Slow, Fast
    }
    public enum ColliderMouseState
    {
        Out = 0, Hover = 1
    }
    public enum CircleActionState
    {
        None, Unavailable, ShowAvailable, Available
    }

    public enum ButtonName
    {
        None = 0, ESC_COG = 1, W_HANDS = 2,
        I_INVENTORY = 8
    }

    #endregion

    #region InventoryItems

    public enum InventoryGroup
    {
        None, LeftPocket, RightPocket, Backpack, JacketLeftPocket, JacketRightPocket
    }

    public enum ObjectState
    {
        InInventory, OnGround, OnTable, OnShelf
    }

    public enum InventorySpace
    {
        Square, Square2, VerticalLine2, HorizontalLine2,
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
        ClimbTable = 4, ClimbDownTable = 5, MovingTable = 6, DropTable = 7, PickupObject = 8
    }

    public enum UnitFeetState
    {
        OnGround = 0, OnStairs = 1, OnChair = 2, OnLadder = 3, OnTable = 4, InAir = 11
    }

    #endregion

    public enum ActionType
    {
        None = 0, Ladder = 1, ChairClimb = 2, ChairGrab = 3, LedgeClimb = 4, TableClimb = 5, GrabTable = 6, PickupObject = 7
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

    public enum LedgeState
    {
        Static = 0, UnitOn = 1
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

    public enum TableMovementAction
    {
        Move = 0, RotateRight = 1, RotateLeft = 2
    }

    #endregion

    public enum DialogBoxType
    {
        RightSideSentence = 0, LeftSide_1LargeSentence = 1, LeftSide_2Sentence = 2, LeftSide_1SmallSentence = 3, LeftSide_1MediumSentence = 4
    }

    public enum ActorName
    {
        John = 0, Father = 1, Mother = 2
    }

    /***********************/
    //        ACTING STUFF
    /***********************/

    public class ActObject
    {
        public bool nextImmediate;
        public int nextImmediateIndex;
        public int nextIndex;
        public bool endPoint;

        public Actor Actor;

        public bool hasAnimation;
        public string animString;
        public float animTime;

        public bool hasLine;
        public string Line;
        public float lineTime;
        public DialogBoxType DialogBoxType;

        public float Time;

        public bool hasPauseAfter;
        public float PauseLength;
    }

    public enum ActType
    {
        Pause, Talk, Animate
    }

    public enum Talk
    {
        Talk1, Talk2, Talk3
    }

    public enum LinePause
    {
        NoPause = 0, Pause1 = 1
    }

    /***********************/
    //        ACTING STUFF  -   END
    /***********************/

    public static class Logic
    {
        // UI functions


        // 3d functions

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

            if (UnitYPos < (groundYPos - 0.25f) || UnitYPos > (Ypos + 0.25f))
            {
                return 2;   // OutOfReach
            }

            var middlePosY = groundYPos + ((Ypos - groundYPos) / 2);

            if (UnitYPos < middlePosY)
            {
                returnValue = 0;    // Bottom
            }
            else
            {
                returnValue = 1;    // Top
            }

            return returnValue;
        }

        public static Vector3 GetEdgePosition(Vector3 lastPosition, Transform Edge, float Ypos)
        {
            var hitPoint = GetPointHitAtMousePosition();

            hitPoint = new Vector3(Mathf.Round(hitPoint.x * 100f) / 100f, Mathf.Round(hitPoint.y * 100f) / 100f, Mathf.Round(hitPoint.z * 100f) / 100f);
            if (lastPosition != hitPoint)
            {
                lastPosition = hitPoint;

                // If we hit the top side of the edge we need to put the point on the EDGE !
                if (Mathf.Round(hitPoint.y) == Mathf.Round(Edge.position.y))
                {
                    // A = is where the ray hits
                    // B = is the Edge point 
                    // C = is thebackward position of the edge 
                    // http://mate123.ro/formule-matematice-geometrie-generala/teorema-inaltimii./

                    Vector3 back = -Edge.forward;

                    var A = new Vector3(hitPoint.x, Ypos, hitPoint.z);
                    var B = Edge.position;
                    var C = (Vector3)B + back;

                    var AC = Vector3.Distance(A, C);
                    var AB = Vector3.Distance(A, B);
                    var BC = Vector3.Distance(B, C);    // base of the triangle.

                    var s = (AC + AB + BC) / 2.0d;
                    var Area = Math.Sqrt(s * (s - AC) * (s - AB) * (s - BC));   //  Find the Area using Heron's formula http://socoder.net/?topic=2274

                    var AD = (float)(Area / (BC / 2));    // height    http://www.mathwarehouse.com/geometry/triangles/area/find-height-of-triangle-how-to.php

                    // we use AC and AD and the (angle between AD and DC)
                    // https://www.mathsisfun.com/algebra/trig-solving-ssa-triangles.html

                    //  Ipotenuza (latura care este opusa unghiuliu de 90 de grade) la patrat este egala cu suma patratelor catetelor
                    // AC^2 = AD^2 + DC^2   //  Pithagora
                    var DC = Mathf.Sqrt(Mathf.Pow(AC, 2) - Mathf.Pow(AD, 2));

                    var BD = BC - DC;   //  Lenght from the point to the edge.

                    // we found out how much we want to set the point forward. thats BD
                    // E is one meter in front for vector A
                    var E = (Vector3)A + Edge.forward;
                    var AE = Vector3.Distance(E, A);
                    // now decrease AE to be the size of BD http://www.teacherschoice.com.au/Maths_Library/Analytical%20Geometry/AnalGeom_3.htm
                    var k1 = AE - BD;
                    var x = ((k1 * A.x) + (BD * E.x)) / (k1 + BD);
                    var y = ((k1 * A.y) + (BD * E.y)) / (k1 + BD);
                    var z = ((k1 * A.z) + (BD * E.z)) / (k1 + BD);

                    // And we get the point on the edge.
                    var ThePointOnTheEdge = new Vector3(x, y, z);

                    return ThePointOnTheEdge;
                }
                else
                {
                    return hitPoint;
                }
            }
            return Vector3.zero;
        }

        // Point A, Point B, Length of AB, How much you want the line to be*
        public static Vector3 IncreaseOrDecreaseLine(Vector3 A, Vector3 B, float AB, float AB_desiredLength)
        {
            float x, y, z = 0f;
            if (AB < AB_desiredLength)  // For increase in length
            {
                var k1 = AB_desiredLength - AB;

                x = B.x + (B.x - A.x) / AB * k1;
                y = A.y;
                z = B.z + (B.z - A.z) / AB * k1;
            }
            else
            {   //  For decreasing the lenght : http://www.teacherschoice.com.au/Maths_Library/Analytical%20Geometry/AnalGeom_3.htm

                var k1 = AB - AB_desiredLength;

                x = ((k1 * A.x) + (AB_desiredLength * B.x)) / (k1 + AB_desiredLength);
                y = ((k1 * A.y) + (AB_desiredLength * B.y)) / (k1 + AB_desiredLength);
                z = ((k1 * A.z) + (AB_desiredLength * B.z)) / (k1 + AB_desiredLength);
            }
            return new Vector3(x, y, z);
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
                return new Vector3(Mathf.Round(Hit.point.x * 100f) / 100f, Mathf.Round(Hit.point.y * 100f) / 100f, Mathf.Round(Hit.point.z * 100f) / 100f);
            }
            return Vector3.zero;
        }
    }
}