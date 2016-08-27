using System;
using UnityEngine;
using System.Collections;
using System.Configuration;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerEditor : Editor
{
    private SceneManager _myScript;

    private bool _setupConfirm;

    public enum InspectorButton
    {
        RecreateDataBase,
        PopulateBehaviours
    }

    private InspectorButton _actionTool;
    private InspectorButton _action
    {
        get { return _actionTool; }
        set
        {
            _actionTool = value;
            _setupConfirm = true;
        }
    }

    public override void OnInspectorGUI()
    {
        _myScript = (SceneManager)target;

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Database");
        GUILayout.Space(5);

        if (GUILayout.Button("Recreate Database"))
            _action = InspectorButton.RecreateDataBase;

        GUILayout.Space(5);

        if (GUILayout.Button("Populate Behaviours"))
            _action = InspectorButton.PopulateBehaviours;

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Required:");
        GUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Player:");
        _myScript.Player = EditorGUILayout.ObjectField(_myScript.Player, typeof(Unit), true) as Unit;

        EditorGUILayout.BeginVertical();

        foreach (var enemy in _myScript.Enemies)
        {
            GUILayout.Label("Enemies:" + enemy.name);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);    // CONFIRM
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        if (_setupConfirm)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Confirm", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                ConfirmAccepted();

            if (GUILayout.Button("Cancel", GUILayout.Width(UiUtils.GetPercent(Screen.width, 25)), GUILayout.Height(50)))
                _setupConfirm = false;

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
        }
    }

    private void ConfirmAccepted()
    {
        switch (_action)
        {
            case InspectorButton.RecreateDataBase:

                _myScript.RecreateDataBase();
                break;

            case InspectorButton.PopulateBehaviours:

                _myScript.PopulateBehaviours();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        _setupConfirm = false;
    }
}