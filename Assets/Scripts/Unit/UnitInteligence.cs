using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class UnitInteligence : MonoBehaviour
{
    private Unit _unit;

    private int _layerMask;

    public GameObject StatusBarPanel;

    public Image StatusBar;
    private float _statusBarTime;
    private readonly float _statusBarMax = 100.0f;
    private float _statusBarCurr;

    private Vector3 _alertPosition;
    private Vector3 _lastPlayerPosition;
    
    private AlertType _alertType;
    public AlertType AlertType
    {
        get { return _alertType; }
        set
        {
            _alertType = value;
            switch (_alertType)
            {
                case AlertType.HearingPlayer:

                    BehaviourState = BehaviourState.Suspicious;

                    _unit.UnitController.TurnToTargetPosition = _alertPosition;
                    break;

                case AlertType.PlayerInFieldOfView:

                    if (CheckIfYouCanSeePlayer())
                    {
                        _lastPlayerPosition = _alertPosition;
                        StartCoroutine(Wait(1.2f,_alertType));
                    }
                    break;

                case AlertType.SeeingPlayer:

                    break;
            }
        }
    }

    private BehaviourState _behaviourState;
    public BehaviourState BehaviourState
    {
        get { return _behaviourState; }
        set
        {
            _behaviourState = value;
            switch (_behaviourState)
            {
                case BehaviourState.Suspicious:

                    if (StatusBarPanel.activeSelf == false)
                        StatusBarPanel.SetActive(true);

                    StatusBar.color = new Color32(251, 225, 76, 255); // yellow

                    StartStatusBar();
                    StartCoroutine("WaitStatus");

                    break;
                case BehaviourState.Agressive:
                    StatusBar.color = new Color32(255, 87, 76, 255); // red

                    CancelInvoke("UpdateStatusBar");
                    StartStatusBar();

                    StopCoroutine("WaitStatus");
                    StartCoroutine("WaitStatus");   

                    break;

                case BehaviourState.Idle:

                    if (StatusBarPanel.activeSelf)
                        StatusBarPanel.SetActive(false);

                    break;
            }
        }
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
        go.transform.eulerAngles = new Vector3(0, 0, 0);

        go.GetComponent<FieldOfHearing>().Initialize(this);

        // UI

        StatusBarPanel.SetActive(false);

        // layermasks
        var wallLayerMask = 1 << LayerMask.NameToLayer("WallOrObstacle");
        var unitLayerMask = 1 << LayerMask.NameToLayer("UnitInteraction");
        _layerMask = wallLayerMask | unitLayerMask; // Shoot ray only on wallLayer or unitLayer
    }

    private void BehaviourLogic()
    {
        //switch (hideFlags)
        //{


        //}
    }



    private bool CheckIfYouCanSeePlayer()
    {
        var direction = Logic.GetDirection(_unit.UnitProperties.HeadPosition, _alertPosition);
        RaycastHit hit;
        if (Physics.Raycast(new Ray(_unit.UnitProperties.HeadPosition, direction), out hit, float.PositiveInfinity, _layerMask))
        {
            return hit.transform.tag == "Player";
        }
        return false;
    }

    IEnumerator Wait(float time, AlertType condition)
    {
        yield return new WaitForSeconds(time);

        switch (condition)
        {
            case AlertType.PlayerInFieldOfView:
                if (CheckIfYouCanSeePlayer())
                {
                    BehaviourState = BehaviourState.Agressive;
                }
                // else the WaitStatus for Behaviour.Suspicious is going to end the alert.
                break;
        }
    }

    private void StartStatusBar()
    {
        _statusBarTime = 10.0f;
        _statusBarCurr = _statusBarMax; // 100
        var statusBarRepeatRate = _statusBarTime / _statusBarMax;

        InvokeRepeating("UpdateStatusBar", 0, statusBarRepeatRate); // ex: 3s -> 0.33
    }

    private IEnumerator WaitStatus()
    {
        yield return new WaitForSeconds(_statusBarTime);

        switch (BehaviourState)
        {
            case BehaviourState.Suspicious:

                BehaviourState = BehaviourState.Idle;
                break;

            case BehaviourState.Agressive:
                BehaviourState = BehaviourState.Suspicious;
                break;
        }

        CancelInvoke("UpdateStatusBar");
    }

    private void UpdateStatusBar()
    {
        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax; //70 / 100 = 0.7 din bara
    }

    public void Alert(Vector3 position, AlertType alertType)
    {
        _alertPosition = position;
        AlertType = alertType;
    }
}