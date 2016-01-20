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

        //GlobalData.AllItemsInScene = new System.Collections.Generic.List<Item>();
        //var gos = GameObject.FindGameObjectsWithTag("Item");

        //foreach (GameObject go in gos)
        //{
        //    var item = Items.CreateItem(go.GetComponent<InteractiveObject>().ItemName);
        //    go.GetComponent<InteractiveObject>().Item = item;
        //    go.GetComponent<InteractiveObject>().Initialize();
            
        //    GlobalData.AllItemsInScene.Add(item);
        //}

        GlobalData.CameraControl.Initialize();
    }
}
