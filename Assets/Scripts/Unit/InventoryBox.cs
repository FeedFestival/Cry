using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;
using UnityEngine.EventSystems;

public class InventoryBox : MonoBehaviour
{
    UnitInventory UnitInventory;

    public Image Image;

    public InventoryGroup InventoryGroup;

    public int H;
    public int X;

    public bool Ocupied;

    //private Color32 IBox_active;
    //private Color32 IBox_inactive;

    public void Initialize(Image image, InventoryGroup inventoryGroup, int h, int x)
    {
        UnitInventory = GlobalData.Player.UnitInventory;

        Image = image;

        InventoryGroup = inventoryGroup;

        H = h;
        X = x;

        EventTrigger trigger = Image.GetComponent<EventTrigger>();

        EventTrigger.Entry hoverEvent = new EventTrigger.Entry();
        hoverEvent.eventID = EventTriggerType.PointerEnter;
        hoverEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> hoverCall = new UnityEngine.Events.UnityAction<BaseEventData>(Hover);
        hoverEvent.callback.AddListener(hoverCall);

        trigger.delegates.Add(hoverEvent);

        EventTrigger.Entry exithoverEvent = new EventTrigger.Entry();
        exithoverEvent.eventID = EventTriggerType.PointerExit;
        exithoverEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> exithoverCall = new UnityEngine.Events.UnityAction<BaseEventData>(ExitHover);
        exithoverEvent.callback.AddListener(exithoverCall);

        trigger.delegates.Add(exithoverEvent);

        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        clickEvent.eventID = EventTriggerType.PointerClick;
        clickEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> clickCall = new UnityEngine.Events.UnityAction<BaseEventData>(Click);
        clickEvent.callback.AddListener(clickCall);

        trigger.delegates.Add(clickEvent);
    }

    public void Hover(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (UnitInventory.InventoryObjectInHand != null && this.Ocupied == false)
            UnitInventory.CalculateSpace(H, X, InventoryGroup);
    }

    public void ExitHover(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (UnitInventory.InventoryObjectInHand != null && this.Ocupied == false)
            UnitInventory.CalculateSpaceExit(H, X, InventoryGroup);
    }

    public void Click(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (UnitInventory.InventoryObjectInHand != null && this.Ocupied == false)
            UnitInventory.PlaceInSpace(H, X, InventoryGroup);
    }
}
