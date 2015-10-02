using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Ledge : MonoBehaviour
{
    public SceneManager SceneManager;

    private LedgeInputTrigger LedgeInputTrigger;
    [HideInInspector]
    public LedgeActionHandler LedgeActionHandler;

    public LedgeType LedgeType;
    [HideInInspector]
    public LedgeStartPoint LedgeStartPoint;

    [HideInInspector]
    public Transform thisTransform;

    [HideInInspector]
    public Vector3 StartPointPosition;

    [HideInInspector]
    public Transform Root;

    [HideInInspector]
    public string thisWallClimb_Name;

    [HideInInspector]
    public Animation Ledge_Animator;

    // Use this for initialization
    void Start()
    {
        //SceneManager = sceneManager;
        thisTransform = this.transform;

        var liftUpPosition = new Vector3(0,0.01f,0);
        var forwardLiftUp = new Vector3(0, 0, 0.01f);

        thisTransform.position = thisTransform.position + liftUpPosition;
        thisTransform.position = thisTransform.position + forwardLiftUp;

        thisWallClimb_Name = "[" + thisTransform.position.x + "," + thisTransform.position.y + "," + thisTransform.position.z + "]";

        LedgeInputTrigger = thisTransform.GetComponent<LedgeInputTrigger>();
        LedgeInputTrigger.Initialize(this);

        LedgeActionHandler = thisTransform.GetComponent<LedgeActionHandler>();
        LedgeActionHandler.Initialize(this);
    }

    public void ResetLedgeAction()
    {
        Destroy(LedgeInputTrigger.Ledge_GameObject);
    }
}
