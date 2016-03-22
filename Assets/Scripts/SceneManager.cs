using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class SceneManager : MonoBehaviour
{
    [HideInInspector]
    public MapInputTrigger Map;

    public Unit Player;

    // The only awake function in the game !!
    void Awake()
    {
        GlobalData.SceneManager = this;

        GlobalData.Player = Player;
        Player.Initialize();

        GlobalData.CameraControl = Camera.main.GetComponent<CameraControl>();
        GlobalData.CameraControl.Initialize();

        var itemsInScene = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in itemsInScene)
        {
            item.GetComponent<InteractiveObject>().Initialize();
        }

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "ViewEditor":    // HARD_CODED
                    child.transform.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}