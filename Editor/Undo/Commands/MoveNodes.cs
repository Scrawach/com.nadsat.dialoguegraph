using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Undo.Commands
{
    public class MoveNodes : IUndoCommand
    {
        private readonly List<IMovableNode> _movedElements;
        private readonly List<Rect> _newPositions;
        private readonly List<Rect> _oldPositions;

        public MoveNodes(IEnumerable<IMovableNode> movedElements)
        {
            _movedElements = movedElements.ToList();
            _newPositions = _movedElements.Select(x => x.GetPosition()).ToList();
            _oldPositions = _movedElements.Select(x => x.GetPreviousPosition()).ToList();
        }

        public void Undo()
        {
            for (var i = 0; i < _movedElements.Count; i++)
                _movedElements[i].SavePosition(_oldPositions[i]);
        }

        public void Redo()
        {
            for (var i = 0; i < _movedElements.Count; i++)
                _movedElements[i].SavePosition(_newPositions[i]);
        }

        public override string ToString() =>
            "Move elements";
    }
}