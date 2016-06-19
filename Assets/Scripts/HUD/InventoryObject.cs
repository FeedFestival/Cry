using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Types;

public class InventoryObject : MonoBehaviour
{
    public Item Item;
    public Image Image;

    public void Initialize(Item inventoryObject)
    {
        Item = inventoryObject;

        EventTrigger trigger = Image.transform.gameObject.GetComponent<EventTrigger>();

        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        clickEvent.eventID = EventTriggerType.PointerClick;
        clickEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> clickCall = new UnityEngine.Events.UnityAction<BaseEventData>(Click);
        clickEvent.callback.AddListener(clickCall);

        trigger.triggers.Add(clickEvent);
    }

    public void Click(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        // Grab Item.
        GlobalData.Player.UnitInventory.InventoryObjectInHand = this.Item;
        GlobalData.Player.UnitInventory.RemoveInventoryItems();
    }
}