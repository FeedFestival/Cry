using UnityEngine;
using UnityEditor;
using Assets.Scripts.Utils;
using BAD;

namespace Assets.Editor
{
    [CustomEditor(typeof(Intel))]
    public class IntelEditor : UnityEditor.Editor
    {
        private static Intel _intel;
        private static BADViewerWindow window;

        public override void OnInspectorGUI()
        {
            _intel = (Intel)target;

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Watch this", GUILayout.MinWidth(64)))
                {
                    if (window == null)
                        window = (BADViewerWindow)EditorWindow.GetWindow(typeof(BADViewerWindow));
                    window.GuardNeuron = _intel.Guard.GuardNeuron;
                    window.Show();
                    base.OnInspectorGUI();
                }
            }
        }
    }
}
