using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class UnitInteligence : MonoBehaviour
{
    private Unit _unit;

    //private FieldOfView fieldOfView;
    //private FieldOfHearing fieldOfHearing;

    /*
     * State bools
     */
    private bool _isHearingPlayer;
    private bool _isPlayerInFieldOfView;
    private bool _isSeeingPlayer;

    [HideInInspector]
    public bool IsHearingPlayer
    {
        set
        {
            _isHearingPlayer = value;
            if (_isHearingPlayer)
            {
                Debug.Log("AI [" + _unit.gameObject.name + "] - is hearing player near him.");

                // turn to investigate
            }
        }
        get { return _isHearingPlayer; }
    }
    [HideInInspector]
    public bool IsPlayerInFieldOfView
    {
        set
        {
            _isPlayerInFieldOfView = value;
            if (_isPlayerInFieldOfView)
                Debug.Log("AI [" + _unit.gameObject.name + "] - THINKS is seeing the player in front of him. IS UNSURE");
        }
        get { return _isPlayerInFieldOfView; }
    }
    [HideInInspector]
    public bool IsSeeingPlayer
    {
        set
        {
            _isSeeingPlayer = value;
            if (_isSeeingPlayer)
                Debug.Log("AI [" + _unit.gameObject.name + "] - IS seeing the player in front of him.");
        }
        get { return _isSeeingPlayer; }
    }

    public void Initialize(Unit unit)
    {
        _unit = unit;

        var go = Logic.CreateFromPrefab("Prefabs/UI/FieldOfView", transform.position);

        go.transform.parent = transform;
        go.name = "AI_FieldOfView";
        go.layer = 11;
        go.transform.eulerAngles = new Vector3(270, 0, 0);

        go.GetComponent<FieldOfView>().Initialize(this);
        //--
        go = Logic.CreateFromPrefab("Prefabs/UI/FieldOfHearing", transform.position);

        go.transform.parent = transform;
        go.name = "AI_FieldOfHearing";
        go.layer = 11;
        go.transform.eulerAngles = new Vector3(270, 0, 0);

        go.GetComponent<FieldOfHearing>().Initialize(this);
    }
}