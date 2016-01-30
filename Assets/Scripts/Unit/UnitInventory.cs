using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UnitInventory : MonoBehaviour
{
    [HideInInspector]
    public Unit Unit;

    [HideInInspector]
    public List<Item> InventoryItems;

    [HideInInspector]
    public InventoryBox[,] LeftPocket;
    [HideInInspector]
    public InventoryBox[,] RightPocket;
    [HideInInspector]
    public InventoryBox[,] Backpack;

    private Item _inventoryObjectInHand;
    [HideInInspector]
    public Item InventoryObjectInHand
    {
        get
        {
            return _inventoryObjectInHand;
        }
        set
        {
            _inventoryObjectInHand = value;
            if (value == null)
            {
                GlobalData.Player.UnitPrimaryState = UnitPrimaryState.Idle;
                GlobalData.Player.UnitActionState = UnitActionState.None;
                GlobalData.CameraControl.HUD.InventoryDropArea.gameObject.SetActive(false);

                return;
            }

            GlobalData.Player.UnitPrimaryState = UnitPrimaryState.Busy;
            GlobalData.Player.UnitActionState = UnitActionState.MovingItemInInventory;

            GlobalData.CameraControl.HUD.InventoryDropArea.gameObject.SetActive(true);
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = true;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;
        }
    }

    private Sprite IBox_active;
    private Sprite IBox_inactive;

    private bool isLastItemValid = true;

    private int newH = 0;
    private int newX = 0;

    public void Initialize(Unit unit)
    {
        Unit = unit;

        InventoryItems = new List<Item>();

        IBox_active = Resources.Load<Sprite>("HUD/InventoryBox");
        IBox_inactive = Resources.Load<Sprite>("HUD/InventoryBox_O");
    }

    public void RemoveInventoryItems()
    {
        var inventory = getInventoryGroupArray(InventoryObjectInHand.InventoryGroup);
        inventory = removeFromInventorySpace(inventory);

        InventoryObjectInHand.InventoryObject.Image.transform.SetParent(GlobalData.CameraControl.HUD.PendingInventory.transform);
        InventoryItems.Remove(InventoryObjectInHand);
    }

    public void CalculateSpace(int posH, int posX, InventoryGroup inventoryGroup)
    {
        InventoryObjectInHand.originH = posH;
        InventoryObjectInHand.originX = posX;
        InventoryObjectInHand.InventoryGroup = inventoryGroup;

        newH = 0;
        newX = 0;

        // If the item doesn't fit, return and dont color anything;
        if (checkValidity() == false)
        {
            isLastItemValid = false;
            return;
        }
        else
        {
            isLastItemValid = true;

            // show the InventoryItem where it will be;
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = true;

            // color the InventoryBoxes as black;
            var inventory = getInventoryGroupArray(inventoryGroup);
            inventory = showInventorySpace(inventory);
        }
    }

    public void CalculateSpaceExit(int posH, int posX, InventoryGroup inventoryGroup)
    {
        if (isLastItemValid == false)
        {
            return;
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = true;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;

            var inventory = getInventoryGroupArray(inventoryGroup);
            inventory = hideInventorySpace(inventory);
        }
    }

    // This function is used for when u pick up and object.
    public bool FindSpaceInInventory(Item item, out Item returnItem)
    {
        for (var h = 3; h >= 0; h--)
        {
            for (var x = 0; x < 2; x++)
            {
                var igLength = 3;   //  None, LeftPocket, RightPocket
                if (GlobalData.Player.hasBackPack)
                    igLength = igLength + 1;
                if (GlobalData.Player.hasJacket)
                    igLength = igLength + 2;

                for (var ig = 1; ig < igLength; ig++)
                {
                    // If we find space for the item, then return true as operation completed successfully;
                    item.originH = h;
                    item.originX = x;
                    item.InventoryGroup = (InventoryGroup)ig;
                    if (checkValidity(item))
                    {
                        item.originH = h;
                        item.originX = x;
                        item.InventoryGroup = (InventoryGroup)ig;

                        returnItem = item;
                        this.InventoryObjectInHand = item;
                        isLastItemValid = true;
                        return true;
                    }
                }
            }
        }

        returnItem = item;
        isLastItemValid = false;
        return false;
    }

    public bool checkValidity(Item item = null)
    {
        if (item == null)
            item = InventoryObjectInHand;

        int maxX = 2;
        var inventory = getInventoryGroupArray(item.InventoryGroup);
        int blockedH;
        int blockedX;

        if (item.InventoryGroup == InventoryGroup.LeftPocket)
        {
            blockedH = 0;
            blockedX = 1;
        }
        else if (item.InventoryGroup == InventoryGroup.RightPocket)
        {
            blockedH = 0;
            blockedX = 0;
        }
        else
        {
            //maxH = 8;
            maxX = 4;

            // there are multiple
            blockedH = 0;
            blockedX = 0;
        }

        for (var H = item.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= item.spaceX; X++)
            {
                newH = item.originH - H;
                newX = item.originX + X;
                if (newH >= 0 && newX < maxX)   // Test for bounds
                {
                    if (newH == blockedH && newX == blockedX)   // test for the blocked inventoryBox;
                    {
                        return false;
                    }
                    //occupied = inventory[newH, newX].Ocupied;
                    if (inventory[newH, newX].Ocupied)
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlaceInSpace(int? posH = null, int? posX = null, InventoryGroup pendingInventoryGroup = InventoryGroup.None)
    {
        // this can be from a click event
        if (posH.HasValue && posX.HasValue)
        {
            InventoryObjectInHand.originH = posH.Value;
            InventoryObjectInHand.originX = posX.Value;
        }

        if (InventoryObjectInHand.isInInventory == true && checkValidity() == false)
        {
            return;
        }
        if (InventoryObjectInHand.isInInventory == false && isLastItemValid == false)
        {
            return;
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;

            if (pendingInventoryGroup != InventoryGroup.None)
                InventoryObjectInHand.InventoryGroup = pendingInventoryGroup;

            var inventory = getInventoryGroupArray(InventoryObjectInHand.InventoryGroup);
            inventory = placeInventorySpace(inventory);

            // We use the pending for when you pick up the object from the inventory and then u right click to cancel. Then we just put the item back.
            InventoryObjectInHand.PlaceInInventory();
            InventoryObjectInHand = null;
        }
    }

    private InventoryBox[,] getInventoryGroupArray(InventoryGroup InventoryGroup)
    {
        switch (InventoryGroup)
        {
            case InventoryGroup.LeftPocket:
                return LeftPocket;
            case InventoryGroup.RightPocket:
                return RightPocket;
            case InventoryGroup.Backpack:
                return Backpack;
            case InventoryGroup.JacketLeftPocket:
                break;
            case InventoryGroup.JacketRightPocket:
                break;
            default:
                break;
        }

        return null;
    }
    private InventoryBox[,] showInventorySpace(InventoryBox[,] array)
    {
        InventoryObjectInHand.InventoryObject.transform.position = CenterItemImage(array);

        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                newH = InventoryObjectInHand.originH - H;
                newX = InventoryObjectInHand.originX + X;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
            }
        }
        return array;
    }
    private InventoryBox[,] placeInventorySpace(InventoryBox[,] array)
    {
        InventoryObjectInHand.InventoryObject.transform.position = CenterItemImage(array);

        InventoryObjectInHand.ObjectState = ObjectState.InInventory;

        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                newH = InventoryObjectInHand.originH - H;
                newX = InventoryObjectInHand.originX + X;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
                array[newH, newX].Ocupied = true;
            }
        }
        return array;
    }
    public Vector3 CenterItemImage(InventoryBox[,] array)
    {
        // The Item image;
        // Midpoint beween to vectors -> http://www.leadinglesson.com/midpoint-between-two-vectors
        return (Vector3)(
                        (array[InventoryObjectInHand.originH, InventoryObjectInHand.originX].Image.transform.position +
                         array[InventoryObjectInHand.originH - InventoryObjectInHand.spaceH, InventoryObjectInHand.originX + InventoryObjectInHand.spaceX].Image.transform.position)
                         / 2);
    }
    private InventoryBox[,] hideInventorySpace(InventoryBox[,] array)
    {
        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                newH = InventoryObjectInHand.originH - H;
                newX = InventoryObjectInHand.originX + X;
                array[newH, newX].Image.overrideSprite = IBox_active;
            }
        }
        return array;
    }
    private InventoryBox[,] removeFromInventorySpace(InventoryBox[,] array)
    {
        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                newH = InventoryObjectInHand.originH - H;
                newX = InventoryObjectInHand.originX + X;
                array[newH, newX].Image.overrideSprite = IBox_active;
                array[newH, newX].Ocupied = false;
            }
        }
        return array;
    }
}