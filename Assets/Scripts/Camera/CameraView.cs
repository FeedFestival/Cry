using UnityEngine;
using UnityEditor;
using System.Collections;
using Assets.Scripts.Types;

public class CameraView : MonoBehaviour
{
    private CameraControl CameraControl;

    void Awake()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        Screen.SetResolution(1280, 720, true);
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void Initialize(CameraControl cameraControl)
    {
        CameraControl = cameraControl;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(10, 10, 200, 150), "Loader Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 100, 20), "TimeScale 0.1"))
        {
            Time.timeScale = 0.1f;
        }

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 60, 100, 20), "TimeScale 0.5"))
        {
            Time.timeScale = 0.5f;
        }

        // Make the second button.
        if (GUI.Button(new Rect(20, 80, 100, 20), "TimeScale 1"))
        {
            Time.timeScale = 1;
        }
    }
}
