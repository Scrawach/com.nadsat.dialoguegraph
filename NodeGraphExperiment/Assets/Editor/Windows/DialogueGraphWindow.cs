using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphWindow : EditorWindow
    {
        [MenuItem("Dialogue Graph/Open")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent(nameof(DialogueGraphWindow));
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            /*if (Selection.activeObject is DialogueGraph)
            {
                OpenWindow();
                return true;
            }*/
            return false;
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var graph = new DialogueGraph(this);
            graph.StretchToParentSize();
            root.Add(graph);

            graph.DialogueGraphView.graphViewChanged += OnChange;
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            return graphViewChange;
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            // Save data to asset or something else idk
        }
    }
}