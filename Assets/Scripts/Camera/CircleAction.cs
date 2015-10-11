using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CircleAction : MonoBehaviour
{
    [HideInInspector]
    public Transform thisTransform;

    private MeshRenderer MeshRenderer;

    [HideInInspector]
    public CircleActionState CircleActionState;

    private Material availableAction;

    // Use this for initialization
    void Start()
    {
        this.thisTransform = this.transform;
        MeshRenderer = this.thisTransform.GetComponent<MeshRenderer>();

        availableAction = Resources.Load("Materials/UI/UI_CircleAction") as Material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thisTransform.LookAt(GlobalData.CameraControl.thisTransform);
    }

    public void ChangeState(CircleActionState circleActionState)
    {
        CircleActionState = circleActionState;

        switch (CircleActionState)
        {
            case CircleActionState.None:

                MeshRenderer.enabled = false;
                break;

            case CircleActionState.Unavailable:
                
                break;

            case CircleActionState.Available:

                if (MeshRenderer.enabled == false)
                    MeshRenderer.enabled = true;
                MeshRenderer.materials[0] = availableAction;
                break;

            case CircleActionState.Activate:
                break;
            default:
                break;
        }
    }
}