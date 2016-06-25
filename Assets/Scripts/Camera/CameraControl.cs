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

    private float minCameraPanSpeed = 4.42f;
    private float CameraPanSpeed;

    private Vector3 vectorUp = new Vector3(0, 0.73f, 0.46f);
    private Vector3 cameraDirection;

    private bool _centerCamera;
    public bool CenterCamera {
        set {
            _centerCamera = value;

            if (_centerCamera) {
                CameraPanSpeed = 2.3f;
            }
        }
        get {
            return _centerCamera;
        }
    }
    public bool PanCamera;

    Transform ObjectToFollow;

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

        ObjectToFollow = GlobalData.Player.transform; // HARD_CODED
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (!CenterCamera)
        //        CenterCamera = true;
        //}
        if (CenterCamera)
        {
            CenterCameraOn();
        }
        //if (PanCamera && !this.HUD.I_INVENTORY_button.pressed)
        //{
        //    thisTransform.Translate(cameraDirection * CameraPanSpeed * Time.deltaTime);
        //    if (CheckYDistance())
        //    {
        //        DesiredPosition = new Vector3(thisTransform.position.x, YDistanceFromPlayer, thisTransform.position.z);
        //        thisTransform.position = Vector3.Lerp(thisTransform.position, DesiredPosition, Time.deltaTime * 2f);
        //    }
        //}
    }
    
    private void CenterCameraOn()
    {
        if (GlobalData.Player != null)
        {
            if (HUD.I_INVENTORY_button.pressed == false)
            {
                if (ObjectToFollow == null)
                {
                    ObjectToFollow = GlobalData.Player.transform;
                }
                DesiredPosition = new Vector3(ObjectToFollow.position.x - 8f, YDistanceFromPlayer, ObjectToFollow.position.z + 8f);
            }
            else
            {
                //Debug.Log(DesiredPosition);
                DesiredPosition = new Vector3(ObjectToFollow.position.x - 4.30f, 11f, ObjectToFollow.position.z + 4.30f);
            }
            thisTransform.position = Vector3.Lerp(thisTransform.position, DesiredPosition, Time.deltaTime * CameraPanSpeed);
        }
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
            if (ObjectToFollow == null)
            {
                ObjectToFollow = GlobalData.Player.transform; // HARD_CODED
            }
            var distance = Mathf.Round((ObjectToFollow.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
            var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
            if (distance != cameraCurrentPosition)
            {
                return true;
            }
        }
        return false;
    }
}