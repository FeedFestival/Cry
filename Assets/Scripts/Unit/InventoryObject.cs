using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class InventoryObject
{
    public InventoryObjectState InventoryObjectState;

    public string Name;

    public int originH;
    public int originX;

    public InventoryGroup InventoryGroup;

    public InventorySpace _inventorySpace;
    public InventorySpace InventorySpace
    {
        get
        {
            return _inventorySpace;
        }
        set
        {
            _inventorySpace = value;
            switch (_inventorySpace)
            {
                case InventorySpace.Square:

                    break;

                case InventorySpace.Square2:

                    break;

                case InventorySpace.VerticalLine2:
                    break;
                case InventorySpace.HorizontalLine2:
                    break;
                default:
                    break;
            }
        }
    }

    public Texture2D Image;

    public GOInventoryItem InventoryObject2D;
    public InteractiveObject InteractiveObject;
}
