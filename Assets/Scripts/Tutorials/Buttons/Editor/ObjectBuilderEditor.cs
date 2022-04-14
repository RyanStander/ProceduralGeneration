using UnityEditor;
using UnityEngine;

namespace Tutorials.Buttons.Editor
{
    [CustomEditor(typeof(ObjectBuilderScript))]
    public class ObjectBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (ObjectBuilderScript)target;
            if(GUILayout.Button("Build Object"))
            {
                myScript.BuildObject();
            }
        }
    }
}