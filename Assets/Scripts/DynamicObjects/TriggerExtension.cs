using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using System.Collections.Generic;

public class TriggerExtension : MonoBehaviour
{
    private bool _showMesh;
    [HideInInspector]
    public bool ShowMesh
    {
        set
        {
            _showMesh = value;
            if (GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().enabled = _showMesh;
        }
        get { return _showMesh; }
    }

    void Start()
    {
        if (ShowMesh != true)
            ShowMesh = false;
    }

    void OnMouseEnter()
    {
        //Debug.Log("IsMouseOverMap");
        GlobalData.Player.IsMouseOverMap = true;
    }
    void OnMouseExit()
    {
        //Debug.Log("MouseOverMapIs-Not");
        GlobalData.Player.IsMouseOverMap = false;
    }
}