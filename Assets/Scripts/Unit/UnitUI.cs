using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Assets.Scripts.Utils;

public class UnitUI : MonoBehaviour
{
    Unit Unit;

    public Image StatusBar;

    public float StatusBarTime;
    private readonly float _statusBarMax = 100.0f;
    private float _statusBarCurr;

    public GameObject StatusBarPanel;
    
    internal void Initialize(Unit unit)
    {
        Unit = unit;
        StatusBarPanel.SetActive(false);
    }
    
    public void ChangeState(BehaviourState behaviourState)
    {
        switch (behaviourState)
        {
            case BehaviourState.Suspicious:

                if (StatusBarPanel.activeSelf == false)
                    StatusBarPanel.SetActive(true);

                StatusBar.color = new Color32(251, 225, 76, 255); // yellow
                StartStatusBar("Yellow");
                
                break;
            case BehaviourState.Aggressive:
                StatusBar.color = new Color32(255, 87, 76, 255); // red

                CancelInvoke("UpdateYellowBar");
                StatusBar.fillAmount = 100;
                
                //StartStatusBar("Red");

                break;

            case BehaviourState.Idle:

                if (StatusBarPanel.activeSelf)
                    StatusBarPanel.SetActive(false);

                break;
        }
    }

    public void UpdateYellowBar()
    {
        //Debug.Log(_statusBarCurr);

        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax;

        if (_statusBarCurr < 1)
        {
            CancelInvoke("UpdateYellowBar");

            Unit.UnitInteligence.MainState = MainState.Calm;
            Unit.UnitInteligence.ActionTowardsAlert = ActionTowardsAlert.NoAlert;
        }
    }

    public void UpdateRedBar()
    {
        //Debug.Log(_statusBarCurr);

        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax;

        if (_statusBarCurr < 1)
            CancelInvoke("UpdateRedBar");
    }

    public void StartStatusBar(string color)
    {
        StatusBarTime = 10.0f;
        _statusBarCurr = _statusBarMax; // 100
        var statusBarRepeatRate = StatusBarTime / _statusBarMax;

        InvokeRepeating("Update" + color + "Bar", 0, statusBarRepeatRate); // ex: 3s -> 0.33
    }
}