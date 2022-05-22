using ModularMeshes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(HouseGenerator))]
    public class HouseGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var houseGenerator = (HouseGenerator) target;
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
