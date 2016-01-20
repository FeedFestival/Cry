using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class Item
{
    public ObjectState ObjectState;

    public ItemName ItemName;

    public int originH;
    public int originX;

    public InventoryGroup InventoryGroup;

    public InventorySpace InventorySpace;

    public int spaceH;
    public int spaceX;

    public Texture2D Image;

    public GOInventoryItem InventoryObject2D;
    public InteractiveObject InteractiveObject;
}
