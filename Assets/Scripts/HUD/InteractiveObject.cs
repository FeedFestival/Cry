using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class InteractiveObject : MonoBehaviour
{
    public Item Item;
    public ItemName ItemName;

    public GameObject _3DObject;

    private Material Material;

    // transforms

    private Vector3 objectPosition;
    private Vector3 objectNavMeshPosition;

    public Vector3 StartPointPosition;

    void Start()
    {
        this.Item = Items.CreateItem(this.ItemName);

        Material = _3DObject.GetComponent<Renderer>().material;

        objectPosition = this.transform.position;

        int mask = (1 << LayerMask.NameToLayer("Map"));

        RaycastHit hit;
        if (Physics.Linecast(objectPosition, new Vector3(objectPosition.x, 0, objectPosition.z), out hit, mask))
        {
            objectNavMeshPosition = hit.point;
        }

        // check to see if object is standing on something. Probably a table.
        var dif = objectPosition.y - objectNavMeshPosition.y;
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

        Item.InteractiveObject = this;
    }

    private void Pickup()
    {
        var playerPosition = GlobalData.Player.UnitProperties.thisTransform.position;
        var length = Vector3.Distance(playerPosition, objectPosition);

        if (length > 7)
            return;

        switch (Item.ObjectState)
        {
            case ObjectState.InInventory:
                break;
            case ObjectState.OnGround:

                var path = GlobalData.Player.UnitController.GetNavMeshPathCorners(playerPosition, objectNavMeshPosition);

                if (path != null && path.Length != 0)
                {
                    StartPointPosition = Logic.IncreaseOrDecreaseLine(objectNavMeshPosition, path[path.Length - 2], length, 0.7f);
                    GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                }
                break;

            case ObjectState.OnTable:

                StartPointPosition = Logic.IncreaseOrDecreaseLine(objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong.
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            case ObjectState.OnShelf:

                StartPointPosition = Logic.IncreaseOrDecreaseLine(objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong.
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            default:
                break;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            Pickup();
        }
    }

    void OnMouseEnter()
    {
        Material.SetColor("_OutlineColor", Color.white);
        Material.SetFloat("_Outline", 20f);
    }

    void OnMouseExit()
    {
        Material.SetColor("_OutlineColor", Color.black);
        Material.SetFloat("_Outline", 4f);
    }
}