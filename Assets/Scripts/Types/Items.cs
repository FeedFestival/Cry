using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Types
{
    public enum ItemName
    {
        Apple
    }
    public static class Items
    {
        public static Item CreateItem(ItemName ItemName)
        {
            Item item = new Item
            {
                ItemName = ItemName
            };

            switch (ItemName)
            {
                case ItemName.Apple:

                    item.InventorySpace = InventorySpace.Square2;

                    break;

                default:
                    break;
            }

            return item;
        }

        public static Item CreateInventoryObject2D(Item Item)
        {
            var goImage = GameObject.Instantiate(Resources.Load("Prefabs/UI/InventoryItem"), Vector3.zero, GlobalData.CameraControl.HUD.InventoryList.transform.rotation) as GameObject;

            Item.InventoryObject2D = goImage.transform.GetComponent<InventoryObject>();
            Item.InventoryObject2D.Image = goImage.GetComponent<Image>();

            // we modify the parent so we can click on the box instead of the item;
            Item.InventoryObject2D.Image.transform.parent = GlobalData.CameraControl.HUD.InventoryList.transform;

            // we assing the image to the apple;
            Item.InventoryObject2D.Image.overrideSprite = Resources.Load<Sprite>("InventoryItems/" + Item.ItemName.ToString());
            Item.InventoryObject2D.Image.name = Item.ItemName.ToString();

            Item.InventoryObject2D.Image.transform.localScale = new Vector3(1, 1, 1);
            Item.InventoryObject2D.Image.rectTransform.sizeDelta = new Vector2(42f, 42f);

            Item.InventoryObject2D.Initialize(Item);

            return Item;
        }

        //public static Item CreateInventoryObject3D(Item Item)
        //{
            //var goImage = GameObject.Instantiate(Resources.Load("Prefabs/UI/InventoryItem"), Vector3.zero, GlobalData.CameraControl.HUD.InventoryList.transform.rotation) as GameObject;

            //Item.InventoryObject2D = goImage.transform.GetComponent<InventoryObject>();
            //Item.InventoryObject2D.Image = goImage.GetComponent<Image>();

            //// we modify the parent so we can click on the box instead of the item;
            //Item.InventoryObject2D.Image.transform.parent = GlobalData.CameraControl.HUD.InventoryList.transform;

            //// we assing the image to the apple;
            //Item.InventoryObject2D.Image.overrideSprite = Resources.Load<Sprite>("InventoryItems/" + Item.ItemName.ToString());
            //Item.InventoryObject2D.Image.name = Item.ItemName.ToString();

            //Item.InventoryObject2D.Image.transform.localScale = new Vector3(1, 1, 1);
            //Item.InventoryObject2D.Image.rectTransform.sizeDelta = new Vector2(42f, 42f);

            //Item.InventoryObject2D.Initialize(Item);

            //return Item;
        //}
    }
}
