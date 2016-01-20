using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class InteractiveObject : MonoBehaviour
{
    public InventoryObject Item;
    public GameObject _3DObject;

    public ObjetState ObjetState;

    private Material Material;

    private Vector3 objectPosition;
    private Vector3 objectNavMeshPosition;

    public Vector3 StartPointPosition;

    void Start()
    {
        Material = _3DObject.GetComponent<Material>();

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
            ObjetState = ObjetState.OnGround;
        }
        else if (dif > 1f && dif < 2f)
        {
            ObjetState = ObjetState.OnShelf;
        }
        else if (dif < 1f)
        {
            ObjetState = ObjetState.OnTable;
        }
    }

    private void Pickup()
    {
        var playerPosition = GlobalData.Player.UnitProperties.thisTransform.position;
        var length = Vector3.Distance(playerPosition, objectPosition);

        if (length > 7)
            return;

        switch (ObjetState)
        {
            case ObjetState.InInventory:
                break;
            case ObjetState.OnGround:

                var path = GlobalData.Player.UnitController.GetNavMeshPathCorners(playerPosition, objectNavMeshPosition);

                if (path != null && path.Length != 0)
                {
                    StartPointPosition = Logic.IncreaseOrDecreaseLine(objectNavMeshPosition, path[path.Length - 2], length, 0.7f);
                    GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                }
                break;

            case ObjetState.OnTable:

                StartPointPosition = Logic.IncreaseOrDecreaseLine(objectNavMeshPosition, playerPosition, length, 0.7f);   // this might be totally wrong.
                GlobalData.Player.UnitActionHandler.SetAction(this.gameObject, ActionType.PickupObject);
                break;

            case ObjetState.OnShelf:

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
        _3DObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.white);
        _3DObject.GetComponent<Renderer>().material.SetFloat("_Outline", 20f);
    }

    void OnMouseExit()
    {
        _3DObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.black);
        _3DObject.GetComponent<Renderer>().material.SetFloat("_Outline", 4f);
    }
}

public enum ObjetState
{
    InInventory, OnGround, OnTable, OnShelf
}