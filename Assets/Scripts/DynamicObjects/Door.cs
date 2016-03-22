using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Door : MonoBehaviour
{
    [HideInInspector]
    public DoorInputTrigger DoorInputTrigger;
    [HideInInspector]
    public DoorActionHandler DoorActionHandler;
    [HideInInspector]
    public DoorAnimation DoorAnimation;
    [HideInInspector]
    public Animation DoorAnimator;

    // transforms
    [HideInInspector]
    public Transform front;
    [HideInInspector]
    public Transform back;

    [HideInInspector]
    public CircleAction CircleAction;

    public DoorState DoorState;

    public DoorStartPoint DoorStartPoint;

    public int DoorPassword = 0;

    public Vector3 StartPointPosition
    {
        get
        {
            if (DoorStartPoint == DoorStartPoint.front)
            {
                return front.position;
            }
            else
            {
                return back.position;
            }
        }
    }

    public Transform GetRoot()
    {
        if (DoorStartPoint == DoorStartPoint.front)
        {
            return front;
        }
        else
        {
            return back;
        }
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "front":

                    front = child.transform;
                    break;

                case "back":

                    back = child.transform;
                    break;

                case "Door_0":
                    DoorInputTrigger = child.transform.gameObject.GetComponent<DoorInputTrigger>();
                    DoorInputTrigger.Initialize(this);
                    break;

                case "UI_ActionCircle":
                    CircleAction = child.transform.gameObject.GetComponent<CircleAction>();
                    break;

                case "DoorAnimator":
                    DoorAnimator = child.gameObject.GetComponent<Animation>();
                    break;

                default:
                    break;
            }
        }
        DoorActionHandler = this.GetComponent<DoorActionHandler>();
        DoorActionHandler.Initialize(this);

        DoorAnimation = this.GetComponent<DoorAnimation>();
        DoorAnimation.Initialize(this);

        if (DoorState == DoorState.Locked && DoorPassword == 0)
        {
            Debug.LogError("Door ("+ this.gameObject.name +") is locked and has no password. no key can open it.");
        }
    }
}
