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
    
    public void ChangeState(BehaviourState neuronState)
    {
        switch (neuronState)
        {
            case BehaviourState.Suspicious:

                if (StatusBarPanel.activeSelf == false)
                    StatusBarPanel.SetActive(true);

                StatusBar.color = new Color32(251, 225, 76, 255); // yellow

                StartStatusBar();
                
                break;
            case BehaviourState.Agressive:
                StatusBar.color = new Color32(255, 87, 76, 255); // red

                CancelInvoke("UpdateStatusBar");
                StartStatusBar();
                
                break;

            case BehaviourState.Idle:

                if (StatusBarPanel.activeSelf)
                    StatusBarPanel.SetActive(false);

                break;
        }
    }

    public void UpdateStatusBar()
    {
        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax; //70 / 100 = 0.7 din bara
    }

    public void StartStatusBar()
    {
        StatusBarTime = 10.0f;
        _statusBarCurr = _statusBarMax; // 100
        var statusBarRepeatRate = StatusBarTime / _statusBarMax;

        InvokeRepeating("UpdateStatusBar", 0, statusBarRepeatRate); // ex: 3s -> 0.33
    }
}