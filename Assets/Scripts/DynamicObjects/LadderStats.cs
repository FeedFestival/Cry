using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class LadderStats : MonoBehaviour {

    public bool debuging;

    public SceneManager SceneManager;

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

    // Use this for initialization
	void Start () {
        // Scripts Initialization
        var thisScript = GetComponent<LadderStats>();

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
            else if (child.gameObject.name == "Ladder")   // HARD_CODED
            {
                LadderAnimator = child.gameObject.GetComponent<Animation>();
            }
            else if (child.gameObject.name == "Bottom")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Bottom, LadderActionHandler);
            }
            else if (child.gameObject.name == "Level1")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Level1, LadderActionHandler);
            }
            else if (child.gameObject.name == "Level2_Top")
            {
                child.gameObject.AddComponent<LadderInputTrigger>();
                child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Level2_Top, LadderActionHandler);
            }
            //else if (child.gameObject.name == "Level2")
            //{
            //    child.gameObject.AddComponent<LadderInputTrigger>();
            //    child.gameObject.GetComponent<LadderInputTrigger>().Initialize(LadderTriggerInput.Level2, LadderActionHandler);
            //}
        }

        // Scripts Initialization
        LadderAnimation = GetComponent<LadderAnimation>();
        LadderAnimation.Initialize(thisScript);

        InitializeAnimations();
	}

    #region Animation

    [HideInInspector]
    public string GetOn_From_Bottom;
    [HideInInspector]
    public string Climb_From_Bottom_To_Level1;
    [HideInInspector]
    public string Climb_Exit_To_Level2_Top;

    [HideInInspector]
    public string GetOn_From_Level2_Top;

    [HideInInspector]
    public string ClimbDown_From_Level1_To_Bottom;

    [HideInInspector]
    public string ClimbDown_Exit_To_Bottom;

    //---------------------------------------------------------------------------

    [HideInInspector]
    public string Climb_From_Level1_To_Level2;

    //public string Ladder_Get_Down_Fast_O;
    //[HideInInspector]
    //public string Ladder_Get_Down_Origin;
    //[HideInInspector]
    //public string Ladder_Zone_0_to_1;
    //[HideInInspector]
    //public string Ladder_Zone_1_to_2;
    //[HideInInspector]
    //public string Ladder_Zone_2_to_3;
    //[HideInInspector]
    //public string Ladder_Zone_3_to_2;
    //[HideInInspector]
    //public string Ladder_Zone_2_to_1;
    //[HideInInspector]
    //public string Ladder_Zone_1_to_0;
    //[HideInInspector]
    //public string Ladder_Get_Up_Origin;
    

    void InitializeAnimations() {

        GetOn_From_Bottom = "Ladder_Get_On_Origin";
        LadderAnimator[GetOn_From_Bottom].wrapMode = WrapMode.Once;

        Climb_From_Bottom_To_Level1 = "Ladder_Zone_0_to_1";
        LadderAnimator[Climb_From_Bottom_To_Level1].wrapMode = WrapMode.Once;

        Climb_Exit_To_Level2_Top = "Ladder_Get_Up_Origin";
        LadderAnimator[Climb_Exit_To_Level2_Top].wrapMode = WrapMode.Once;

        //------------------------------

        GetOn_From_Level2_Top = "Ladder_Get_On_From_Up_O";
        LadderAnimator[GetOn_From_Level2_Top].wrapMode = WrapMode.Once;

        ClimbDown_From_Level1_To_Bottom = "Ladder_Zone_1_to_0";
        LadderAnimator[ClimbDown_From_Level1_To_Bottom].wrapMode = WrapMode.Once;

        ClimbDown_Exit_To_Bottom = "Ladder_Get_Down_Origin";
        LadderAnimator[ClimbDown_Exit_To_Bottom].wrapMode = WrapMode.Once;


        //----------------------------------------------------------------------------------------

        //Climb_Exit_To_Level2_Top = "Ladder_Zone_2_to_3";
        //LadderAnimations[Climb_Exit_To_Level2_Top].wrapMode = WrapMode.Once;

        //Ladder_Get_On_From_Up_O = "Ladder_Get_On_From_Up_O";
        //LadderAnimations[Ladder_Get_On_From_Up_O].wrapMode = WrapMode.Once;

        //Ladder_Climb = "Ladder_Climb";
        //LadderAnimations[Ladder_Climb].wrapMode = WrapMode.Once;
        //Ladder_Climb_Down = "Ladder_Climb_Down";
        //LadderAnimations[Ladder_Climb_Down].wrapMode = WrapMode.Once;

        //Ladder_Get_Down_Fast_O = "Ladder_Get_Down_Fast_O";
        //LadderAnimations[Ladder_Get_Down_Fast_O].wrapMode = WrapMode.Once;
        //Ladder_Get_Down_Origin = "Ladder_Get_Down_Origin";
        //LadderAnimations[Ladder_Get_Down_Origin].wrapMode = WrapMode.Once;

        //Ladder_Zone_3_to_2 = "Ladder_Zone_3_to_2";
        //LadderAnimations[Ladder_Zone_3_to_2].wrapMode = WrapMode.Once;
        //Ladder_Zone_2_to_1 = "Ladder_Zone_2_to_1";
        //LadderAnimations[Ladder_Zone_2_to_1].wrapMode = WrapMode.Once;
        //Ladder_Zone_1_to_0 = "Ladder_Zone_1_to_0";
        //LadderAnimations[Ladder_Zone_1_to_0].wrapMode = WrapMode.Once;
    }
    #endregion
}