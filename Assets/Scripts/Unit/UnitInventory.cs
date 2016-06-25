using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;
using System.Collections.Generic;
using System.Linq;
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

    private Sprite _iBoxActive;
    private Sprite _iBoxInactive;

    private bool _isLastItemValid = true;

    private int _newH;
    private int _newX;

    public void Initialize(Unit unit)
    {
        Unit = unit;

        InventoryItems = new List<Item>();

        _iBoxActive = Resources.Load<Sprite>("HUD/InventoryBox");
        _iBoxInactive = Resources.Load<Sprite>("HUD/InventoryBox_O");
    }

    public void RemoveInventoryItems()
    {
        var inventory = GetInventoryGroupArray(InventoryObjectInHand.InventoryGroup);
        inventory = RemoveFromInventorySpace(inventory);

        InventoryObjectInHand.InventoryObject.Image.transform.SetParent(GlobalData.CameraControl.HUD.PendingInventory.transform);
        InventoryItems.Remove(InventoryObjectInHand);
    }

    public void CalculateSpace(int posH, int posX, InventoryGroup inventoryGroup)
    {
        InventoryObjectInHand.originH = posH;
        InventoryObjectInHand.originX = posX;
        InventoryObjectInHand.InventoryGroup = inventoryGroup;

        _newH = 0;
        _newX = 0;

        // If the item doesn't fit, return and dont color anything;
        if (CheckValidity() == false)
        {
            _isLastItemValid = false;
        }
        else
        {
            _isLastItemValid = true;

            // show the InventoryItem where it will be;
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = true;

            // color the InventoryBoxes as black;
            var inventory = GetInventoryGroupArray(inventoryGroup);
            inventory = ShowInventorySpace(inventory);
        }
    }

    public void CalculateSpaceExit(int posH, int posX, InventoryGroup inventoryGroup)
    {

        if (_isLastItemValid == false)
        {
            return;
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = true;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;

            var inventory = GetInventoryGroupArray(inventoryGroup);
            inventory = HideInventorySpace(inventory);
        }
    }

    // This function is used for when u pick up and object.
    public bool FindSpaceInInventory(Item item, out Item returnItem)
    {
        var igLength = 3;   //  None, LeftPocket, RightPocket
        if (GlobalData.Player.MainCharacterProperties.Contains(MainCharacterProperties.HasBackPack))
            igLength = igLength + 1;
        if (GlobalData.Player.MainCharacterProperties.Contains(MainCharacterProperties.HasJacket))
            igLength = igLength + 2;

        for (var ig = 1; ig < igLength; ig++)
        {
            for (var h = 3; h >= 0; h--)
            {
                for (var x = 0; x < 2; x++)
                {
                    // If we find space for the item, then return true as operation completed successfully;
                    item.originH = h;
                    item.originX = x;
                    item.InventoryGroup = (InventoryGroup)ig;
                    if (CheckValidity(item))
                    {
                        item.originH = h;
                        item.originX = x;
                        item.InventoryGroup = (InventoryGroup)ig;

                        returnItem = item;
                        this.InventoryObjectInHand = item;
                        _isLastItemValid = true;
                        return true;
                    }
                }
            }
        }

        returnItem = item;
        _isLastItemValid = false;
        return false;
    }

    public bool CheckValidity(Item item = null)
    {
        if (item == null)
            item = InventoryObjectInHand;

        int maxX = 2;
        var inventory = GetInventoryGroupArray(item.InventoryGroup);
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
                _newH = item.originH - H;
                _newX = item.originX + X;
                if (_newH >= 0 && _newX < maxX)   // Test for bounds
                {
                    if (_newH == blockedH && _newX == blockedX)   // test for the blocked inventoryBox;
                    {
                        return false;
                    }
                    //occupied = inventory[_newH, _newX].Ocupied;
                    if (inventory[_newH, _newX].Ocupied)
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

        if (InventoryObjectInHand.isInInventory == true && CheckValidity() == false)
        {
            return;
        }
        if (InventoryObjectInHand.isInInventory == false && _isLastItemValid == false)
        {
            return;
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;

            if (pendingInventoryGroup != InventoryGroup.None)
                InventoryObjectInHand.InventoryGroup = pendingInventoryGroup;

            var inventory = GetInventoryGroupArray(InventoryObjectInHand.InventoryGroup);
            inventory = PlaceInventorySpace(inventory);

            InventoryItems.Add(InventoryObjectInHand);

            // We use the pending for when you pick up the object from the inventory and then u right click to cancel. Then we just put the item back.
            InventoryObjectInHand.PlaceInInventory();
            InventoryObjectInHand = null;
        }
    }

    private InventoryBox[,] GetInventoryGroupArray(InventoryGroup inventoryGroup)
    {
        switch (inventoryGroup)
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
    private InventoryBox[,] ShowInventorySpace(InventoryBox[,] array)
    {
        InventoryObjectInHand.InventoryObject.transform.position = CenterItemImage(array);

        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                _newH = InventoryObjectInHand.originH - H;
                _newX = InventoryObjectInHand.originX + X;
                array[_newH, _newX].Image.overrideSprite = _iBoxInactive;
            }
        }
        return array;
    }
    private InventoryBox[,] PlaceInventorySpace(InventoryBox[,] array)
    {
        InventoryObjectInHand.InventoryObject.transform.position = CenterItemImage(array);

        InventoryObjectInHand.ObjectState = ObjectState.InInventory;

        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                _newH = InventoryObjectInHand.originH - H;
                _newX = InventoryObjectInHand.originX + X;
                array[_newH, _newX].Image.overrideSprite = _iBoxInactive;
                array[_newH, _newX].Ocupied = true;
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
    private InventoryBox[,] HideInventorySpace(InventoryBox[,] array)
    {
        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                _newH = InventoryObjectInHand.originH - H;
                _newX = InventoryObjectInHand.originX + X;
                array[_newH, _newX].Image.overrideSprite = _iBoxActive;
            }
        }
        return array;
    }
    private InventoryBox[,] RemoveFromInventorySpace(InventoryBox[,] array)
    {
        for (var H = InventoryObjectInHand.spaceH; H >= 0; H--)
        {
            for (var X = 0; X <= InventoryObjectInHand.spaceX; X++)
            {
                _newH = InventoryObjectInHand.originH - H;
                _newX = InventoryObjectInHand.originX + X;
                array[_newH, _newX].Image.overrideSprite = _iBoxActive;
                array[_newH, _newX].Ocupied = false;
            }
        }
        return array;
    }
}