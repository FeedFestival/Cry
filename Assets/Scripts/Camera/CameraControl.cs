using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CameraControl : MonoBehaviour
{
    public bool debug;

    private CameraEdge CameraEdge;
    private CameraEdgeSpeed CameraEdgeSpeed;

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

    private Vector3 DesiredPosition;

    private float minCameraPanSpeed = 4.44f;
    private float CameraPanSpeed;

    private Vector3 vectorUp = new Vector3(0, 0.77f, 0.44f);
    private Vector3 cameraDirection;

    public bool CenterCamera;
    public bool PanCamera;

    Transform CameraOn;

    void Awake()
    {
        GlobalData.CameraControl = this;

        CameraOn = GlobalData.Player.transform; // HARD_CODED
    }

    public void Initialize()
    {
        //  Scripts initialization
        CameraCursor = GetComponent<CameraCursor>();
        CameraCursor.Initialize();

        CameraView = GetComponent<CameraView>();
        //CameraView.Initialize(this);

        KeyboardInput = GetComponent<KeyboardInput>();
        KeyboardInput.Initialize(this);

        //  Props
        YDistanceFromPlayer = 18f;

        thisTransform = this.transform;

        CameraPanSpeed = minCameraPanSpeed;

        HUD = GetComponent<HUD>();
        HUD.Initialize(this);

        CenterCamera = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!CenterCamera)
                CenterCamera = true;
        }
        if (CenterCamera)
        {
            CenterCameraOn();
        }
        if (PanCamera && !this.HUD.I_INVENTORY_button.pressed)
        {
            thisTransform.Translate(cameraDirection * CameraPanSpeed * Time.deltaTime);
            if (CheckYDistance())
            {
                DesiredPosition = new Vector3(thisTransform.position.x,
                                            CameraOn.position.y + YDistanceFromPlayer,
                                            thisTransform.position.z);
                thisTransform.position = Vector3.Lerp(thisTransform.position, DesiredPosition, Time.deltaTime * 2f);
            }
        }
    }

    private void CenterCameraOn()
    {
        if (!this.HUD.I_INVENTORY_button.pressed)
        {
            DesiredPosition = new Vector3(CameraOn.position.x - 7.22f,
                                                CameraOn.position.y + YDistanceFromPlayer,
                                                CameraOn.position.z + 7.22f);
        }
        else
        {
            // -5.77 // 
            DesiredPosition = new Vector3(CameraOn.position.x - 4.30f,
                                                CameraOn.position.y + 11f,
                                                CameraOn.position.z + 4.30f);
        }
        thisTransform.position = Vector3.Lerp(thisTransform.position, DesiredPosition, Time.deltaTime * 2f);
    }

    public void CameraPan(CameraEdge cameraEdge, CameraEdgeSpeed cameraEdgeSpeed)
    {
        if (GlobalData.Player.UnitPrimaryState != UnitPrimaryState.Walk)
        {
            CameraEdge = cameraEdge;
            CameraEdgeSpeed = cameraEdgeSpeed;
            MoveCameraDirection();
            CenterCamera = false;
            PanCamera = true;
        }
    }

    public void DontCameraPan()
    {
        PanCamera = false;
    }

    public void MoveCameraDirection()
    {
        if (CameraEdgeSpeed == CameraEdgeSpeed.Slow)
            CameraPanSpeed = minCameraPanSpeed / 4;
        else
            CameraPanSpeed = minCameraPanSpeed;

        switch (CameraEdge)
        {
            case CameraEdge.T:

                cameraDirection = vectorUp;
                break;

            case CameraEdge.TR:

                cameraDirection = (vectorUp + Vector3.right);
                break;

            case CameraEdge.R:

                cameraDirection = Vector3.right;
                break;

            case CameraEdge.DR:

                cameraDirection = ((-vectorUp) + Vector3.right);
                break;

            case CameraEdge.D:

                cameraDirection = -vectorUp;
                break;

            case CameraEdge.DL:

                cameraDirection = ((-vectorUp) + (-Vector3.right));
                break;

            case CameraEdge.L:

                cameraDirection = -Vector3.right;
                break;

            case CameraEdge.TL:

                cameraDirection = (vectorUp + (-Vector3.right));
                break;

            default:
                break;
        }
    }

    private bool CheckYDistance()
    {
        if (GlobalData.Player != null)
        {
            var distance = Mathf.Round((CameraOn.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
            var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
            if (distance != cameraCurrentPosition)
            {
                return true;
            }
        }
        return false;
    }
}