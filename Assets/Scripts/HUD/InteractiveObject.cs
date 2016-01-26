using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class InteractiveObject : MonoBehaviour
{
    public Item Item;
    public ItemName ItemName;

    void Start()
    {
        this.Item = Items.CreateItem(this.ItemName);

        Item.model = this.transform.GetChild(0).gameObject;
        Item.Material = Item.model.GetComponent<Renderer>().material;
        Item.objectPosition = this.transform.position;
        Item.InteractiveObject = this;

        /* Settings */
        //--------------------

        int mask = (1 << LayerMask.NameToLayer("Map"));
        RaycastHit hit;
        if (Physics.Linecast(Item.objectPosition, new Vector3(Item.objectPosition.x, 0, Item.objectPosition.z), out hit, mask))
        {
            Item.objectNavMeshPosition = hit.point;
        }

        // check to see if object is standing on something.
        var dif = Item.objectPosition.y - Item.objectNavMeshPosition.y;
        if (dif < 0.3f)
        {
            Item.ObjectState = ObjectState.OnGround;
        }
        else if (dif > 1f && dif < 2f)
        {
            Item.ObjectState = ObjectState.OnShelf;
        }
        else if (dif < 1f)
        {
            Item.ObjectState = ObjectState.OnTable;
        }
    }

    private void Pickup()
    {
        var playerPosition = GlobalData.Player.UnitProperties.thisTransform.position;
        var length = Vector3.Distance(playerPosition, Item.objectPosition);

        if (length > 7)
            return;

        switch (Item.ObjectState)
        {
            case ObjectState.InInventory:
                break;
            case ObjectState.OnGround:

                var path = GlobalData.Player.UnitController.GetNavMeshPathCorners(playerPosition, Item.objectNavMeshPosition);

                if (path != null && path.Length != 0)
                {
                    Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, path[path.Length - 2], length, 0.7f);
                    GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                }
                break;

            case ObjectState.OnTable:

                Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong. // TO_DO
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            case ObjectState.OnShelf:

                Item.StartPointPosition = Logic.IncreaseOrDecreaseLine(Item.objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong. // TO_DO
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            default:
                break;
        }
    }

    void OnMouseOver()
    {
        if (GlobalData.Player.UnitActionState != UnitActionState.MovingItemInInventory)
        {
            if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
            {
                Pickup();
            }
        }
    }

    void OnMouseEnter()
    {
        Item.Material.SetColor("_OutlineColor", Color.white);
        Item.Material.SetFloat("_Outline", 20f);
    }

    void OnMouseExit()
    {
        Item.Material.SetColor("_OutlineColor", Color.black);
        Item.Material.SetFloat("_Outline", 4f);
    }
}