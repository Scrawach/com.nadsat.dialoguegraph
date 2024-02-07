using Editor.Windows;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
{
    public static class DialogueGraphOpen
    {
        [MenuItem("Dialogue Graph/Open")]
        public static void OpenWindow() =>
            OpenDialogueGraphWindow();

        public static void OpenWindow(DialogueGraphContainer graph) =>
            OpenDialogueGraphWindow().Populate(graph);

        private static DialogueGraphWindow OpenDialogueGraphWindow() =>
            EditorWindow.GetWindow<DialogueGraphWindow>(nameof(DialogueGraphWindow));

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