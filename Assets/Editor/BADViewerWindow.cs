using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEditor;

namespace BAD
{
    public class BADViewerWindow : EditorWindow
    {
        static BADViewerWindow window;
        public AiReactor reactor;
        Vector2 scrollPosition;
        Rect cursor;
        List<Connector> connectors = new List<Connector>();

        float panX = 0;
        float panY = 0;

        public Neuron GuardNeuron;

        void DrawNeuron(Neuron neuron, string decorator = "")
        {
            GUIStyle style = new GUIStyle("button");
            style.normal.textColor = Color.black;

            if (neuron.NeuronType == NeuronType.Sequence)
            {
                GUI.color = neuron.NeuronState == NeuronState.Running ? Color.yellow : Color.grey;
                style.normal.textColor = Color.red;
            }
            else if (neuron.NeuronType == NeuronType.Selector)
                GUI.color = neuron.NeuronState == NeuronState.Running ? Color.yellow : Color.magenta;
            else if (neuron.NeuronType.IsIn(NeuronType.If, NeuronType.IfElse))
                GUI.color = neuron.NeuronState == NeuronState.Running ? Color.yellow : Color.cyan;
            else
                GUI.color = neuron.NeuronState == NeuronState.Running ? Color.yellow : Color.white;

            if (neuron.NeuronResult != NeuronResult.WaitFor)
            {
                var icon = cursor;
                icon.x -= 14;
                icon.width = 14;
                var color = GUI.color;
                GUI.color = neuron.NeuronResult == NeuronResult.Success
                    ? Color.green
                    : neuron.NeuronResult == NeuronResult.Continue ? Color.yellow : Color.red;
                GUI.Label(icon, "", EditorStyles.radioButton);
                GUI.color = color;
            }
            var text = string.Format(decorator + "{0}", neuron);
            cursor.width = GUI.skin.button.CalcSize(new GUIContent(text)).x;

            GUI.Label(cursor, text, style);
        }

        void DrawNeuronGraph(Neuron neuron, Vector2 parentPosition, string decorator = "")
        {
            var midY = Mathf.Lerp(cursor.yMax, cursor.yMin, 0.5f);
            var x = parentPosition.x;
            var A = new Vector2(cursor.xMin, midY);
            var B = new Vector2(x, midY);
            var C = parentPosition;

            if (neuron.NeuronState == NeuronState.Running)
            {
                connectors.Add(new Connector(A, B, C));
            }
            else
            {
                Handles.color = Color.white;
                Handles.DrawPolyLine(A, B, C);
            }

            if (neuron.NeuronType.IsIn(NeuronType.If, NeuronType.IfElse))
            {
                DrawNeuron(neuron, decorator);
                parentPosition = new Vector2(cursor.xMax, Mathf.Lerp(cursor.yMin, cursor.yMax, 0.5f));
                var indent = cursor.width + 16;
                cursor.x += indent;

                var count = 1;
                foreach (var child in neuron.Children)
                {
                    // if this is a the false definition
                    if (count == 2)
                        DrawNeuronGraph(child, parentPosition, "Else ");
                    else
                        DrawNeuronGraph(child, parentPosition);
                    count++;
                }
                cursor.x -= indent;
            }
            else if (neuron.NeuronType.IsIn(NeuronType.Selector, NeuronType.Sequence, NeuronType.Root))
            {
                DrawNeuron(neuron, decorator);
                parentPosition = new Vector2(Mathf.Lerp(cursor.xMin, cursor.xMax, 0.25f), cursor.yMax);
                var indent = (cursor.width / 4) + 16;
                cursor.x += indent;
                cursor.y += cursor.height + 4;
                foreach (var child in neuron.Children)
                {
                    DrawNeuronGraph(child, parentPosition);
                }
                cursor.x -= indent;
            }
            else
            {
                DrawNeuron(neuron, decorator);
                cursor.y += cursor.height + 4;
            }
        }

        void OnDrawGUI()
        {
            cursor = new Rect(20, 20, 160, 18);
            connectors.Clear();

            if (GuardNeuron != null)
            {
                DrawNeuronGraph(GuardNeuron, new Vector2(20, 20));
            }

            foreach (var c in connectors)
            {
                Handles.color = Color.green;
                Handles.DrawPolyLine(c.A, c.B, c.C);
            }
        }

        void OnGUI()
        {
            Rect panRect = new Rect(panX, panY, 100000, 100000);

            GUI.BeginGroup(panRect);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!Application.isPlaying)
                if (GUILayout.Button("Show Guard"))
                    GuardNeuron = MindMap.GuardNeuron;
            GUILayout.EndHorizontal();

            // - Main DRAW
            OnDrawGUI();

            GUI.EndGroup();

            Event e = Event.current;
            if (e.type == EventType.MouseDrag && (e.button == 0 || e.button == 1 || e.button == 2) && panRect.Contains(e.mousePosition))
            {
                panX += Event.current.delta.x;
                panY += Event.current.delta.y;
            }

            Repaint();
        }

        void Update()
        {
            if (reactor != null)
            {
                Repaint();
            }
        }

        class Connector
        {
            public Vector2 A, B, C;

            public Connector(Vector2 A, Vector2 B, Vector2 C)
            {
                this.A = A;
                this.B = B;
                this.C = C;
            }
        }
    }
}