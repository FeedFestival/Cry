using UnityEngine;
using System.Collections;

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

    //  Player input;
    private bool leftClickFlag = true;

    private Transform Target;

    private string floorTag;

    [HideInInspector]
    public float YDistanceFromPlayer;

    //    Camera movement;
    [HideInInspector]
    public Transform thisTransform;

    Rect recdownFast;
    Rect recupFast;
    Rect recleftFast;
    Rect recrightFast;

    Rect recdownMed;
    Rect recupMed;
    Rect recleftMed;
    Rect recrightMed;

    Rect recdownSlow;
    Rect recupSlow;
    Rect recleftSlow;
    Rect recrightSlow;

    public bool LockCamera;

    float CamSpeed = 0;

    float CamSpeedFast = 0.15f;
    float CamSpeedMed = 0.09f;
    float CamSpeedSlow = 0.05f;
    float CamSpeedSlowMoa = 0.01f;

    float GUIsizeSlow = 90;
    float GUIsizeMed = 60;
    float GUIsizeFast = 25;

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

        //  Transforms initialiations
        floorTag = "Map";

        Target = SceneManager.Player.GetComponent<UnitStats>().thisUnitTarget.transform;

        thisTransform = this.transform;

        recdownFast = new Rect(0, 0, Screen.width, GUIsizeFast);
        recupFast = new Rect(0, Screen.height - GUIsizeFast, Screen.width, GUIsizeFast);
        recleftFast = new Rect(0, 0, GUIsizeFast, Screen.height);
        recrightFast = new Rect(Screen.width - GUIsizeFast, 0, GUIsizeFast, Screen.height);

        recdownMed = new Rect(0, 0, Screen.width, GUIsizeMed);
        recupMed = new Rect(0, Screen.height - GUIsizeMed, Screen.width, GUIsizeMed);
        recleftMed = new Rect(0, 0, GUIsizeMed, Screen.height);
        recrightMed = new Rect(Screen.width - GUIsizeMed, 0, GUIsizeMed, Screen.height);

        recdownSlow = new Rect(0, 0, Screen.width, GUIsizeSlow);
        recupSlow = new Rect(0, Screen.height - GUIsizeSlow, Screen.width, GUIsizeSlow);
        recleftSlow = new Rect(0, 0, GUIsizeSlow, Screen.height);
        recrightSlow = new Rect(Screen.width - GUIsizeSlow, 0, GUIsizeSlow, Screen.height);
    }

    void Update()
    {
        // Right Click control;

        if (Input.GetKey(KeyCode.Mouse1) && leftClickFlag)
            leftClickFlag = false;

        if (!Input.GetKey(KeyCode.Mouse1) && !leftClickFlag)
        {
            RightClickMove();
        }

        // Camera Panning

        if (!Input.GetKey(KeyCode.Space) && !LockCamera)
        {
            //thisTransform.parent = null;

            if (recdownSlow.Contains(Input.mousePosition) || recdownMed.Contains(Input.mousePosition) || recdownFast.Contains(Input.mousePosition))
            {
                if (recdownFast.Contains(Input.mousePosition))
                {
                    CamSpeed = CamSpeedFast;
                }
                else
                    if (recdownMed.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedMed;
                    }
                    else
                    {
                        CamSpeed = CamSpeedSlow;
                    }
            }

            if (recupSlow.Contains(Input.mousePosition) || recupMed.Contains(Input.mousePosition) || recupFast.Contains(Input.mousePosition))
            {
                if (recupFast.Contains(Input.mousePosition))
                {
                    CamSpeed = CamSpeedFast;
                }
                else
                    if (recupMed.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedMed;
                    }
                    else
                    {
                        CamSpeed = CamSpeedSlow;
                    }
            }
            if (recleftSlow.Contains(Input.mousePosition) || recleftMed.Contains(Input.mousePosition) || recleftFast.Contains(Input.mousePosition))
            {
                if (recleftFast.Contains(Input.mousePosition))
                {
                    CamSpeed = CamSpeedFast;
                }
                else
                    if (recleftMed.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedMed;
                    }
                    else
                    {
                        CamSpeed = CamSpeedSlow;
                    }
            }
            if (recrightSlow.Contains(Input.mousePosition) || recrightMed.Contains(Input.mousePosition) || recrightFast.Contains(Input.mousePosition))
            {
                if (recrightFast.Contains(Input.mousePosition))
                {
                    CamSpeed = CamSpeedFast;
                }
                else
                    if (recrightMed.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedMed;
                    }
                    else
                    {
                        CamSpeed = CamSpeedSlow;
                    }
            }
        }
    }

    private void RightClickMove()
    {
        leftClickFlag = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (debug)
                Debug.Log("Ray launched");

            if (hit.transform.tag == floorTag)
            {
                Target.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                if (debug)
                    Debug.Log("target_move to this pos : " + Target.position);
                
                SceneManager.Player.GetComponent<UnitController>().GoToTarget();
            }
        }
    }
}