using BuildingGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BuildingGenerator))]
    public class BuildingGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var houseGenerator = (BuildingGenerator) target;
            if (GUILayout.Button("Build House"))
            {
                houseGenerator.Generate();
            }

            if (GUILayout.Button("Clear House"))
            {
                houseGenerator.ClearBuilding();
            }
        }
    }
}