using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Undo.Commands
{
    public class AddElements : IUndoCommand
    {
        private readonly RemoveElements _removeElements;

        public AddElements(GraphView canvas, List<GraphElement> addedElements) =>
            _removeElements = new RemoveElements(canvas, addedElements);

        public void Undo() =>
            _removeElements.Redo();

        public void Redo() =>
            _removeElements.Undo();
    }
}