using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class InteractiveObject : MonoBehaviour
{
    [HideInInspector]
    public Item Item;

    public ItemName ItemName;
    public ItemType ItemType;

    public int Password = 0;

    [HideInInspector]
    public GameObject ItemIndicator;

    public void Initialize(Item item = null)
    {
        if ((ItemType == ItemType.Key || ItemType == ItemType.QuestItem) && Password == 0)
        {
            Debug.LogError("Key ("+ this.gameObject.name +") has no password and can't open any door. or pose any value in a quest.");
        }

        if (item == null)
        {
            this.Item = Items.CreateItem(this.ItemName, this.ItemType, this.Password);
            setSettings();
            Item.InteractiveObject = this;
        }
        else
        {
            this.Item = item;
            Item.ObjectState = ObjectState.InInventory;
        }
        ItemIndicator = this.transform.GetChild(this.transform.childCount - 1).gameObject;
        ItemIndicator.SetActive(false);
    }

    public void setSettings()
    {
        /* Settings */
        //--------------------
        Item.objectPosition = this.transform.position;
   
        int mask = (1 << LayerMask.NameToLayer("Map"));
        RaycastHit hit;
        if (Physics.Linecast(Item.objectPosition, new Vector3(Item.objectPosition.x, 0, Item.objectPosition.z), out hit, mask))
        {
            Item.objectNavMeshPosition = hit.point;
        }

        // check to see if object is standing on something.
        var dif = Item.objectPosition.y - Item.objectNavMeshPosition.y;
        if (dif < 0.3f)
        {
            Item.ObjectState = ObjectState.OnGround;
        }
        else if (dif > 1f && dif < 2f)
        {
            Item.ObjectState = ObjectState.OnShelf;
        }
        else if (dif < 1f)
        {
            Item.ObjectState = ObjectState.OnTable;
        }
    }

    private void Pickup()
    {
        var playerPosition = GlobalData.Player.UnitProperties.ThisUnitTransform.position;
        var length = Vector3.Distance(playerPosition, Item.objectPosition);

        if (length > 7)
            return;

        switch (Item.ObjectState)
        {
            case ObjectState.InInventory:
                break;
            case ObjectState.OnGround:

                var path = GlobalData.Player.UnitController.GetNavMeshPathCorners(playerPosition, Item.objectNavMeshPosition);

                if (path != null && path.Length != 0)
                {
                    Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, path[path.Length - 2], length, 0.7f);
                    GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                }
                break;

            case ObjectState.OnTable:

                Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong. // TO_DO
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            case ObjectState.OnShelf:

                Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong. // TO_DO
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            default:
                break;
        }
    }

    void OnMouseOver()
    {
        if (GlobalData.Player.UnitActionState != UnitActionState.MovingItemInInventory)
        {
            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                Pickup();
            }
        }
    }

    void OnMouseEnter()
    {
        if (Item.ObjectState != ObjectState.InInventory)
        {
            ItemIndicator.SetActive(true);
            ItemIndicator.transform.LookAt(ItemIndicator.transform.position + GlobalData.CameraControl.thisTransform.rotation * (Vector3.forward), GlobalData.CameraControl.thisTransform.rotation * Vector3.up);
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.None);
        }
    }

    void OnMouseExit()
    {
        if (Item.ObjectState != ObjectState.InInventory)
        {
            ItemIndicator.SetActive(false);
            GlobalData.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
        }
    }
}