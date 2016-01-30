using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class SceneManager : MonoBehaviour
{
    [HideInInspector]
    public MapInputTrigger Map;

    // The only awake function in the game !!
    void Awake()
    {
        GlobalData.SceneManager = this;

        GlobalData.CameraControl.Initialize();

        var itemsInScene = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in itemsInScene)
        {
            item.GetComponent<InteractiveObject>().Initialize();
        }
    }
}
