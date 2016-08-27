using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class DoorAnimation : MonoBehaviour
{
    public bool debuging = false;

    [HideInInspector]
    private Door Door;

    public void Initialize(Door door)
    {
        Door = door;

        SetupAnimations();
    }

    public void Play(string animationString)
    {
        Door.DoorAnimator.CrossFade(animationString);

        float animationLenght = Door.DoorAnimator[animationString].length;
        if (debuging)
            Debug.Log("an - " + animationString + " , lenght = " + animationLenght);

        StartCoroutine(WaitForEndOfAnimation(animationLenght));
    }

    IEnumerator WaitForEndOfAnimation(float animTime)
    {
        yield return new WaitForSeconds(animTime);
        Door.DoorAnimator.Stop();

        if (Door.DoorState == DoorState.Closed)
        {
            Door.DoorState = DoorState.Open;
        }
        else
        {
            Door.DoorState = DoorState.Closed;
        }

        GlobalData.Player.UnitActionHandler.ExitCurentAction();
    }

    void SetupAnimations()
    {
        Door.DoorAnimator[DoorAnimations.OpenDoor.ToString()].wrapMode = WrapMode.PingPong;
        Door.DoorAnimator[DoorAnimations.CloseDoor.ToString()].wrapMode = WrapMode.PingPong;
    }
}
