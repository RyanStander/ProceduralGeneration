using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class Circlelizer : EditorWindow
    {
        private Vector3 centerPoint = Vector3.zero;
        private float radius = 5;

        private void OnGUI()
        {
            centerPoint = EditorGUILayout.Vector3Field("Center", centerPoint);
            radius = EditorGUILayout.FloatField("Radius", radius);
            if (GUILayout.Button("Circlelize!")) UpdateLayout();
        }

        private void UpdateLayout()
        {
            var count = Selection.gameObjects.Length;
            var angleStep = 2 * Mathf.PI / count;
            var facing = Quaternion.Euler(new Vector3(90, 0, 0));
            var right = facing * Vector3.right * radius;
            var up = facing * Vector3.up * radius;

            for (var i = 0; i < count; i++)
            {
                var angle = i * angleStep;
                Selection.gameObjects[i].transform.position =
                    centerPoint + Mathf.Cos(angle) * right + Mathf.Sin(angle) * up;
            }
        }

        [MenuItem("Custom menu/Circlelizer")]
        private static void Init()
        {
            var window = GetWindow<Circlelizer>("Circlelizer");
            window.minSize = window.maxSize = new Vector2(300, 200);
            window.Show();
        }
    }
}
