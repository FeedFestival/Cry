using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;

public class SceneManager : MonoBehaviour
{
    [HideInInspector]
    public MapInputTrigger Map;

    public Unit Player;

    public List<Unit> Enemies;

    // The only awake function in the game !!
    void Awake()
    {
        GlobalData.SceneManager = this;

        GlobalData.Player = Player;
        Player.Initialize(UnitType.Player);

        if (Enemies != null && Enemies.Count > 0)
        {
            foreach (Unit enemy in Enemies)
            {
                enemy.Initialize(UnitType.Enemy);
            }
        }

        GlobalData.CameraControl = Camera.main.GetComponent<CameraControl>();
        GlobalData.CameraControl.Initialize();

        var itemsInScene = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in itemsInScene)
        {
            item.GetComponent<InteractiveObject>().Initialize();
        }

        var visionInScene = GameObject.FindGameObjectsWithTag("Vision");
        foreach (GameObject vision in visionInScene)
        {
            vision.GetComponent<MeshRenderer>().enabled = false;
            var pos = vision.transform.position;
            vision.transform.position = new Vector3(pos.x, pos.y - 0.6f, pos.z);
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