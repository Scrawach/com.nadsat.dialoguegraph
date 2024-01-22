using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class RemoveElements : IUndoCommand
    {
        private readonly List<IUndoCommand> _removeCommands;

        public RemoveElements(GraphView canvas, List<GraphElement> elementsToRemove) =>
            _removeCommands = new List<IUndoCommand>()
            {
                new RemoveEdges(canvas, elementsToRemove.OfType<Edge>()),
                new RemoveNodes(canvas, elementsToRemove.OfType<Node>())
            };

        public void Undo()
        {
            foreach (var command in _removeCommands) 
                command.Undo();
        }

        public void Redo()
        {
            foreach (var command in _removeCommands) 
                command.Redo();
        }
    }
}