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
        private DialogueGraphRoot _graphRoot;
        private DialogueGraphView _graphView;

        public void Populate(DialogueGraphContainer graph) =>
            _graphRoot.Populate(graph);

        public void CreateGUI()
        {
            var root = rootVisualElement;
            _graphRoot = new DialogueGraphRoot(this);
            _graphView = _graphRoot.DialogueGraphView;
            _graphRoot.StretchToParentSize();
            root.Add(_graphRoot);

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