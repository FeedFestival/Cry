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
}
