using UnityEngine;
using System.Collections;
using Pathfinding;

public class SceneManager : MonoBehaviour
{

    public GameObject Player;   // HARD_CODED

    [HideInInspector]
    public Unit PlayerStats;

    public CameraControl CameraControl; // HARD_CODED

    // The only awake function in the game !!
    void Awake()
    {
        PlayerStats = Player.GetComponent<Unit>();
        PlayerStats.Initialize(this);    // HARD_CODED

        CameraControl.Initialize(this);
    }
}
