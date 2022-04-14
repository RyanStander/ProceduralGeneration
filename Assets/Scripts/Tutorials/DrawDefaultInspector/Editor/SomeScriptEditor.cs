using UnityEditor;
using UnityEngine;

namespace Tutorials.DrawDefaultInspector.Editor
{
    [CustomEditor(typeof(SomeScript))]
    public class SomeScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.HelpBox("This is a help box",MessageType.Info);
        }
    }
}
