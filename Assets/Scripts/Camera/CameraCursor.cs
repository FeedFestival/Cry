using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CameraCursor : MonoBehaviour {

    /*
        This script deals with the change of the cursor when hovering certain objects in the scene.
     * - Its parameters are diferent cursor textures.
     * - The function its called from the object in question that senses the 'hover'.
     */
    [HideInInspector]
    public CursorType lastCursor = CursorType.Default;

    public Texture2D curentCursor;

    // Normal Cursor
    [HideInInspector]
    public Texture2D defaultCursor;
    [HideInInspector]
    public Texture2D no_Cursor;

    // DoorCursr
    [HideInInspector]
    public Texture2D Door_Cursor;

    // Ladder Cursor
    [HideInInspector]
    public Texture2D Ladder_Up_Cursor;
    [HideInInspector]
    public Texture2D Ladder_Down_Cursor;

    // Grab Cursor
    [HideInInspector]
    public Texture2D Grab_Cursor;

    int cursorSizeX = 88;   // 48
    int cursorSizeY = 111;

    public void Initialize()
    {
        Cursor.visible = false;

        no_Cursor = new Texture2D(1,1);
        defaultCursor = Resources.Load("Cursor/_Cursor") as Texture2D;
        Grab_Cursor = Resources.Load("Cursor/Cursor_Hand") as Texture2D;
        Door_Cursor = Resources.Load("Cursor/Cursor_Door") as Texture2D;

        Ladder_Up_Cursor = Resources.Load("Cursor/Cursor_Ladder_Up") as Texture2D;
        Ladder_Down_Cursor = Resources.Load("Cursor/Cursor_Ladder_Down") as Texture2D;

        curentCursor = defaultCursor;
        ChangeCursor(CursorType.Default);
    }

    void OnGUI()
    {
        if (lastCursor != CursorType.None)
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorSizeX / 2, Event.current.mousePosition.y - cursorSizeY / 2, cursorSizeX, cursorSizeY)
                            , curentCursor);
        }
    }

    public void ChangeCursor(CursorType cursorType)
    {
        if (GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
        {
            if (lastCursor != cursorType)
            {
                lastCursor = cursorType;

                switch (cursorType)
                {
                    case CursorType.Default:
                        curentCursor = defaultCursor;
                        break;
                    case CursorType.Ladder_Up:
                        curentCursor = Ladder_Up_Cursor;
                        break;
                    case CursorType.Ladder_Down:
                        curentCursor = Ladder_Down_Cursor;
                        break;
                    case CursorType.Grab:
                        curentCursor = Grab_Cursor;
                        break;
                    case CursorType.None:
                        curentCursor = no_Cursor;
                        break;
                    default:
                        curentCursor = defaultCursor;
                        break;
                }
            }
        }
    }
}
