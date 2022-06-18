using TownGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TownGenerator))]
    public class TownGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var townGenerator = (TownGenerator) target;
            if (GUILayout.Button("Build Town"))
            {
                townGenerator.CreateTown();
            }

            if (GUILayout.Button("Clear Town"))
            {
                townGenerator.ResetTown();
            }
            
            if (GUILayout.Button("Press if clear town isn't clearing"))
            {
                townGenerator.ResetTown();
                townGenerator.ClearObjects();
            }
        }
    }
}
