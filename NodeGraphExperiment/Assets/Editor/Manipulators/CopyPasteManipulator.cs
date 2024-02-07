using System.Collections.Generic;
using Editor.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class CopyPasteManipulator : Manipulator
    {
        private readonly CopyPaste _copyPaste;
        private readonly CopyPasteFactory _copyPasteFactory;

        private GraphView _graphView;

        public CopyPasteManipulator(CopyPaste copyPaste, CopyPasteFactory factory)
        {
            _copyPaste = copyPaste;
            _copyPasteFactory = factory;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            _graphView = (GraphView) target;
            _graphView.serializeGraphElements += OnCutCopyOperation;
            _graphView.unserializeAndPaste += OnPasteOperation;
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            _graphView.serializeGraphElements -= OnCutCopyOperation;
            _graphView.unserializeAndPaste -= OnPasteOperation;
        }

        private string OnCutCopyOperation(IEnumerable<GraphElement> elements) =>
            _copyPaste.ToJson(elements);

        private void OnPasteOperation(string operationName, string data)
        {
            _graphView.ClearSelection();
            var copiedData = _copyPaste.FromJson(data);
            foreach (var graphElement in _copyPasteFactory.Create(copiedData)) 
                _graphView.AddToSelection(graphElement);
        }
    }
}