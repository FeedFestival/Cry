using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CircleAction : MonoBehaviour
{
    [HideInInspector]
    public Transform thisTransform;

    public CircleActionState CircleActionState;

    private SpriteRenderer SpriteRenderer;

    [HideInInspector]
    public Sprite[] sprites;
    [HideInInspector]
    public Sprite active;

    float framesPerSecond;

    public int endIndex;
    public int index;
    bool forward;

    // Posible Parents
    [HideInInspector]
    public Ledge Ledge;

    // Use this for initialization
    public void Initialize(Ledge ledge)
    {
        Ledge = ledge;
        Start();
    }

    private void Start()
    {
        this.thisTransform = this.transform;
        framesPerSecond = 30.0f;

        SpriteRenderer = this.thisTransform.GetComponent<SpriteRenderer>();

        active = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI");

        sprites = new Sprite[11];
        sprites[0] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_11");
        sprites[1] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_22");
        sprites[2] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_33");
        sprites[3] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_44");
        sprites[4] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_50");
        sprites[5] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_60");
        sprites[6] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_70");
        sprites[7] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_80");
        sprites[8] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_85");
        sprites[9] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive_90");
        sprites[10] = Resources.Load<Sprite>("Sprites/ActionCircle/CircleUI_inactive");
    }

    public bool isPlaying;

    private IEnumerator Play()
    {
        isPlaying = true;
        while (GetStopCondition())
        {
            ChangeIndex();

            yield return new WaitForSeconds(1f / framesPerSecond);
        }
        EndInstruction();
    }
    private bool GetStopCondition()
    {
        if (forward)
        {
            if (index < endIndex)
                return true;
            return false;
        }
        else
        {
            if (index >= endIndex)
                return true;
            return false;
        }
    }
    private void ChangeIndex()
    {
        if (forward)
        {
            if (index < endIndex)
                SpriteRenderer.sprite = sprites[index];
            index++;
        }
        else
        {
            if (index > endIndex)
                SpriteRenderer.sprite = sprites[index];
            index--;
        }
    }
    private void EndInstruction()
    {
        isPlaying = false;
        if (forward)
        {
            if (CircleActionState == CircleActionState.Available)
                SpriteRenderer.sprite = active;
        }
        else
        {
            Hide();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thisTransform.rotation = Logic.SmoothLook(thisTransform.rotation,
                Logic.GetDirection(thisTransform.position, GlobalData.CameraControl.thisTransform.position),
                7f);
    }

    public void ShowAvailable()
    {
        if (isPlaying == false)
        {
            CircleActionState = CircleActionState.ShowAvailable;

            SpriteRenderer.enabled = true;

            endIndex = 10;
            index = 0;
            forward = true;
            framesPerSecond = 30.0f;
            StartCoroutine(Play());
        }
    }
    public void GoAvailable()
    {
        if (isPlaying == false)
        {
            if (CircleActionState == CircleActionState.Unavailable || CircleActionState == CircleActionState.None)
            {
                CircleActionState = CircleActionState.Available;

                SpriteRenderer.enabled = true;

                endIndex = 10;
                index = 0;
                forward = true;
                framesPerSecond = 60.0f;
                StartCoroutine(Play());
            }
        }
    }
    public void GoUnavailable()
    {
        if (CircleActionState == CircleActionState.ShowAvailable || CircleActionState == CircleActionState.Available)
        {
            CircleActionState = CircleActionState.Unavailable;

            SpriteRenderer.enabled = true;

            if (isPlaying)
            {
                // Continue animation
                endIndex = 0;
                forward = false;
            }
            else
            {
                endIndex = 0;
                index = 10;
                forward = false;

                framesPerSecond = 30.0f;
                StartCoroutine(Play());
            }
        }
        CircleActionState = CircleActionState.Unavailable;
    }
    public void Hide()
    {
        if (isPlaying == false)
        {
            CircleActionState = CircleActionState.None;
            SpriteRenderer.enabled = false;
            SetCircleActionStateOnParent();
        }
    }

    void SetCircleActionStateOnParent()
    {
        if (Ledge)
            Ledge.CircleActionState = CircleActionState;
    }
}