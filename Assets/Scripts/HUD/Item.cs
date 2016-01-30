using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class Item
{
    public ObjectState ObjectState;
    public ItemName ItemName;
    public Texture2D Image;

    public bool isInInventory;

    // 2d
    public InventoryObject InventoryObject;

    public int pendingH;
    public int pendingX;

    public int originH;
    public int originX;

    public InventoryGroup InventoryGroup;

    public InventoryGroup pendingInventoryGroup;

    // This is how much this Item ocupies on the board: If its 0 then it ocupies only one box;
    public int spaceH;
    public int spaceX;

    // 3d
    public InteractiveObject InteractiveObject;
    public GameObject model;
    public Material Material;

    public Vector3 objectPosition;
    public Vector3 objectNavMeshPosition;

    public Vector3 StartPointPosition;

    public void PlaceInInventory()
    {
        pendingH = originH;
        pendingX = originX;
        pendingInventoryGroup = InventoryGroup;
        InventoryObject.Image.transform.SetParent(GlobalData.CameraControl.HUD.InventoryList.transform);
    }
    public void PlaceIn3DWorld()
    {
        this.ObjectState = ObjectState.OnGround;
        this.InteractiveObject.setSettings();
        GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
        GlobalData.CameraControl.CameraCursor.showDropItemLocation = false;
        this.isInInventory = false;
    }

    public void ShowDropItemLocation()
    {
        GlobalData.Player.UnitInventory.InventoryObjectInHand = Items.CreateInventoryObject3D(GlobalData.Player.UnitInventory.InventoryObjectInHand);
        GlobalData.Player.UnitInventory.InventoryObjectInHand.InventoryObject.Image.enabled = false;
        GlobalData.CameraControl.CameraCursor.drawInventoryItem = false;
        GlobalData.CameraControl.CameraCursor.firstTimeMoving3DObject = true;
        GlobalData.CameraControl.CameraCursor.showDropItemLocation = true;
    }
    public void DontShowDropItemLocation()
    {
        GlobalData.Player.UnitInventory.InventoryObjectInHand.InventoryObject.Image.enabled = true;
        GlobalData.CameraControl.CameraCursor.drawInventoryItem = true;
        GlobalData.CameraControl.CameraCursor.showDropItemLocation = false;
    }
}
