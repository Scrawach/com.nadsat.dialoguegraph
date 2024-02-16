using System;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Factories;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class InspectorShowManipulator : Manipulator
    {
        private readonly InspectorViewFactory _inspectorFactory;
        private readonly InspectorView _inspectorView;

        public InspectorShowManipulator(InspectorView inspectorView, InspectorViewFactory inspectorFactory)
        {
            _inspectorView = inspectorView;
            _inspectorFactory = inspectorFactory;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            if (target is not ISelectableNode selectableNode)
                throw new Exception($"{nameof(InspectorShowManipulator)} works only for ISelectableNode nodes!");
            selectableNode.Selected += OnNodeSelected;
            selectableNode.UnSelected += OnNodeUnselected;
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            if (target is not ISelectableNode selectableNode)
                throw new Exception($"{nameof(InspectorShowManipulator)} works only for ISelectableNode nodes!");
            selectableNode.Selected -= OnNodeSelected;
            selectableNode.UnSelected -= OnNodeUnselected;
        }

        private void OnNodeSelected(Node node)
        {
            var inspector = _inspectorFactory.Build(node);
            _inspectorView.Populate(inspector);
        }

        private void OnNodeUnselected(Node node) =>
            _inspectorView.Cleanup();
    }
}