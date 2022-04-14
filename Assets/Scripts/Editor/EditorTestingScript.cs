using UnityEditor;

namespace Editor
{
    public class EditorTestingScript
    {
        [MenuItem("Custom Menu/Show Dialogue")]
        private static void ShowDialogue()
        {
            EditorUtility.DisplayDialog("Mane menu item demo", "It works :)", "Awesome, thanks.");
        }
    }
}
