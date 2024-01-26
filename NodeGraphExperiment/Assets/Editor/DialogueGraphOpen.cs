using Editor.Windows;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public static class DialogueGraphOpen
    {
        [MenuItem("Dialogue Graph/Open")]
        public static void OpenWindow() =>
            OpenDialogueGraphWindow();

        public static void OpenWindow(DialogueGraphContainer graph)
        {
            var window = OpenDialogueGraphWindow();
            window.Populate(graph);
        }

        private static DialogueGraphWindow OpenDialogueGraphWindow()
        {
            var window = EditorWindow.GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent(nameof(DialogueGraphWindow));
            return window;
        }

        [OnOpenAsset]
        public static bool OnOpenDialogueGraph(int instanceId, int line)
        {
            if (Selection.activeObject is not DialogueGraphContainer graph) 
                return false;
            
            OpenWindow(graph);
            return true;
        }
    }
}