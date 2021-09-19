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

    public GameObject UnitStats;
    public GameObject StatusBarPanel;
    public AlertLevel PreviousAlertLevel;

    internal void Initialize(Unit unit)
    {
        Unit = unit;
        StatusBarPanel.SetActive(false);
    }

    public void ChangeState(AlertLevel alertLevel)
    {
        switch (alertLevel)
        {
            case AlertLevel.Suspicious:

                if (StatusBarPanel.activeSelf == false)
                    StatusBarPanel.SetActive(true);

                if (PreviousAlertLevel == AlertLevel.None || PreviousAlertLevel == AlertLevel.Talkative)
                {
                    StatusBar.color = new Color32(251, 225, 76, 255); // yellow
                    CancelInvoke("UpdateRedBar");
                    StatusBar.fillAmount = 100;
                }
                else if (PreviousAlertLevel == AlertLevel.Aggressive)
                {
                    StatusBar.color = new Color32(255, 87, 76, 255); // red
                    CancelInvoke("UpdateYellowBar");
                    StatusBar.fillAmount = 100;
                    StartStatusBar("Red");
                }
                break;
            case AlertLevel.Talkative:

                if (PreviousAlertLevel == AlertLevel.Suspicious)
                {
                    StatusBar.color = new Color32(255, 132, 53, 255); // orange
                    CancelInvoke("UpdateYellowBar");
                    CancelInvoke("UpdateRedBar");
                    StatusBar.fillAmount = 100;
                }
                else if (PreviousAlertLevel == AlertLevel.Aggressive)
                {
                    StatusBar.color = new Color32(255, 87, 76, 255); // red
                    CancelInvoke("UpdateYellowBar");
                    StatusBar.fillAmount = 100;
                    StartStatusBar("Red");
                }

                break;
            case AlertLevel.Aggressive:

                StatusBar.color = new Color32(255, 87, 76, 255); // red
                CancelInvoke("UpdateYellowBar");
                CancelInvoke("UpdateRedBar");
                StatusBar.fillAmount = 100;
                break;

            case AlertLevel.None:

                if (PreviousAlertLevel == AlertLevel.Suspicious)
                {
                    StatusBar.color = new Color32(251, 225, 76, 255); // yellow
                    StartStatusBar("Yellow");
                }
                //if (StatusBarPanel.activeSelf)
                //    StatusBarPanel.SetActive(false);

                break;
        }
    }

    // this represents the AI going from [Investigative/Alerted] to Calm
    public void UpdateYellowBar()
    {
        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax;

        if (_statusBarCurr < 1)
        {
            CancelInvoke("UpdateYellowBar");

            Unit.UnitInteligence.Intel.CompleteNeuronBranch();
            Unit.UnitInteligence.MainState = MainState.Calm;
        }
    }

    // this represents the AI going from [Agressive] to Investigative/Alerted
    public void UpdateRedBar()
    {
        _statusBarCurr -= 1.0f;
        StatusBar.fillAmount = _statusBarCurr / _statusBarMax;

        //-- END
        if (_statusBarCurr < 1)
        {
            Unit.UnitInteligence.AlertLevel = AlertLevel.Suspicious;

            CancelInvoke("UpdateRedBar");
        }
    }

    public void StartStatusBar(string color)
    {
        StatusBarTime = 10.0f;
        _statusBarCurr = _statusBarMax; // 100
        var statusBarRepeatRate = StatusBarTime / _statusBarMax;

        InvokeRepeating("Update" + color + "Bar", 0, statusBarRepeatRate); // ex: 3s -> 0.33
    }

    private bool CheckRestPoint()
    {
        if (Vector3.Angle(GlobalData.CameraControl.transform.position - transform.position, transform.forward) >
                2f)
        {
            return true;
        }
        return false;
    }
    private void LookAtCamera()
    {
        if (CheckRestPoint())
            UnitStats.transform.rotation =
                Logic.SmoothLook(UnitStats.transform.rotation,
                    Logic.GetDirection(UnitStats.transform.position,
                       GlobalData.CameraControl.transform.position), 4f);
    }
    void Update()
    {
        LookAtCamera();
    }
}