using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UnitInventory : MonoBehaviour
{
    public List<InventoryObject> Inventory;

    public InventoryBox[,] LeftPocket;
    public InventoryBox[,] RightPocket;
    public InventoryBox[,] Backpack;

    private InventoryObject _inventoryObjectInHand;
    public InventoryObject InventoryObjectInHand
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
                return;
            }
            
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = true;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;
        }
    }

    private Sprite IBox_active;
    private Sprite IBox_inactive;

    private bool isLastItemValid;

    private int newH = 0;
    private int newX = 0;

    void Awake()
    {
        Initialize();
    }

    void Start() {
        Inventory = new List<InventoryObject>();
        Inventory.Add(CreateInventoryObject());
    }

    public void Initialize()
    {
        IBox_active = Resources.Load<Sprite>("HUD/InventoryBox");
        IBox_inactive = Resources.Load<Sprite>("HUD/InventoryBox_O");
    }

    public InventoryObject CreateInventoryObject()
    {
        var _object = new InventoryObject
        {
            InventoryObjectState = InventoryObjectState.InInventory,
            Name = "Apple",
            originH = 2, originX = 0,
            InventoryGroup = InventoryGroup.LeftPocket,
            InventorySpace = InventorySpace.Square2
        };
        var goImage = GameObject.Instantiate(Resources.Load("Prefabs/UI/InventoryItem"), Vector3.zero, GlobalData.CameraControl.HUD.InventoryList.transform.rotation) as GameObject;

        _object.InventoryObject2D = goImage.transform.GetComponent<GOInventoryItem>();
        _object.InventoryObject2D.Image = goImage.GetComponent<Image>();

        // we modify the parent so we can click on the box instead of the item;
        _object.InventoryObject2D.Image.transform.parent = GlobalData.CameraControl.HUD.InventoryList.transform;
        
        // we assing the image to the apple;
        _object.InventoryObject2D.Image.overrideSprite = Resources.Load<Sprite>("InventoryItems/Apple");
        _object.InventoryObject2D.Image.name = "Apple";

        _object.InventoryObject2D.Image.transform.localScale = new Vector3(1, 1, 1);
        _object.InventoryObject2D.Image.rectTransform.sizeDelta = new Vector2(42f, 42f);

        _object.InventoryObject2D.Initialize(_object);

        return _object;
    }

    // This function is used for when you click the inventory and you want to see your items displayed;
    public void PlaceInventoryItems()
    {
        foreach (InventoryObject io in Inventory)
        {
            InventoryObjectInHand = io;
            PlaceInSpace(io.originH, io.originX, io.InventoryGroup, true);
        }
    }

    public void RemoveInventoryItems(InventoryObject io)
    {
        switch (io.InventoryGroup)
        {
            case InventoryGroup.LeftPocket:

                LeftPocket = removeFromInventorySpace(LeftPocket, io.originH, io.originX);
                break;

            case InventoryGroup.RightPocket:

                RightPocket = removeFromInventorySpace(RightPocket, io.originH, io.originX);
                break;

            case InventoryGroup.Backpack:

                Backpack = removeFromInventorySpace(Backpack, io.originH, io.originX);
                break;

            default:
                break;
        }
        io.InventoryObject2D.Image.transform.parent = GlobalData.CameraControl.HUD.PendingInventory.transform;
        Inventory.Remove(io);
    }

    // This function is used for when u pick up and object.
    public bool FindSpaceInInventory(InventoryObject io)
    {
        for (var h = 0; h < 4; h++)
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
                    if (checkValidity(h, x, (InventoryGroup)ig, io.InventorySpace))
                    {
                        io.originH = h;
                        io.originX = x;
                        io.InventoryGroup = (InventoryGroup)ig;

                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void CalculateSpace(int posH, int posX, InventoryGroup inventoryGroup)
    {
        newH = 0;
        newX = 0;

        // If the item doesn't fit, return and dont color anything;
        if (checkValidity(posH, posX, inventoryGroup, InventoryObjectInHand.InventorySpace) == false)
        {
            isLastItemValid = false;
            return;
        }
        else
        {
            // show the InventoryItem where it will be;
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = true;

            // color the InventoryBoxes as black;
            isLastItemValid = true;
            switch (inventoryGroup)
            {
                case InventoryGroup.LeftPocket:

                    LeftPocket = showInventorySpace(LeftPocket, posH, posX);
                    break;

                case InventoryGroup.RightPocket:

                    RightPocket = showInventorySpace(RightPocket, posH, posX);
                    break;

                case InventoryGroup.Backpack:

                    Backpack = showInventorySpace(Backpack, posH, posX);
                    break;

                default:
                    break;
            }
        }
    }

    public void PlaceInSpace(int posH, int posX, InventoryGroup inventoryGroup, bool isInInventory = false)
    {
        if (isInInventory == true && checkValidity(posH, posX, inventoryGroup, InventoryObjectInHand.InventorySpace) == false)
        {
            return;
        }
        if (isInInventory == false && isLastItemValid == false)
        {
            return;
        }
        else
        {
            GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
            GlobalData.CameraControl.CameraCursor.showItemInInventory = false;

            switch (inventoryGroup)
            {
                case InventoryGroup.LeftPocket:

                    LeftPocket = placeInventorySpace(LeftPocket, posH, posX);
                    break;

                case InventoryGroup.RightPocket:

                    RightPocket = placeInventorySpace(RightPocket, posH, posX);
                    break;

                case InventoryGroup.Backpack:

                    Backpack = placeInventorySpace(Backpack, posH, posX);
                    break;

                default:
                    break;
            }
            InventoryObjectInHand.originH = posH;
            InventoryObjectInHand.originX = posX;
            InventoryObjectInHand.InventoryObject2D.Image.transform.parent = GlobalData.CameraControl.HUD.InventoryList.transform;
            InventoryObjectInHand.InventoryGroup = inventoryGroup;
            InventoryObjectInHand = null;
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

            switch (inventoryGroup)
            {
                case InventoryGroup.LeftPocket:

                    LeftPocket = hideInventorySpace(LeftPocket, posH, posX);
                    break;

                case InventoryGroup.RightPocket:

                    RightPocket = hideInventorySpace(RightPocket, posH, posX);
                    break;

                case InventoryGroup.Backpack:

                    Backpack = hideInventorySpace(Backpack, posH, posX);
                    break;

                default:
                    break;
            }
        }
    }

    public bool checkValidity(int y, int x, InventoryGroup InventoryGroup, InventorySpace InventorySpace)
    {
        int maxH = 4;
        int maxX = 2;

        int blockedH;
        int blockedX;

        if (InventoryGroup == InventoryGroup.LeftPocket)
        {
            blockedH = 0;
            blockedX = 1;
        }
        else if (InventoryGroup == InventoryGroup.RightPocket)
        {
            blockedH = 0;
            blockedX = 0;
        }
        else
        {
            maxH = 8;
            maxX = 4;

            // there are multiple
            blockedH = 0;
            blockedX = 0;
        }

        switch (InventorySpace)
        {
            case InventorySpace.Square:
                break;
            case InventorySpace.Square2:

                if ((y - 1 >= 0) && (x + 1 < maxX))
                {
                    // test for the blocked inventoryBox;
                    if (y - 1 == blockedH && x + 1 == blockedX)
                    {
                        return false;
                    }
                    else if (y == blockedH && x + 1 == blockedX)
                    {
                        return false;
                    }
                    else if (y - 1 == blockedH && x == blockedX)
                    {
                        return false;
                    }

                    var occupied = LeftPocket[y - 1, x].Ocupied;
                    occupied = LeftPocket[y - 1, x + 1].Ocupied;
                    occupied = LeftPocket[y, x + 1].Ocupied;

                    if (occupied)
                        return false;
                }
                else
                {
                    return false;
                }

                break;

            case InventorySpace.VerticalLine2:
                break;
            case InventorySpace.HorizontalLine2:
                break;
            default:
                break;
        }

        return true;
    }

    private InventoryBox[,] showInventorySpace(InventoryBox[,] array, int posH, int posX)
    {
        switch (InventoryObjectInHand.InventorySpace)
        {
            case InventorySpace.Square:

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_inactive;
                break;

            case InventorySpace.Square2:

                // The InventoryObject image;
                // Midpoint beween to vectors -> http://www.leadinglesson.com/midpoint-between-two-vectors
                InventoryObjectInHand.InventoryObject2D.transform.position = (Vector3)((array[posH, posX].Image.transform.position + array[posH - 1, posX + 1].Image.transform.position) / 2);

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_inactive;

                // Up
                newH = posH - 1;
                newX = posX;
                array[newH, newX].Image.overrideSprite = IBox_inactive;

                // Right
                newH = posH;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_inactive;

                // Up - Right
                newH = posH - 1;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
                break;

            case InventorySpace.VerticalLine2:
                break;
            case InventorySpace.HorizontalLine2:
                break;
            default:
                break;
        }
        return array;
    }
    private InventoryBox[,] placeInventorySpace(InventoryBox[,] array, int posH, int posX)
    {
        switch (InventoryObjectInHand.InventorySpace)
        {
            case InventorySpace.Square:

                // Origin
                array[posH, posX].Ocupied = true;

                break;

            case InventorySpace.Square2:

                // The InventoryObject image;
                // Midpoint beween to vectors -> http://www.leadinglesson.com/midpoint-between-two-vectors
                InventoryObjectInHand.InventoryObject2D.transform.position = (Vector3)((array[posH, posX].Image.transform.position + array[posH - 1, posX + 1].Image.transform.position) / 2);

                InventoryObjectInHand.InventoryObjectState = InventoryObjectState.InInventory;

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_inactive;
                array[posH, posX].Ocupied = true;
                // Up
                newH = posH - 1;
                newX = posX;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
                array[newH, newX].Ocupied = true;
                // Right
                newH = posH;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
                array[newH, newX].Ocupied = true;
                // Up - Right
                newH = posH - 1;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_inactive;
                array[newH, newX].Ocupied = true;
                break;

            case InventorySpace.VerticalLine2:
                break;
            case InventorySpace.HorizontalLine2:
                break;
            default:
                break;
        }
        return array;
    }
    private InventoryBox[,] hideInventorySpace(InventoryBox[,] array, int posH, int posX)
    {
        newH = 0;
        newX = 0;
        switch (InventoryObjectInHand.InventorySpace)
        {
            case InventorySpace.Square:

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_active;
                break;

            case InventorySpace.Square2:

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_active;

                // Up
                newH = posH - 1;
                newX = posX;
                array[newH, newX].Image.overrideSprite = IBox_active;

                // Right
                newH = posH;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_active;

                // Up - Right
                newH = posH - 1;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_active;
                break;

            case InventorySpace.VerticalLine2:
                break;
            case InventorySpace.HorizontalLine2:
                break;
            default:
                break;
        }
        return array;
    }
    private InventoryBox[,] removeFromInventorySpace(InventoryBox[,] array, int posH, int posX)
    {
        newH = 0;
        newX = 0;
        switch (InventoryObjectInHand.InventorySpace)
        {
            case InventorySpace.Square:

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_active;
                array[posH, posX].Ocupied = false;
                break;

            case InventorySpace.Square2:

                // Origin
                array[posH, posX].Image.overrideSprite = IBox_active;
                array[posH, posX].Ocupied = false;

                // Up
                newH = posH - 1;
                newX = posX;
                array[newH, newX].Image.overrideSprite = IBox_active;
                array[newH, newX].Ocupied = false;

                // Right
                newH = posH;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_active;
                array[newH, newX].Ocupied = false;

                // Up - Right
                newH = posH - 1;
                newX = posX + 1;
                array[newH, newX].Image.overrideSprite = IBox_active;
                array[newH, newX].Ocupied = false;
                break;

            case InventorySpace.VerticalLine2:
                break;
            case InventorySpace.HorizontalLine2:
                break;
            default:
                break;
        }
        return array;
    }
}