using System;
using Editor.Drawing.Nodes;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphWindow : EditorWindow
    {
        private static DialogueGraph _graph;
        private DialogueGraphRoot _root;
        
        [MenuItem("Dialogue Graph/Open")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent(nameof(DialogueGraphWindow));
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is DialogueGraph graph)
            {
                _graph = graph;
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var graph = new DialogueGraphRoot(this);
            _root = graph;
            graph.StretchToParentSize();
            root.Add(graph);
            graph.DialogueGraphView.Populate(_graph);

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
            _graph.Nodes.Clear();
            foreach (var node in _root.DialogueGraphView.nodes)
            {
                if (node is DialogueNodeView nodeView) 
                    _graph.Nodes.Add(nodeView.Model);
            }
            AssetDatabase.SaveAssetIfDirty(_graph);
        }
    }
}