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

        var gos = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject go in gos)
        {
            InventoryObject io = new InventoryObject
            {
                InteractiveObject = go.GetComponent<InteractiveObject>()
            };
            go.GetComponent<InteractiveObject>().Item = io;

            GlobalData.AllItemsInScene.Add(io);
        }
    }
}
