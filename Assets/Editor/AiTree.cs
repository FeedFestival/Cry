using UnityEngine;
using UnityEditor;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

public class AiTree : EditorWindow
{
    //static AiTree aiTree;

    //[MenuItem("My Functions/AiTree")]
    //static void DisplayAiTree()
    //{
    //    aiTree = GetWindow<AiTree>();
    //    Logic.ReloadNeurons();
    //    Init();
    //}

    //static List<Neuron> _neurons;

    ////List<Rect> windows = new List<Rect>();
    //static List<Neuron> neuronsToConnect;
    //static List<int> attachedNeurons;

    //static float[] levelsYValue;

    //static float viewportWidth;
    //static float viewportHeight;

    //static float neuronWidth = 150;

    //float panX = 0;
    //float panY = 0;

    //static Neuron currentNodeToAttach;
    //static float oldNodePosition;

    //public static bool showSquares;

    //void OnGUI()
    //{
    //    if (neuronsToConnect == null)
    //        Init();

    //    if (viewportWidth == 0)
    //        viewportWidth = position.width;

    //    if (viewportHeight == 0)
    //        viewportHeight = position.height;

    //    if (neuronsToConnect.Count == 2)
    //    {
    //        ConnectNodes();
    //    }

    //    Rect panRect = new Rect(panX, panY, 100000, 100000);

    //    GUI.BeginGroup(panRect);

    //    if (attachedNeurons.Count >= 2)
    //    {
    //        for (int i = 0; i < attachedNeurons.Count; i += 2)
    //        {
    //            DrawNodeCurve(_neurons[attachedNeurons[i]], _neurons[attachedNeurons[i + 1]]);
    //        }
    //    }

    //    BeginWindows();

    //    for (int i = 0; i < _neurons.Count; i++)
    //    {
    //        var title = (showSquares) ? _neurons[i].Id.ToString() : ("" + _neurons[i].Self);

    //        _neurons[i].rect = GUI.Window(i,
    //            _neurons[i].rect,
    //            DrawNeuron,
    //            title
    //            );
    //    }

    //    EndWindows();

    //    GUI.EndGroup();

    //    Event e = Event.current;
    //    if (e.type == EventType.MouseDrag && (e.button == 0 || e.button == 1 || e.button == 2) && panRect.Contains(e.mousePosition))
    //    {
    //        panX += Event.current.delta.x;
    //        panY += Event.current.delta.y;
    //    }

    //    GUILayout.BeginVertical();

    //    GUILayout.BeginHorizontal();

    //    if (GUILayout.Button("-"))
    //    {
    //        showSquares = true;
    //    }

    //    if (GUILayout.Button("+"))
    //    {
    //        showSquares = false;
    //    }

    //    GUILayout.EndHorizontal();

    //    GUILayout.EndVertical();

    //    Repaint();
    //}

    //private static void Init()
    //{
    //    _neurons = Logic.Neurons;

    //    neuronsToConnect = new List<Neuron>();
    //    attachedNeurons = new List<int>();

    //    // setup levels
    //    levelsYValue = new float[Logic.NeuronsLevels];
    //    float y = 50.0f;

    //    for (var i = 0; i < Logic.NeuronsLevels; i++)
    //    {
    //        levelsYValue[i] = y;
    //        y += 170;
    //    }

    //    // arange neurons
    //    int index = 0;
    //    foreach (var neuron in _neurons)
    //    {
    //        if (neuron.ParentId == null && index != 0)
    //        {
    //            Logic.DataService.DeleteNeuron(neuron);
    //            Logic.ReloadNeurons();
    //            Init();
    //            return;
    //        }

    //        Neuron parent = null;

    //        if (neuron.ParentId != null)
    //            parent = _neurons.Where(p => p.Id == neuron.ParentId.Value).FirstOrDefault();

    //        if (neuron.Position != 0)
    //            neuron.rect = new Rect(neuron.Position, levelsYValue[neuron.Level - 1], neuronWidth, 150);
    //        else
    //        {
    //            var position = parent.rect.x + (neuron.Edge ? -150 : 150);
    //            neuron.rect = new Rect(position, levelsYValue[neuron.Level - 1], neuronWidth, 150);
    //        }

    //        if (index == 0)
    //        {
    //            index++;
    //            continue;
    //        }

    //        neuron.attachIndex = index;

    //        // draw connection
    //        if (parent != null)
    //            ConnectNodes(neuron, parent);

    //        index++;
    //    }
    //}

    //void DrawNeuron(int i)
    //{
    //    var neuron = _neurons[i];

    //    if (showSquares)
    //        neuron.rect.Set(neuron.rect.x, neuron.rect.y, 40, 40);
    //    else if (neuron.edit)
    //        neuron.rect.Set(neuron.rect.x, neuron.rect.y, neuron.rect.width, 300);
    //    else
    //        neuron.rect.Set(neuron.rect.x, neuron.rect.y, 150, 150);

    //    if (showSquares)
    //    {

    //    }
    //    else
    //    {
    //        GUILayout.BeginVertical();

    //        if (neuron.Id != 1 && neuron.edit == false)
    //            if (currentNodeToAttach != null && neuron.Id == currentNodeToAttach.Id)
    //            {
    //                if (GUILayout.Button("Cancel"))
    //                {
    //                    currentNodeToAttach = null;
    //                }
    //            }
    //            else if (neuron.ParentId == null)
    //            {
    //                if (GUILayout.Button("Connect"))
    //                {
    //                    currentNodeToAttach = neuron;
    //                }
    //            }
    //            else if (currentNodeToAttach != null)
    //            {
    //                GUILayout.BeginHorizontal();

    //                if (GUILayout.Button("Yay"))
    //                {
    //                    if (CanAddChild(neuron, true) == false)
    //                        return;

    //                    ConnectNeurons(neuron, true);
    //                }

    //                if (GUILayout.Button("Nay"))
    //                {
    //                    if (CanAddChild(neuron, false) == false)
    //                        return;

    //                    ConnectNeurons(neuron, false);
    //                }

    //                GUILayout.EndHorizontal();
    //            }
    //            else
    //            {
    //                if (GUILayout.Button("Disconnect"))
    //                {
    //                    neuron.ParentId = null;
    //                    Logic.DataService.UpdateNeuron(neuron);

    //                    DisconnectNodes(i);
    //                }
    //            }

    //        //--

    //        if (neuron.edit == false)
    //        {
    //            GUILayout.BeginHorizontal();

    //            GUILayout.Label("ID: " + neuron.Id.ToString());
    //            GUILayout.Label("Parent ID: " + neuron.ParentId.ToString());

    //            GUILayout.EndHorizontal();
    //        }
    //        GUILayout.Space(5);

    //        if (neuron.edit == false)
    //        {
    //            GUILayout.BeginHorizontal();

    //            if (GUILayout.Button("Yes"))
    //            {
    //                if (CanAddChild(neuron, true) == false)
    //                    return;

    //                var n = new Neuron
    //                {
    //                    ParentId = neuron.Id,
    //                    Level = neuron.Level + 1,
    //                    Edge = true
    //                };

    //                Logic.DataService.InsertNeuron(n);
    //                Logic.ReloadNeurons();
    //                Init();
    //            }
    //            if (GUILayout.Button("No"))
    //            {
    //                if (CanAddChild(neuron, false) == false)
    //                    return;

    //                var n = new Neuron
    //                {
    //                    ParentId = neuron.Id,
    //                    Level = neuron.Level + 1,
    //                    Edge = false
    //                };

    //                Logic.DataService.InsertNeuron(n);
    //                Logic.ReloadNeurons();
    //                Init();
    //            }

    //            GUILayout.EndHorizontal();
    //        }

    //        if (neuron.edit)
    //        {
    //            GUILayout.Label("Question");
    //            neuron.Self = GUILayout.TextField(neuron.Self == null ? "" : neuron.Self);
    //            GUILayout.Space(5);

    //            //neuron.DoAction = (DoAction)EditorGUILayout.EnumPopup("", neuron.DoAction);

    //            GUILayout.Label("Keywords");
    //            GUILayout.Space(5);
    //            neuron.Keywords = GUILayout.TextField(neuron.Keywords == null ? "" : neuron.Keywords);

    //            GUILayout.Label("Method");
    //            GUILayout.Space(5);
    //            neuron.Method = GUILayout.TextField(neuron.Method == null ? "" : neuron.Method);
    //            GUILayout.Space(20);
    //        }
    //        else
    //        {
    //            if (neuron.Method != null)
    //            {
    //                var style = new GUIStyle();
    //                style.normal.textColor = Color.blue;
    //                GUILayout.Label(neuron.Method.ToString(), style);
    //            }
    //        }

    //        if (neuron.edit == false)
    //            if (GUILayout.Button("Edit"))
    //                neuron.edit = true;

    //        if (neuron.edit)
    //        {
    //            if (GUILayout.Button("Save"))
    //            {
    //                neuron.edit = false;
    //                Logic.DataService.UpdateNeuron(neuron);
    //            }
    //            if (GUILayout.Button("Cancel"))
    //                neuron.edit = false;
    //        }

    //        //----

    //        var dragableArea = new Rect(0, 0, neuron.rect.width, 20);

    //        Event e = Event.current;
    //        if (e.type == EventType.MouseDown && (e.button == 0 || e.button == 1 || e.button == 2) && dragableArea.Contains(e.mousePosition))
    //        {
    //            oldNodePosition = neuron.rect.x;
    //        }

    //        e = Event.current;
    //        if (e.type == EventType.MouseUp && (e.button == 0 || e.button == 1 || e.button == 2) && dragableArea.Contains(e.mousePosition))
    //        {
    //            var diference = neuron.rect.x - oldNodePosition;

    //            neuron.rect.Set(neuron.rect.x, levelsYValue[neuron.Level - 1], neuron.rect.width, neuron.rect.height);
    //            neuron.Position = neuron.rect.x;

    //            List<Neuron> children = Logic.GetAllChildren(neuron);

    //            foreach (var child in children)
    //            {
    //                child.rect.Set(child.rect.x + diference, child.rect.y, child.rect.width, child.rect.height);
    //                child.Position = child.rect.x;
    //            }

    //            children.Add(neuron);
    //            Logic.DataService.UpdateNeurons(children);
    //        }

    //        GUI.DragWindow(dragableArea);

    //        GUILayout.EndVertical();
    //    }
    //}

    //static bool CanAddChild(Neuron neuron, bool value)
    //{
    //    var children = Logic.GetChildren(neuron.Id.Value);
    //    if (children != null)
    //    {
    //        if (children.Count == 2)
    //            return false;
    //        foreach (var child in children)
    //        {
    //            if (child.Edge == value)
    //                return false;
    //        }
    //    }
    //    return true;
    //}

    //static void ConnectNeurons(Neuron neuron, bool edge)
    //{
    //    currentNodeToAttach.ParentId = neuron.Id;
    //    currentNodeToAttach.Level = neuron.Level + 1;
    //    currentNodeToAttach.Edge = edge;

    //    List<Neuron> children = Logic.GetAllChildren(currentNodeToAttach, true);
    //    for (var k = 0; k < children.Count; k++)
    //    {
    //        for (var c = 1; c < children.Count; c++)
    //        {
    //            if (children[k].Id == children[c].ParentId)
    //                children[c].Level = children[k].Level + 1;
    //        }
    //    }
    //    Logic.DataService.UpdateNeurons(children);

    //    currentNodeToAttach = null;

    //    Logic.ReloadNeurons();
    //    Init();
    //}

    //static void ConnectNodes(Neuron child = null, Neuron parent = null)
    //{
    //    attachedNeurons.Add(
    //        child == null ? neuronsToConnect[0].attachIndex : child.attachIndex
    //        );
    //    attachedNeurons.Add(
    //        parent == null ? neuronsToConnect[1].attachIndex : parent.attachIndex
    //        );
    //    neuronsToConnect = new List<Neuron>();
    //}

    //private void DisconnectNodes(int child)
    //{
    //    var parentIndex = attachedNeurons.IndexOf(child);

    //    attachedNeurons.Remove(child);
    //    attachedNeurons.RemoveAt(parentIndex);
    //}

    //void DrawNodeCurve(Neuron child, Neuron parent)
    //{
    //    Vector2 parentStart = new Vector2(0.5f, 0.0f);
    //    Vector2 childStart = new Vector2(0.5f, 0.5f);
    //    Color color;
    //    if (child.Edge)
    //    {
    //        parentStart = new Vector2(0f, 0.5f);
    //        color = Color.green;
    //    }
    //    else
    //    {
    //        parentStart = new Vector2(1f, 0.5f);
    //        color = Color.red;
    //    }

    //    DrawNodeCurve(parent.rect, child.rect, parentStart, childStart, color);
    //}

    //void DrawNodeCurve(Rect start, Rect end, Vector2 vStartPercentage, Vector2 vEndPercentage, Color color)
    //{
    //    Vector3 startPos = new Vector3(start.x + start.width * vStartPercentage.x, start.y + start.height * vStartPercentage.y, 0);
    //    Vector3 endPos = new Vector3(end.x + end.width * vEndPercentage.x, end.y + end.height * vEndPercentage.y, 0);
    //    Vector3 startTan = startPos + Vector3.right * (-50 + 100 * vStartPercentage.x) + Vector3.up * (-50 + 100 * vStartPercentage.y);
    //    Vector3 endTan = endPos + Vector3.right * (-50 + 100 * vEndPercentage.x) + Vector3.up * (-50 + 100 * vEndPercentage.y);
    //    Color shadowCol = new Color(0, 0, 0, 0.06f);
    //    for (int i = 0; i < 3; i++) // Draw a shadow
    //        Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
    //    Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 2);
    //}
}