using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class CameraCursor : MonoBehaviour
{
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

    public bool showDropItemLocation;

    public bool drawInventoryItem;
    public bool showItemInInventory;

    public void Initialize()
    {
        Cursor.visible = false;

        no_Cursor = new Texture2D(1, 1);
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
            if (curentCursor != null)
                GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorSizeX / 2.0f, Event.current.mousePosition.y - cursorSizeY / 2.0f, cursorSizeX, cursorSizeY), curentCursor);
        }
        if (drawInventoryItem && showItemInInventory == false)
        {
            GlobalData.Player.UnitInventory.InventoryObjectInHand.InventoryObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + 22, Input.mousePosition.y + 22, 10.0f));
        }
    }

    public void ChangeCursor(CursorType cursorType)
    {
        if (GlobalData.Player != null && GlobalData.Player.PlayerActionInMind != PlayerActionInMind.UseAbility)
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

    public Vector3 item3DPosition = new Vector3(0, 0, 0);
    public bool firstTimeMoving3DObject = true;

    void Update()
    {
        if (showDropItemLocation)
        {
            if (GlobalData.Player.UnitInventory.InventoryObjectInHand != null)
            {
                RaycastHit Hit;
                Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(Ray, out Hit, 100))
                {
                    if (Hit.transform.gameObject.tag == "Map" || Hit.transform.gameObject.tag == "Table")
                    {
                        item3DPosition = new Vector3(Mathf.Round(Hit.point.x * 100f) / 100f, Mathf.Round((Hit.point.y + 0.025f) * 100f) / 100f, Mathf.Round(Hit.point.z * 100f) / 100f);
                        if (firstTimeMoving3DObject)
                        {
                            firstTimeMoving3DObject = false;
                            GlobalData.Player.UnitInventory.InventoryObjectInHand.InteractiveObject.transform.position = item3DPosition;
                        }
                    }
                }

                var curPos = GlobalData.Player.UnitInventory.InventoryObjectInHand.InteractiveObject.transform.position;

                GlobalData.Player.UnitInventory.InventoryObjectInHand.InteractiveObject.transform.position = Vector3.Lerp(curPos, item3DPosition, Time.deltaTime * 8f);

            }
        }
    }
}
