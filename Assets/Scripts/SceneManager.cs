using UnityEngine;
using System.Collections;
using Pathfinding;
using Assets.Scripts.Types;

public class SceneManager : MonoBehaviour
{
    [HideInInspector]
    public MapInputTrigger Map;

    // The only awake function in the game !!
    void Awake()
    {
        GlobalData.SceneManager = this;
    }
}
