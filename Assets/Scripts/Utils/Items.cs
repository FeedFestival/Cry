using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public enum ItemType
    {
        Miscellaneous, Key, QuestItem
    }

    public enum InventoryGroup
    {
        None, LeftPocket, RightPocket, Backpack, JacketLeftPocket, JacketRightPocket
    }
    public enum ObjectState
    {
        InInventory, OnGround, OnTable, OnShelf
    }

    public static class Items
    {
        public static Item CreateItem(ItemName ItemName, ItemType itemType, int password)
        {
            Item item = new Item
            {
                ItemName = ItemName,
                ItemType = itemType,
                Password = password
            };

            switch (ItemName)
            {
                case ItemName.Apple:

                    item.spaceH = 1;
                    item.spaceX = 1;

                    break;

                case ItemName.Watch:

                    item.spaceH = 1;
                    item.spaceX = 0;
                    break;

                case ItemName.Key:

                    item.spaceH = 1;
                    item.spaceX = 0;
                    break;

                default:
                    break;
            }

            return item;
        }

        public static Item CreateInventoryObject2D(Item Item)
        {
            var goImage = GameObject.Instantiate(Resources.Load("Prefabs/UI/InventoryItem"), Vector3.zero, GlobalData.CameraControl.HUD.InventoryList.transform.rotation) as GameObject;

            Item.InventoryObject = goImage.transform.GetComponent<InventoryObject>();
            Item.InventoryObject.Image = goImage.GetComponent<Image>();

            // we modify the parent so we can click on the box instead of the item;
            Item.InventoryObject.Image.transform.SetParent(GlobalData.CameraControl.HUD.InventoryList.transform);

            // we assing the image to the apple;
            Item.InventoryObject.Image.overrideSprite = Resources.Load<Sprite>("InventoryItems/" + Item.ItemName.ToString());
            Item.InventoryObject.Image.name = Item.ItemName.ToString();

            Item.InventoryObject.Image.transform.localScale = new Vector3(1, 1, 1);
            Item.InventoryObject.Image.rectTransform.sizeDelta = new Vector2(42f, 42f);

            Item.InventoryObject.Initialize(Item);

            return Item;
        }

        public static Item CreateInventoryObject3D(Item Item)
        {
            var goObject = GameObject.Instantiate(Resources.Load("Prefabs/" + Item.ItemName.ToString()), Vector3.zero, Quaternion.identity) as GameObject;

            Item.InteractiveObject = goObject.GetComponent<InteractiveObject>();
            Item.InteractiveObject.Initialize(Item);

            return Item;
        }
    }

    public enum ItemName
    {
        Apple, Watch, Key
    }
}