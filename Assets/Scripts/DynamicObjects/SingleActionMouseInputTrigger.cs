using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class SingleActionMouseInputTrigger : MonoBehaviour
{
    [HideInInspector]
    private SceneManager SceneManager;

    #region Use this for initialization

    [HideInInspector]
    private ChairActionHandler ChairActionHandler;

    public void Initialize(ChairActionHandler chairActionHandler)
    {
        this.ChairActionHandler = chairActionHandler;
        this.SceneManager = ChairActionHandler.ChairStats.SceneManager;
    }

    #endregion

    void OnMouseOver()
    {
        this.HighlightParentObject(true);
        if (Input.GetMouseButtonDown((int)MouseInput.RightClick))
        {
            this.SetActionOnParent();
        }
        this.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Grab);
    }
    void OnMouseExit()
    {
        this.HighlightParentObject(false);
        this.SceneManager.CameraControl.CameraCursor.ChangeCursor(CursorType.Default);
    }

    //  Events
    private void SetActionOnParent()
    {
        if (this.ChairActionHandler)
            this.ChairActionHandler.SetAction(0);
    }
    private void HighlightParentObject(bool value)
    {
        if (this.ChairActionHandler)
            this.ChairActionHandler.HighlightObject(value);
    }
}
