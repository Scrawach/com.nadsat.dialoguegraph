using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class CreateEdges : IUndoCommand
    {
        private readonly RemoveEdges _removeEdges;
        
        public CreateEdges(GraphView canvas, List<Edge> edgesToCreate) =>
            _removeEdges = new RemoveEdges(canvas, edgesToCreate);

        public void Undo() =>
            _removeEdges.Redo();

        public void Redo() =>
            _removeEdges.Undo();
    }
}