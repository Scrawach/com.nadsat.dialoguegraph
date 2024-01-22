using Editor.Drawing.Nodes;
using Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphWindow : EditorWindow
    {
        private DialogueGraphView _graphView;
        private DialogueGraph _graphData;
        
        public void CreateGUI()
        {
            var root = rootVisualElement;
            var graph = new DialogueGraphRoot(this);
            _graphView = graph.DialogueGraphView;
            graph.StretchToParentSize();
            root.Add(graph);

            _graphView.graphViewChanged += OnChange;
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            return graphViewChange;
        }
        
        public override void SaveChanges()
        {
            Debug.Log("SAVE CHANGES!");
            base.SaveChanges();
            return;
            _graphData.Nodes.Clear();
            foreach (var node in _graphView.nodes)
            {
                if (node is DialogueNodeView nodeView) 
                    _graphData.Nodes.Add(nodeView.Model);
            }
            AssetDatabase.SaveAssetIfDirty(_graphData);
        }

        public void Populate(DialogueGraph graph)
        {
            _graphData = graph;
            _graphView.Populate(graph);
        }
    }
}