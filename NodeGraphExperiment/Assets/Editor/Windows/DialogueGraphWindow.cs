using Editor.Drawing;
using Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphWindow : EditorWindow
    {
        private DialogueGraphRoot _graphRoot;
        private DialogueGraphView _graphView;

        public bool IsDirty
        {
            get => hasUnsavedChanges;
            set => hasUnsavedChanges = value;
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            _graphRoot = new DialogueGraphRoot(this);
            _graphView = _graphRoot.DialogueGraphView;
            _graphRoot.StretchToParentSize();
            root.Add(_graphRoot);

            _graphView.graphViewChanged += OnChange;
        }

        public void Populate(DialogueGraphContainer graph) =>
            _graphRoot.Load(graph);

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            return graphViewChange;
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            _graphRoot.Save();
        }
    }
}