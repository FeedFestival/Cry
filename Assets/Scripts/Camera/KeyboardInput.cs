using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class KeyboardInput : MonoBehaviour
{

    private CameraControl CameraControl;

    public void Initialize(CameraControl cameraControl)
    {
        CameraControl = cameraControl;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CameraControl.SceneManager.PlayerStats.PlayerActionInMind == PlayerActionInMind.Moving)
            {
                CameraControl.CameraCursor.ChangeCursor(CursorType.Grab);
                CameraControl.SceneManager.PlayerStats.PlayerActionInMind = PlayerActionInMind.UseAbility;
            }
            else if (CameraControl.SceneManager.PlayerStats.PlayerActionInMind == PlayerActionInMind.MovingTable)
            {
                CameraControl.SceneManager.PlayerStats.Table.TableActionHandler.PlayActionAnimation(TableAnimations.DropTable_FromBack);
            }
        }
    }
}
