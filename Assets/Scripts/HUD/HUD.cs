using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour
{
    private CameraControl CameraControl;

    public ButtonName buttonType;

    // buttons
    private CryButton ESC_COG_button;

    public CryButton I_INVENTORY_button;

    private Button GameMenu_ResumeGame;

    // Image / Panels
    private Image GameMenu;

    public Image Inventory;
    public Image InventoryDropArea;

    // buttons images
    private Sprite ESC_COG_image;
    private Sprite ESC_COG_image_active;

    private Sprite I_INVENTORY_image;
    private Sprite I_INVENTORY_image_active;

    public GameObject PendingInventory;
    public GameObject InventoryList;

    public Transform MovementPoints;

    public void Initialize(CameraControl cameraControl)
    {
        CameraControl = cameraControl;

        ESC_COG_image = Resources.Load<Sprite>("HUD/Cog");
        ESC_COG_image_active = Resources.Load<Sprite>("HUD/Cog_Active");

        I_INVENTORY_image = Resources.Load<Sprite>("HUD/Backpack_C_inactive");
        I_INVENTORY_image_active = Resources.Load<Sprite>("HUD/Backpack_C_active");

        Transform[] allChildren = GetComponentsInChildren<Transform>(true); // we want the transforms that are inactive too with the 'true' parameter.
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "ESC_COG_button":

                    ESC_COG_button = new CryButton();
                    ESC_COG_button.Button = child.transform.GetComponent<Button>();
                    break;

                case "I_INVENTORY_button":

                    I_INVENTORY_button = new CryButton();
                    I_INVENTORY_button.Button = child.transform.GetComponent<Button>();
                    break;

                case "GameMenu":

                    GameMenu = child.transform.GetComponent<Image>();
                    break;

                case "InventoryItems":

                    Inventory = child.transform.GetComponent<Image>();
                    break;

                case "GameMenu_ResumeGame":

                    GameMenu_ResumeGame = child.transform.GetComponent<Button>();
                    break;

                case "PendingInventory":

                    PendingInventory = child.transform.gameObject;
                    break;

                case "InventoryList":

                    InventoryList = child.transform.gameObject;
                    break;

                case "InventoryDropArea":

                    InventoryDropArea = child.transform.gameObject.GetComponent<Image>();
                    break;

                case "MovementPoints":

                    MovementPoints = child;
                    break;

                default:
                    break;
            }
        }
        foreach (Transform child in MovementPoints)
        {
            child.gameObject.GetComponent<ScreenEdge>().Initialize(this.CameraControl);
        }

        GetInventoryBoxes();
    }

    public void GetInventoryBoxes()
    {
        string name = "";
        int hl = 0, xl = 0;

        InventoryGroup _inventoryGroup;

        foreach (Transform trans in Inventory.transform)
        {
            if (trans.name == InventoryGroup.LeftPocket.ToString())
            {
                _inventoryGroup = InventoryGroup.LeftPocket;
                hl = 4;
                xl = 2;
            }
            else if (trans.name == InventoryGroup.RightPocket.ToString())
            {
                _inventoryGroup = InventoryGroup.RightPocket;
                hl = 4;
                xl = 2;
            }
            else
            {
                if (GlobalData.Player.hasBackPack)
                {
                    _inventoryGroup = InventoryGroup.Backpack;
                    hl = 8;
                    xl = 5;
                }
                else
                {
                    trans.gameObject.SetActive(false);
                    continue;
                }
            }

            InventoryBox[,] array = new InventoryBox[hl, xl];

            Transform[] allChildren = trans.GetComponentsInChildren<Transform>(true); // we want the transforms that are inactive too with the 'true' parameter.

            for (var h = 0; h <= 3; h++)
            {
                for (var x = 0; x <= 1; x++)
                {
                    name = "InventoryBox[" + h + "," + x + "]";
                    var exist = false;
                    var c = 0;

                    for (c = 0; c < allChildren.Length; c++)
                    {
                        if (allChildren[c] != null && allChildren[c].gameObject.name == name)
                        {
                            array[h, x] = allChildren[c].gameObject.GetComponent<InventoryBox>();
                            array[h, x].Ocupied = false;
                            array[h, x].Initialize(allChildren[c].gameObject.GetComponent<Image>(), _inventoryGroup, h, x);

                            exist = true;
                            break;
                        }
                    }
                    if (exist)
                    {
                        allChildren[c] = null;
                    }
                }
            }

            if (_inventoryGroup == InventoryGroup.LeftPocket)
            {
                array[0, 1].Ocupied = true;
                GlobalData.Player.UnitInventory.LeftPocket = array;
            }
            else if (_inventoryGroup == InventoryGroup.RightPocket)
            {
                array[0, 0].Ocupied = true;
                GlobalData.Player.UnitInventory.RightPocket = array;
            }
            else
            {
                GlobalData.Player.UnitInventory.Backpack = array;
            }
        }
    }

    public ButtonName CurrentButton;

    public void MouseHover(int buttonTypeId)
    {
        //Debug.Log(buttonTypeId);
        CurrentButton = (ButtonName)buttonTypeId;
        switch (CurrentButton)
        {
            case ButtonName.ESC_COG:

                if (ESC_COG_button.pressed == false)
                {
                    ESC_COG_button.Button.image.overrideSprite = ESC_COG_image_active;
                }
                break;

            case ButtonName.I_INVENTORY:

                if (I_INVENTORY_button.pressed == false)
                {
                    I_INVENTORY_button.Button.image.overrideSprite = I_INVENTORY_image_active;
                }
                break;

            default:
                break;
        }
    }

    public void MouseExit(int buttonTypeId)
    {
        CurrentButton = (ButtonName)buttonTypeId;
        switch (CurrentButton)
        {
            case ButtonName.ESC_COG:

                if (ESC_COG_button.pressed == false)
                {
                    ESC_COG_button.Button.image.overrideSprite = ESC_COG_image;
                }
                break;

            case ButtonName.I_INVENTORY:

                if (I_INVENTORY_button.pressed == false)
                {
                    I_INVENTORY_button.Button.image.overrideSprite = I_INVENTORY_image;
                }
                break;

            default:
                break;
        }
        CurrentButton = ButtonName.None;
    }

    public void MouseClick(int mouseNameId = 0)
    {
        if (mouseNameId != 0)
        {
            CurrentButton = (ButtonName)mouseNameId;
        }
        HUDAction();
    }

    public void HUDAction(ButtonName keyboardPress = ButtonName.None)
    {
        if (keyboardPress != ButtonName.None)
        {
            CurrentButton = keyboardPress;
        }
        //Debug.Log(CurrentButton);
        if (CurrentButton != ButtonName.None)
        {
            switch (CurrentButton)
            {
                case ButtonName.ESC_COG:

                    if (ESC_COG_button.pressed == false)
                    {
                        ESC_COG_button.Button.image.overrideSprite = ESC_COG_image_active;
                        ESC_COG_button.pressed = true;

                        // Show Options
                        GameMenu.transform.gameObject.SetActive(true);

                        MovementPoints.gameObject.SetActive(false);
                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        ESC_COG_button.Button.image.overrideSprite = ESC_COG_image;
                        ESC_COG_button.pressed = false;

                        // Hide Options
                        GameMenu.transform.gameObject.SetActive(false);

                        MovementPoints.gameObject.SetActive(true);
                        Time.timeScale = 1.0f;
                    }
                    break;

                case ButtonName.W_HANDS:
                    break;

                case ButtonName.I_INVENTORY:
                    if (I_INVENTORY_button.pressed == false)
                    {
                        I_INVENTORY_button.Button.image.overrideSprite = I_INVENTORY_image_active;
                        I_INVENTORY_button.pressed = true;

                        // Show InventoryItems
                        Inventory.transform.gameObject.SetActive(true);
                        InventoryList.gameObject.SetActive(true);
                        PendingInventory.gameObject.SetActive(true);
                        GlobalData.Player.UnitInventory.PlaceInventoryItems();

                        MovementPoints.gameObject.SetActive(false);
                    }
                    else
                    {
                        I_INVENTORY_button.Button.image.overrideSprite = I_INVENTORY_image;
                        I_INVENTORY_button.pressed = false;

                        // Hide InventoryItems
                        Inventory.transform.gameObject.SetActive(false);
                        InventoryDropArea.gameObject.SetActive(false);

                        MovementPoints.gameObject.SetActive(true);
                    }
                    break;

                default:
                    break;
            }
            CurrentButton = ButtonName.None;
        }
    }
}

public class CryButton
{
    public bool pressed;
    public Button Button;
}