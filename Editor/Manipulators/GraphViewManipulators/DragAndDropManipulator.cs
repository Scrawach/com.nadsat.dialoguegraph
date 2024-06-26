using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Factories;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators
{
    public class DragAndDropManipulator : Manipulator
    {
        private const string DragSelection = "DragSelection";
        private readonly INodeViewFactory _factory;

        public DragAndDropManipulator(INodeViewFactory factory) =>
            _factory = factory;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.UnregisterCallback<DragPerformEvent>(OnDragPerformEvent);
        }

        private void OnDragPerformEvent(DragPerformEvent evt)
        {
            var graphView = (UnityEditor.Experimental.GraphView.GraphView) target;

            var selection = DragAndDrop.GetGenericData(DragSelection) as List<ISelectable>;
            var fields = selection.OfType<BlackboardField>();
            foreach (var field in fields)
                _factory.CreateVariable(new VariableNode
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = field.text,
                    Position = graphView.contentViewContainer.WorldToLocal(new Rect(evt.mousePosition, Vector2.zero))
                });
        }

        private void OnDragUpdated(DragUpdatedEvent e)
        {
            if (DragAndDrop.GetGenericData(DragSelection) is List<ISelectable> selection && selection.OfType<BlackboardField>().Count() >= 0)
                DragAndDrop.visualMode = e.actionKey ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Move;
        }
    }
}