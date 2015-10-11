using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Ladder : MonoBehaviour {

    public bool debuging;

    [HideInInspector]
    public LadderActionHandler LadderActionHandler;
    [HideInInspector]
    public LadderAnimation LadderAnimation;
    [HideInInspector]
    public Animation LadderAnimator;

    // transforms
    [HideInInspector]
    public Transform Root;
    [HideInInspector]
    public Transform StartPoint_Bottom;
    [HideInInspector]
    public Transform StartPoint_Level2_Top;
    [HideInInspector]
    public Transform StartPoint_7m;

    [HideInInspector]
    public int lastAnimation;

    public LadderStartPoint LadderStartPoint;

    public LadderTriggerInput LadderTriggerInput;

    // Use this for initialization
	void Start () {
        // Scripts Initialization
        var thisScript = GetComponent<Ladder>();

        LadderActionHandler = GetComponent<LadderActionHandler>();
        LadderActionHandler.Initialize(thisScript, this.gameObject);

        // Transforms initialization
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Root")
            {
                Root = child;
            }
            else if (child.gameObject.name == "StartPoint_Bottom")
            {
                StartPoint_Bottom = child;
            }
            else if (child.gameObject.name == "StartPoint_Level2_Top")
            {
                StartPoint_Level2_Top = child;
            }
            else if (child.gameObject.name == "StartPoint_7m")
            {
                StartPoint_7m = child;
            }
            else if (child.gameObject.name == "LadderStaticAnimator")   // HARD_CODED
            {
                LadderAnimator = child.gameObject.GetComponent<Animation>();
            }
            else if (child.gameObject.name == "Bottom")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Bottom, this);
            }
            else if (child.gameObject.name == "Level1")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Level1, this);
            }
            else if (child.gameObject.name == "Level2_Top")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Level2_Top, this);
            }
        }

        // Scripts Initialization
        LadderAnimation = GetComponent<LadderAnimation>();
        LadderAnimation.Initialize(thisScript);
	}
}