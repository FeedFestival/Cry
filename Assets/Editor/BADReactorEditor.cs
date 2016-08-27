using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BAD
{
    [CustomEditor(typeof(AiReactor))]
    public class BADReactorEditor : Editor
    {
        static AiReactor activeReactor;
        private static BADViewerWindow window;
        public override void OnInspectorGUI()
        {
            if (activeReactor == null)
                activeReactor = target as AiReactor;
            if (window == null)
                window = (BADViewerWindow)EditorWindow.GetWindow(typeof(BADViewerWindow));

            activeReactor.Debug = true;
            window.reactor = activeReactor;
            window.Show();
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(activeReactor.Pause ? "Resume" : "Pause", GUILayout.MinWidth(64)))
                {
                    activeReactor.Pause = !activeReactor.Pause;
                }
                if (GUILayout.Button("Step", GUILayout.MinWidth(64)))
                {
                    activeReactor.Step = true;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                foreach (var i in activeReactor.Blackboard.Items)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(i.Key);
                    GUILayout.Label(i.Value.ToString(), "box", GUILayout.Width(128));
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}