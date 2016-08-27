using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class DoorActionHandler : MonoBehaviour
{
    private Door Door;

    public void Initialize(Door door)
    {
        Door = door;
    }

    public void PlayActionAnimation(Unit unit = null)
    {
        if (Door.DoorState == DoorState.Locked)
        {
            // check unit inventory for a key.
            foreach (Item item in GlobalData.Player.UnitInventory.InventoryItems)
            {
                if (item.ItemType == ItemType.Key)
                {
                    if (item.Password == Door.DoorPassword)
                    {
                        Door.DoorState = DoorState.Closed;
                    }
                }
            }
            // if there is no key then the door remains locked until then.
        }
        if (Door.DoorState == DoorState.Closed)
        {
            if (Door.DoorStartPoint == DoorStartPoint.front)
                GlobalData.Player.UnitActionAnimation.PlaySingleAnimation(DoorAnimations.OpenDoor.ToString());
            else
                GlobalData.Player.UnitActionAnimation.PlaySingleAnimation(DoorAnimations.OpenDoorBack.ToString());
            Door.DoorAnimation.Play(DoorAnimations.OpenDoor.ToString());
        }
        else if (Door.DoorState == DoorState.Open)
        {
            if (Door.DoorStartPoint == DoorStartPoint.front)
                GlobalData.Player.UnitActionAnimation.PlaySingleAnimation(DoorAnimations.CloseDoor.ToString());
            else
                GlobalData.Player.UnitActionAnimation.PlaySingleAnimation(DoorAnimations.CloseDoorBack.ToString());
            Door.DoorAnimation.Play(DoorAnimations.CloseDoor.ToString());
        }
        else
        {
            GlobalData.Player.UnitActionHandler.ExitCurentAction();
        }
    }

    public void CalculateStartPoint()
    {
        var playerPos = GlobalData.Player.UnitProperties.ThisUnitTransform.position;

        float[] distancesLadder = new float[2];
        distancesLadder[(int)DoorStartPoint.front] = Vector3.Distance(playerPos, Door.front.position);
        distancesLadder[(int)DoorStartPoint.back] = Vector3.Distance(playerPos, Door.back.position);

        Door.DoorStartPoint = (DoorStartPoint)Logic.GetSmallestDistance(distancesLadder);
    }
}
