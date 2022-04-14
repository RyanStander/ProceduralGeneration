using UnityEditor;

namespace Tutorials.LevelScript.Editor
{
    [CustomEditor(typeof(LevelScript))]
    public class LevelScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var myLevelScrip = (LevelScript)target;

            myLevelScrip.experience=EditorGUILayout.IntField("Experience", myLevelScrip.experience);
            EditorGUILayout.LabelField("Level",myLevelScrip.Level().ToString());
        }
    }
}
