using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Chair : MonoBehaviour
{
    public SceneManager SceneManager;

    [HideInInspector]
    public ChairActionHandler ChairActionHandler;
    [HideInInspector]
    public ChairAnimation ChairAnimation;

    [HideInInspector]
    public SingleActionMouseInputTrigger SingleActionMouseInputTrigger;

    [HideInInspector]
    public GameObject ChairGameObject;

    [HideInInspector]
    public Material ChairMaterial;

    //  transforms
    [HideInInspector]
    public Transform StartPoint_Left;
    [HideInInspector]
    public Transform StartPoint_Right;
    [HideInInspector]
    public Transform StartPoint_Front;
    [HideInInspector]
    public Transform StartPoint_Back;

    public Transform Root;

    //[HideInInspector]
    public Animation ChairStaticAnimator;

    public ChairStartPoint ChairStartPoint;

    // Use this for initialization
    void Start()
    {
        ChairActionHandler = GetComponent<ChairActionHandler>();
        ChairActionHandler.Initialize(this);

        ChairAnimation = GetComponent<ChairAnimation>();
        ChairAnimation.Initialize(this);

        SingleActionMouseInputTrigger = GetComponent<SingleActionMouseInputTrigger>();
        SingleActionMouseInputTrigger.Initialize(ChairActionHandler);

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "GO_Chair":
                    ChairGameObject = child.gameObject; // FOR_NOW
                    var renderer = ChairGameObject.GetComponent<Renderer>();
                    ChairMaterial = renderer.material;
                    break;
                case "Left":
                    StartPoint_Left = child;
                    break;
                case "Right":
                    StartPoint_Right = child;
                    break;
                case "Front":
                    StartPoint_Front = child;
                    break;
                case "Chair_StaticAnimator":
                    var gameObject = child.gameObject;
                    ChairStaticAnimator = gameObject.GetComponent<Animation>();
                    break;
                default:
                    break;
            }
        }

        InitializeAnimations();
    }

    #region Chair Animations    [Chair animation strings ; setup]

    public string GetOn_FromFront;
    public string GetOn_FromLeft;
    public string GetOn_FromRight;
    public string GetOff_ToFront;
    public string GetOff_ToLeft;
    public string GetOff_ToRight;

    private void InitializeAnimations()
    {
        GetOn_FromFront = "Chair_ActionAnimator|Chair_GetOff_ToFront_O";
        GetOn_FromLeft = "Chair_ActionAnimator|Chair_GetOn_FromLeft_O";
        GetOn_FromRight = "Chair_ActionAnimator|Chair_GetOn_FromRight_O";
        GetOff_ToFront = "Chair_ActionAnimator|Chair_GetOff_ToFront_O";
        GetOff_ToLeft = "Chair_ActionAnimator|Chair_GetOff_ToLeft_O";
        GetOff_ToRight = "Chair_ActionAnimator|Chair_GetOff_ToRight_O";
    }
    #endregion
}
