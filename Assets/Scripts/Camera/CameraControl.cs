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
    [HideInInspector]
    public SceneManager SceneManager { get; set; }

    public bool debug;

    [HideInInspector]
    public CameraCursor CameraCursor;
    [HideInInspector]
    public CameraView CameraView;

    [HideInInspector]
    public float YDistanceFromPlayer;

    [HideInInspector]
    public Transform thisTransform;

    public void Initialize(SceneManager sceneManager)
    {
        //  Scripts initialization
        SceneManager = sceneManager;

        CameraCursor = GetComponent<CameraCursor>();
        CameraCursor.Initialize();

        CameraView = GetComponent<CameraView>();
        CameraView.Initialize(this);

        //  Props
        YDistanceFromPlayer = 18f;

        thisTransform = this.transform;
    }

    void Update()
    {
        if (CheckYDistance())
        {
            var desiredPosition = new Vector3(thisTransform.position.x,
                                                SceneManager.PlayerStats.UnitProperties.thisTransform.position.y + YDistanceFromPlayer,
                                                thisTransform.position.z);
            thisTransform.position = Vector3.Lerp(thisTransform.position, desiredPosition, Time.deltaTime * 1.7f);
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
        if (SceneManager.PlayerStats != null && SceneManager.PlayerStats.UnitPrimaryState != UnitPrimaryState.Idle)
        {
            var distance = Mathf.Round((SceneManager.PlayerStats.UnitProperties.thisTransform.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
            var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
            if (distance != cameraCurrentPosition)
                return true;
        }
        return false;
    }
}