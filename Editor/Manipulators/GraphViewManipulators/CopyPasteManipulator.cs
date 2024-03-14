using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using Nadsat.DialogueGraph.Editor.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators
{
    public class CopyPasteManipulator : Manipulator
    {
        private const string PasteOperationName = "Paste";
        private const string DuplicateOperationName = "Duplicate";

        private readonly CopyPaste _copyPaste;
        private readonly CopyPasteFactory _copyPasteFactory;

        private UnityEditor.Experimental.GraphView.GraphView _graphView;
        private Vector2 _mousePosition;

        public CopyPasteManipulator(CopyPaste copyPaste, CopyPasteFactory factory)
        {
            _copyPaste = copyPaste;
            _copyPasteFactory = factory;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            _graphView = (UnityEditor.Experimental.GraphView.GraphView) target;
            _graphView.serializeGraphElements += OnCutCopyOperation;
            _graphView.unserializeAndPaste += OnPasteOperation;
            _graphView.RegisterCallback<MouseMoveEvent>(OnMouseMoved);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            _graphView.serializeGraphElements -= OnCutCopyOperation;
            _graphView.unserializeAndPaste -= OnPasteOperation;
            _graphView.UnregisterCallback<MouseMoveEvent>(OnMouseMoved);
        }

        private void OnMouseMoved(MouseMoveEvent evt) =>
            _mousePosition = evt.mousePosition;

        private string OnCutCopyOperation(IEnumerable<GraphElement> elements) =>
            _copyPaste.ToJson(elements);

        private void OnPasteOperation(string operationName, string data)
        {
            _graphView.ClearSelection();
            var elements = CopiedElementsFromJson(data);

            if (operationName == PasteOperationName)
                UpdatePositionForCopiedElements(elements);

            foreach (var graphElement in elements)
                _graphView.AddToSelection(graphElement);
        }

        private void UpdatePositionForCopiedElements(IEnumerable<GraphElement> elements)
        {
            var modelHandlers = elements.OfType<IModelHandle>().ToArray();
            var firstModel = modelHandlers.First();
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(_mousePosition);
            var difference = firstModel.Model.Position - localMousePosition;

            foreach (var handle in modelHandlers)
            {
                var previousPosition = handle.Model.Position;
                var newPosition = previousPosition - difference;
                handle.Model.SetPosition(new Rect(newPosition, Vector2.zero));
            }
        }

        private GraphElement[] CopiedElementsFromJson(string json)
        {
            var copiedData = _copyPaste.FromJson(json);
            return _copyPasteFactory.Create(copiedData).ToArray();
        }
    }
}