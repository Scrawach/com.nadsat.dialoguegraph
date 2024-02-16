using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class RemoveNodes : IUndoCommand
    {
        private readonly GraphView _canvas;
        private readonly IEnumerable<Node> _nodesToRemove;

        public RemoveNodes(GraphView canvas, IEnumerable<Node> nodesToRemove)
        {
            _canvas = canvas;
            _nodesToRemove = nodesToRemove;
        }

        public void Undo()
        {
            foreach (var node in _nodesToRemove)
                _canvas.AddElement(node);
        }

        public void Redo()
        {
            foreach (var node in _nodesToRemove)
                _canvas.RemoveElement(node);
        }
    }
}