using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CameraControl : MonoBehaviour
{
    /*
     * Reads input from mouse
     * -    It handles the panning of the camera.
     */
    public bool debug;

    private Camera Camera;

    [HideInInspector]
    public CameraCursor CameraCursor;
    [HideInInspector]
    public CameraView CameraView;

    [HideInInspector]
    public HUD HUD;

    [HideInInspector]
    KeyboardInput KeyboardInput;

    [HideInInspector]
    public float YDistanceFromPlayer;

    [HideInInspector]
    public Transform thisTransform;

    private Vector3 currentScreenPos;

    private float minCameraPanSpeed = 4.44f;
    private float CameraPanSpeed;

    private Vector3 vectorUp = new Vector3(0, 0.77f, 0.44f);
    private Vector3 cameraDirection;

    void Awake()
    {
        GlobalData.CameraControl = this;
        Initialize();
    }

    public void Initialize()
    {
        //  Scripts initialization
        CameraCursor = GetComponent<CameraCursor>();
        CameraCursor.Initialize();

        Camera = GetComponent<Camera>();

        CameraView = GetComponent<CameraView>();
        //CameraView.Initialize(this);

        KeyboardInput = GetComponent<KeyboardInput>();
        KeyboardInput.Initialize(this);

        HUD = GetComponent<HUD>();
        HUD.Initialize();

        //  Props
        YDistanceFromPlayer = 18f;

        thisTransform = this.transform;

        CameraPanSpeed = minCameraPanSpeed;
    }

    void Update()
    {
        if (CheckYDistance())
        {
            var desiredPosition = new Vector3(thisTransform.position.x,
                                                GlobalData.Player.UnitProperties.thisTransform.position.y + YDistanceFromPlayer,
                                                thisTransform.position.z);
            thisTransform.position = Vector3.Lerp(thisTransform.position, desiredPosition, Time.deltaTime * 2f);
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            CameraPan();
        }
    }

    private void CameraPan()
    {
        currentScreenPos = Camera.ScreenToViewportPoint(Input.mousePosition);
        cameraDirection = GetCameraDirection();

        if (cameraDirection != Vector3.zero)
        {
            thisTransform.Translate(cameraDirection * CameraPanSpeed * Time.deltaTime);
        }
    }

    private float bigLowSize = 0.15f;
    private float mediumLowSize = 0.1f;
    private float minimmumLowSize = 0.05f;

    private float bigHighSize = 0.80f;
    private float mediumHighSize = 0.85f;
    private float minimmumHighSize = 0.9f;

    private Vector3 GetCameraDirection()
    {
        // left
        if (currentScreenPos.x <= bigLowSize)
        {
            if (currentScreenPos.x <= mediumLowSize)
            {
                if (currentScreenPos.x <= minimmumLowSize)
                {
                    CameraPanSpeed = minCameraPanSpeed / 1.2f;
                    if (currentScreenPos.y >= minimmumHighSize)
                    {
                        return (vectorUp + (-Vector3.right));
                    }
                    else if (currentScreenPos.y <= minimmumLowSize)
                    {
                        return ((-vectorUp) + (-Vector3.right));
                    }
                    CameraPanSpeed = minCameraPanSpeed;
                    return -Vector3.right;
                }
                CameraPanSpeed = minCameraPanSpeed / 3;
                return -Vector3.right;
            }
            CameraPanSpeed = minCameraPanSpeed / 6;
            return -Vector3.right;
        }
        // right
        else if (currentScreenPos.x >= bigHighSize)
        {
            if (currentScreenPos.x >= mediumHighSize)
            {
                if (currentScreenPos.x >= minimmumHighSize)
                {
                    CameraPanSpeed = minCameraPanSpeed / 1.2f;
                    if (currentScreenPos.y >= minimmumHighSize)
                    {
                        return (vectorUp + Vector3.right);
                    }
                    else if (currentScreenPos.y <= minimmumLowSize)
                    {
                        return ((-vectorUp) + Vector3.right);
                    }
                    CameraPanSpeed = minCameraPanSpeed;
                    return Vector3.right;
                }
                CameraPanSpeed = minCameraPanSpeed / 3;
                return Vector3.right;
            }
            CameraPanSpeed = minCameraPanSpeed / 6;
            return Vector3.right;
        }
        // up
        else if (currentScreenPos.y >= bigHighSize)
        {
            if (currentScreenPos.y >= mediumHighSize)
            {
                if (currentScreenPos.y >= minimmumHighSize)
                {
                    CameraPanSpeed = minCameraPanSpeed / 1.2f;
                    if (currentScreenPos.x <= minimmumLowSize)
                    {
                        return (vectorUp + (-Vector3.right));
                    }
                    else if (currentScreenPos.x >= minimmumHighSize)
                    {
                        return (vectorUp + Vector3.right);
                    }
                    CameraPanSpeed = minCameraPanSpeed;
                    return vectorUp;
                }
                CameraPanSpeed = minCameraPanSpeed / 3;
                return vectorUp;
            }
            CameraPanSpeed = minCameraPanSpeed / 6;
            return vectorUp;
        }
        // down
        else if (currentScreenPos.y <= bigLowSize)
        {
            if (currentScreenPos.y <= mediumLowSize)
            {
                if (currentScreenPos.y <= minimmumLowSize)
                {
                    CameraPanSpeed = minCameraPanSpeed / 1.2f;
                    if (currentScreenPos.x <= minimmumLowSize)
                    {
                        return ((-vectorUp) + (-Vector3.right));
                    }
                    else if (currentScreenPos.x >= minimmumHighSize)
                    {
                        return ((-vectorUp) + Vector3.right);
                    }
                    CameraPanSpeed = minCameraPanSpeed;
                    return -vectorUp;
                }
                CameraPanSpeed = minCameraPanSpeed / 3;
                return -vectorUp;
            }
            CameraPanSpeed = minCameraPanSpeed / 6;
            return -vectorUp;
        }
        return Vector3.zero;
    }

    private bool CheckYDistance()
    {
        if (GlobalData.Player != null)
        {
            var distance = Mathf.Round((GlobalData.Player.UnitProperties.thisTransform.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
            var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
            if (distance != cameraCurrentPosition)
            {
                return true;
            }
        }
        return false;
    }
}