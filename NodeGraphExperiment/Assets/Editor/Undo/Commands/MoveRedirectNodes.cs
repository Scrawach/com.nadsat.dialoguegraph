using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using UnityEngine;

namespace Editor.Undo.Commands
{
    public class MoveRedirectNodes : IUndoCommand
    {
        private readonly List<RedirectNodeView> _movedElements;
        private readonly List<Rect>_newPositions;
        private readonly List<Rect> _oldPositions;

        public MoveRedirectNodes(IEnumerable<RedirectNodeView> movedElements)
        {
            _movedElements = movedElements.ToList();
            _newPositions = _movedElements.Select(x => x.GetPosition()).ToList();
            _oldPositions = _movedElements.Select(x => x.Model.Position).ToList();
        }

        public void Undo()
        {
            for (var i = 0; i < _movedElements.Count; i++) 
                _movedElements[i].Model.SetPosition(_oldPositions[i]);
        }

        public void Redo()
        {
            for (var i = 0; i < _movedElements.Count; i++) 
                _movedElements[i].Model.SetPosition(_newPositions[i]);
        }

        public override string ToString() =>
            "Move elements";
    }
}