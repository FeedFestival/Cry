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
            if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.Moving)
            {
                CameraControl.CameraCursor.ChangeCursor(CursorType.Grab);
                GlobalData.Player.PlayerActionInMind = PlayerActionInMind.UseAbility;
            }
            else if (GlobalData.Player.PlayerActionInMind == PlayerActionInMind.MovingTable)
            {
                GlobalData.Player.UnitActionInMind = UnitActionInMind.DropTable;
                GlobalData.Player.Table.TableActionHandler.PlayActionAnimation();
            }
        }
    }
}
