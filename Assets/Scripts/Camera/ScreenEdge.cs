using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine.EventSystems;

public class ScreenEdge : MonoBehaviour
{
    public CameraEdge CameraEdge;
    public CameraEdgeSpeed CameraEdgeSpeed;

    private CameraControl CameraControl;

    public void Initialize(CameraControl cameraControl)
    {
        CameraControl = cameraControl;

        EventTrigger trigger = this.GetComponent<EventTrigger>();

        EventTrigger.Entry hoverEvent = new EventTrigger.Entry();
        hoverEvent.eventID = EventTriggerType.PointerEnter;
        hoverEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> hoverCall = new UnityEngine.Events.UnityAction<BaseEventData>(Hover);
        hoverEvent.callback.AddListener(hoverCall);

        trigger.triggers.Add(hoverEvent);

        EventTrigger.Entry exithoverEvent = new EventTrigger.Entry();
        exithoverEvent.eventID = EventTriggerType.PointerExit;
        exithoverEvent.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> exithoverCall = new UnityEngine.Events.UnityAction<BaseEventData>(ExitHover);
        exithoverEvent.callback.AddListener(exithoverCall);

        trigger.triggers.Add(exithoverEvent);
    }

    public void Hover(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        CameraControl.CameraPan(CameraEdge, CameraEdgeSpeed);
    }

    public void ExitHover(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        CameraControl.PanCamera = false;
    }
}