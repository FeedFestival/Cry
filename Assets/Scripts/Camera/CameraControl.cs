using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class CameraControl : MonoBehaviour
{
    /*
     * Reads input from mouse
     * -    It handles the panning of the camera.
     * -    It sets the Right Click events.
     */
    public bool debug;

    [HideInInspector]
    public CameraCursor CameraCursor;
    [HideInInspector]
    public CameraView CameraView;
    [HideInInspector]
    KeyboardInput KeyboardInput;

    [HideInInspector]
    public float YDistanceFromPlayer;

    [HideInInspector]
    public Transform thisTransform;

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

        CameraView = GetComponent<CameraView>();
        //CameraView.Initialize(this);

        KeyboardInput = GetComponent<KeyboardInput>();
        KeyboardInput.Initialize(this);

        //  Props
        YDistanceFromPlayer = 18f;

        thisTransform = this.transform;
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
        
    }

    private bool CheckYDistance()
    {
        
        var distance = Mathf.Round((GlobalData.Player.UnitProperties.thisTransform.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
        var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
        if (distance != cameraCurrentPosition)
        {
            //Debug.Log("Camera is wrong.");
            return true;
        }
        return false;
    }
}