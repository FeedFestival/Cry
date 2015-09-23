using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitAnimation : MonoBehaviour
{

    bool isInStealth;

    public bool debuging = false;

    UnitStats UnitStats;

    public string curentAnimation;
    public int lastAnimation = 1000;
    bool ControllerFollowRoot;

    #region Standing Variables

    bool dontGoIntoIdle;
    bool isIdle;
    bool isWalking;

    string Walk;
    string Idle;

    #endregion

    #region Atack Variables and Functions

    /*  Atack in Front first atack */
    bool isAtackingInFront;
    bool canAtackInFront1;
    bool dontAtackInFront1;

    /*  Atack in Front second atack */
    bool isAtackingInFront2;
    bool canAtackInFront2;
    bool dontAtackInFront2;

    /*  Atack in front after charge.*/
    bool isAtackingInFront3;
    bool dontAtackInFront3;

    string Atack_Sword_1;
    string Atack_Sword_2;
    string Atack_Sword_Charge;
    string Atack_Sword_3_C;

    // For the atack charge
    float chargeTime = 0;
    float chargeRate = 1;
    float atackTime = 2;

    bool isCharging;
    bool isAtackCharged;

    void refreshAtack()
    {
        isAtackingInFront = false;
        canAtackInFront1 = true;
        dontAtackInFront1 = false;

        /*  Atack in Front second atack */
        isAtackingInFront2 = false;
        canAtackInFront2 = false;
        dontAtackInFront2 = false;

        // Charged
        isAtackCharged = false;
        isAtackingInFront3 = false;
        dontAtackInFront3 = false;
    }
    void refreshCharge()
    {
        isAtackCharged = false;
        chargeTime = 0;
        chargeRate = 1;
        atackTime = 2;
    }

    void isAtacking(bool value)
    {
        isIdle = !value;
        dontGoIntoIdle = value;

        isWalking = !value;
    }
    #endregion

    // UnitStats.UnitActionHandler
    bool GoIntoIdleAfterAnimation = true;

    #region Ladder Animations Variables

    public int ActionType;

    string Ladder_Idle;
    string Ladder_Climb;
    string Ladder_Climb_Down;
    string Ladder_Get_On;
    string Ladder_Get_On_From_Up;
    string Ladder_Get_Up;
    string Ladder_Get_Down;
    string Ladder_Get_Down_Fast;

    #endregion

    public void Initialize(UnitStats unitStats)
    {
        UnitStats = unitStats;

        isInStealth = true;

        if (isInStealth)
        {
            Idle = "Stealth_Idle";
            UnitStats.UnitAnimator[Idle].wrapMode = WrapMode.Loop;

            Walk = "Stealth_Walk";
            UnitStats.UnitAnimator[Walk].wrapMode = WrapMode.Loop;

            Ladder_Idle = "Ladder_Idle";
            UnitStats.UnitAnimator[Ladder_Idle].wrapMode = WrapMode.Loop;

            Ladder_Climb = "Ladder_Climb";
            UnitStats.UnitAnimator[Ladder_Climb].wrapMode = WrapMode.Once;

            Ladder_Climb_Down = "Ladder_Climb_Down";
            UnitStats.UnitAnimator[Ladder_Climb_Down].wrapMode = WrapMode.Once;

            Ladder_Get_On = "Ladder_Get_On";
            UnitStats.UnitAnimator[Ladder_Get_On].wrapMode = WrapMode.PingPong;

            Ladder_Get_On_From_Up = "Ladder_Get_On_From_Up";
            UnitStats.UnitAnimator[Ladder_Get_On_From_Up].wrapMode = WrapMode.Once;

            Ladder_Get_Up = "Ladder_Get_Up";
            UnitStats.UnitAnimator[Ladder_Get_Up].wrapMode = WrapMode.PingPong;

            Ladder_Get_Down = "Ladder_Get_Down";
            UnitStats.UnitAnimator[Ladder_Get_Down].wrapMode = WrapMode.PingPong;

            Ladder_Get_Down_Fast = "Ladder_Get_Down_Fast";
            UnitStats.UnitAnimator[Ladder_Get_Down_Fast].wrapMode = WrapMode.PingPong;
        }
        else
        {
            Walk = "Sword_Walk";
            UnitStats.UnitAnimator[Walk].wrapMode = WrapMode.Loop;

            Atack_Sword_1 = "Sword_Atack_1_Front";
            UnitStats.UnitAnimator[Atack_Sword_1].wrapMode = WrapMode.PingPong;

            Atack_Sword_2 = "Sword_Atack_2_Front";
            UnitStats.UnitAnimator[Atack_Sword_2].wrapMode = WrapMode.Once;

            Atack_Sword_Charge = "Sword_Charge_Atack";
            UnitStats.UnitAnimator[Atack_Sword_Charge].wrapMode = WrapMode.PingPong;

            Atack_Sword_3_C = "Sword_Atack_Charged_Front";
            UnitStats.UnitAnimator[Atack_Sword_3_C].wrapMode = WrapMode.PingPong;

            Idle = "Sword_Idle";
            UnitStats.UnitAnimator[Idle].wrapMode = WrapMode.Loop;

            refreshAtack();
        }

        curentAnimation = Idle;

        PlayAnimation(0);

        // Rotate Head
        //endMarker = new Vector3(Head.eulerAngles.x, Head.eulerAngles.y + 160f, Head.eulerAngles.z);
        //to = Quaternion.Euler(endMarker);

        //myRotation = Head.rotation;
    }

    public void PlayAnimation(int animation, bool isLastAction = true)
    {
        if (lastAnimation != animation)
        {
            lastAnimation = animation;  // we use this so we dont repeat the same call if that call happens.

            GoIntoIdleAfterAnimation = isLastAction;

            #region StandingAnimations [0,1]

            // Animaition Idle
            if (animation == 0 && !dontGoIntoIdle)
            {
                refreshAtack();

                UnitStats.UnitPrimaryState = UnitPrimaryState.Idle;

                curentAnimation = Idle;

                UnitStats.UnitAnimator.CrossFade(Idle);
            }
            // Animation Walk
            else if (animation == 1)
            {
                refreshAtack();

                UnitStats.UnitPrimaryState = UnitPrimaryState.Walking;

                curentAnimation = Walk;
                UnitStats.UnitAnimator.CrossFade(Walk);
            }
            #endregion

            #region  Climbing Ladder [10,11,12,13]

            // Action type 1 = LADDER actions
            else if (ActionType == 1)
            {
                /*
                 Ladder Get On
                 */
                if (animation == 10)
                {
                    dontGoIntoIdle = true;
                    ControllerFollowRoot = true;
                    UnitStats.AIPath.stopMoving();

                    curentAnimation = Ladder_Get_On;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Get_On);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Get_On].length, 13));
                    if (debuging)
                        Debug.Log("Get on the ladder to climb it.");
                }
                else if (animation == 17)
                {
                    dontGoIntoIdle = true;
                    ControllerFollowRoot = true;
                    UnitStats.AIPath.stopMoving();

                    curentAnimation = Ladder_Get_On_From_Up;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Get_On_From_Up);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Get_On_From_Up].length, 13));
                    if (debuging)
                        Debug.Log(" * Get on the ladder from up");
                }
                /*
                 Ladder Climb
                 */
                else if (animation == 12)
                {
                    curentAnimation = Ladder_Climb;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Climb);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Climb].length, 13));
                    if (debuging)
                        Debug.Log(" * Avatar : Im climing the ladder.");
                }

                else if (animation == 16)
                {
                    curentAnimation = Ladder_Climb_Down;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Climb_Down);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Climb_Down].length, 13));
                    if (debuging)
                        Debug.Log(" * Avatar : Im climing the ladder Down!.");
                }
                else if (animation == 13)
                {
                    curentAnimation = Ladder_Idle;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Idle);
                    if (debuging)
                        Debug.Log("On Ladder Idle.");
                }
                /*
                 Ladder get off
                 */
                else if (animation == 14)
                {
                    curentAnimation = Ladder_Get_Down_Fast;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Get_Down_Fast);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Get_Down_Fast].length, 0, true));

                    if (debuging)
                        Debug.Log("Get off the ladder FAST.");
                }
                else if (animation == 15)
                {
                    curentAnimation = Ladder_Get_Up;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Get_Up);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Get_Up].length, 0, true));

                    Debug.Log("Get off the ladder UP.");
                }
                else if (animation == 11)
                {
                    curentAnimation = Ladder_Get_Down;
                    UnitStats.UnitAnimator.CrossFade(Ladder_Get_Down);
                    StartCoroutine(WaitForEndOfAnimation_EVENT(UnitStats.UnitAnimator[Ladder_Get_Down].length, 0, true));
                    if (debuging)
                        Debug.Log("Get off the ladder.");
                }
            }
            #endregion

            #region Atack with sword    [2,3,4,5]
            else // Atack 1 in front 
                if (animation == 2 && !dontAtackInFront1)
                {
                    // When we atack we want to stop another call to this atack. (dont atack in front.)
                    // StartTheAnimation
                    // 
                    curentAnimation = Atack_Sword_1;
                    // Stop other animations from overlaping

                    dontAtackInFront1 = true;
                    isAtackingInFront = true;

                    isAtacking(true);

                    UnitStats.UnitAnimator.CrossFade(Atack_Sword_1, 0.1f);

                    float Lenght = UnitStats.UnitAnimator[Atack_Sword_1].length;
                    StartCoroutine(WaitForAtackInFrontAnimation(Lenght / 2));
                }
                else // Atack 2 in front. 
                    if (animation == 3 && !dontAtackInFront2)
                    {
                        curentAnimation = Atack_Sword_2;

                        isAtackingInFront2 = true;

                        dontAtackInFront2 = true;
                        canAtackInFront2 = false;

                        isAtacking(true);

                        UnitStats.UnitAnimator.CrossFade(Atack_Sword_2, 0.1f);

                        float Lenght = UnitStats.UnitAnimator[Atack_Sword_2].length;
                        StartCoroutine(WaitForAtackInFrontAnimation2(Lenght / 2));
                    }
                    else if (animation == 4)
                    {
                        curentAnimation = Atack_Sword_Charge;

                        isIdle = false;

                        isAtacking(true);

                        isCharging = true;
                        UnitStats.UnitAnimator.CrossFade(Atack_Sword_Charge);
                    }
                    else if (animation == 5 && !dontAtackInFront3)
                    {
                        curentAnimation = Atack_Sword_3_C;

                        isAtacking(true);

                        isAtackingInFront3 = true;
                        dontAtackInFront3 = true;

                        UnitStats.UnitAnimator.CrossFade(Atack_Sword_3_C);

                        float Lenght = UnitStats.UnitAnimator[Atack_Sword_3_C].length;
                        StartCoroutine(WaitForAtackInFrontAnimation3Charged(Lenght));
                    }
            #endregion

        }
    }

    #region Atack waiters.

    IEnumerator WaitForAtackInFrontAnimation(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);

        //UnitStats.AIPath.stepAtack(false);
        canAtackInFront2 = true;

        if (UnitStats.AIControlled)
        {
            UnitStats.UnitBaseAI.EnemyNear();
        }

        yield return new WaitForSeconds(moveTime - 0.1f);
        if (isAtackingInFront)
        {
            lastAnimation = 2;
            canAtackInFront2 = false;

            //UnitStats.thisModelAnimation.CrossFade(Idle, 2.5f);
            isAtacking(false);
            PlayAnimation(0);

            refreshAtack();
        }
    }
    IEnumerator WaitForAtackInFrontAnimation2(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);

        //UnitStats.AIPath.stepAtack(false);

        yield return new WaitForSeconds(moveTime);

        lastAnimation = 2;
        isAtacking(false);
        PlayAnimation(0);

        refreshAtack();

        if (UnitStats.AIControlled)
        {
            UnitStats.UnitBaseAI.EnemyNear();
        }
    }
    IEnumerator WaitForAtackInFrontAnimation3Charged(float moveTime)
    {
        //UnitStats.AIPath.stepAtack(true);
        yield return new WaitForSeconds(moveTime - 0.1f);
        if (isAtackingInFront3)
        {
            isAtackingInFront3 = false;
            refreshAtack();

            isAtacking(false);

            UnitStats.AIPath.stopMoving();
            //UnitStats.AIPath.stepAtack(false);

            PlayAnimation(0);

            refreshCharge();
        }
    }

    #endregion

    

    IEnumerator WaitForEndOfAnimation_EVENT(float animTime, int nextAnimationIndex, bool cancelAction = false)
    {
        yield return new WaitForSeconds(animTime);
        //if (UnitStats.UnitActionHandler.actionIndex == UnitStats.UnitActionHandler.actionOrder.Count)
        if (GoIntoIdleAfterAnimation)
            PlayAnimation(nextAnimationIndex);

        UnitStats.UnitActionHandler.setIsPlayingAction(false);
        if (cancelAction)
        {
            if (debuging)
                Debug.Log("Canceled action and go idle.");
            UnitStats.FeetCollider.SetActive(true);
            setPlayerOffActions(ActionType);
            UnitStats.UnitController.ResumeMoving();

            UnitStats.UnitActionHandler.ResetActions();
        }
        else
        {
            UnitStats.UnitActionHandler.StartAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Atack
        if (Input.GetKey(KeyCode.A))
        {
            //Atack(1);
        }
        #endregion

        if (ControllerFollowRoot)
        {
            UnitStats.thisTransform.position = new Vector3(UnitStats.Root.position.x, UnitStats.Root.position.y + 1, UnitStats.Root.position.z);
            var rot = new Quaternion();
            rot.eulerAngles = new Vector3(UnitStats.Root.eulerAngles.x + 90, UnitStats.Root.eulerAngles.y - 90, UnitStats.Root.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(UnitStats.thisTransform.rotation, rot, Time.deltaTime * 5);
        }
    }

    #region Animation Scripting

    #region Try_Outs
    // AnimationBones
    //public Transform Body;
    //public Transform Neck;
    //public Transform Head;
    Quaternion to;

    Vector3 endMarker;
    float startTime;
    float journeyLength;

    Quaternion myRotation;
    Quaternion lastUpdate;
    #endregion

    #endregion

    void LateUpdate()
    {
        //myRotation = Quaternion.Slerp(myRotation, to, Time.deltaTime * 0.2f);
        //Head.rotation = myRotation;
    }

    void setPlayerOffActions(int actionType = 0)
    {
        switch (actionType)
        {
            case 1:
                ActionType = 0;

                break;
            default:
                break;
        }

        dontGoIntoIdle = false;
        ControllerFollowRoot = false;
        //UnitStats.UnitActionHandler.currentActionIndex = 0;
    }

    public void Atack(int type)
    {
        Debug.Log(" Atack");
        switch (type)
        {
            case 1:
                /*
                //  Atack charged
                if (!dontAtackInFront1 && canAtackInFront1 && !dontAtackInFront2 && !canAtackInFront2)
                {
                    chargeTime += chargeRate * Time.deltaTime;

                    PlayAnimation(4);
                    UnitStats.AIPath.stopMoving();

                    if (atackTime <= chargeTime)
                    {
                        refreshCharge();

                        canAtackInFront1 = false;

                        isCharging = false;
                        isAtackCharged = true;

                        PlayAnimation(5);
                        UnitStats.AIPath.stopMoving();
                        UnitStats.AIPath.stepAtack(true);
                    }
                }
                 */
                //  Atack normally
                //else
                if (!isAtackCharged && !dontAtackInFront2 && !canAtackInFront2 /* && isCharging*/)
                {
                    refreshCharge();

                    isCharging = false;
                    isAtackCharged = false;

                    PlayAnimation(2);
                    UnitStats.AIPath.stopMoving();
                    //UnitStats.AIPath.stepAtack(true);
                }
                //  Atack the second time.
                else if (!isAtackCharged)
                {
                    if (!dontAtackInFront2 && canAtackInFront2)
                    {
                        refreshCharge();

                        isAtackingInFront = false;

                        PlayAnimation(3);
                        //UnitStats.AIPath.stepAtack(true);
                    }
                }
                break;
            case 2:

                break;
            default:
                break;
        }

    }

}
