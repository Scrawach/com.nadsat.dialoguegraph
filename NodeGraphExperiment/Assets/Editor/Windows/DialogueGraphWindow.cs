using Editor.Drawing;
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

        public void Populate(DialogueGraphContainer graph) =>
            _graphView.Populate(graph);

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var graph = new DialogueGraphRoot(this);
            _graphView = graph.DialogueGraphView;
            graph.StretchToParentSize();
            root.Add(graph);

            _graphView.graphViewChanged += OnChange;
            _graphView.Saved += OnSaved;
        }

        private void OnSaved() =>
            hasUnsavedChanges = false;

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            return graphViewChange;
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            _graphView.Save();
        }
    }
}