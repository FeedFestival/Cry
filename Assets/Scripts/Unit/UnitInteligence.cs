using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class UnitInteligence : MonoBehaviour
{
    private Unit _unit;

    public Image StatusBar;
    private float _statusBarTime;
    private float _statusBarMax = 100.0f;
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

                    // shot ray
                    var direction = Logic.GetDirection(transform.position, _alertPosition);
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(transform.position, direction), out hit, 2.5f))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            _lastPlayerPosition = _alertPosition;
                            StartCoroutine(CheckIfPlayer());
                        }
                    }
                    
                    // if you hit Player
                    // wait for 1 or 2 seconds
                    // then at the end check if you can still hit the Player with the ray.
                    // then its time to go ape shit on his ass son

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
                    StatusBar.color = new Color32(251, 225, 76, 255); // yellow
                    break;
                case BehaviourState.Agressive:
                    StatusBar.color = new Color32(255, 87, 76, 255); // red
                    break;
            }
            StartCoroutine(StartStatusBar());
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
        go.transform.eulerAngles = new Vector3(270, 0, 0);

        go.GetComponent<FieldOfHearing>().Initialize(this);
    }


    private void BehaviourLogic()
    {
        //switch (hideFlags)
        //{


        //}
    }





    private IEnumerator CheckIfPlayer()
    {
        yield return new WaitForSeconds(1.2f);

        var direction = Logic.GetDirection(transform.position, _alertPosition);
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, direction), out hit, 2.5f))
        {
            if (hit.transform.tag == "Player")
            {
                Debug.Log("GOT HIM !!");
            }
        }
    }

    private IEnumerator StartStatusBar()
    {
        _statusBarTime = 10.0f;
        _statusBarCurr = _statusBarMax; // 100
        var statusBarRepeatRate = _statusBarTime / _statusBarMax;

        InvokeRepeating("UpdateStatusBar", 0, statusBarRepeatRate); // ex: 3s -> 0.33

        yield return new WaitForSeconds(_statusBarTime);

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