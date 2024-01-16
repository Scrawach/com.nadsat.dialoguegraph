using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Undo.Commands
{
    public class MoveGraphElement : IUndoCommand
    {
        private readonly GraphElement _graphElement;
        private readonly Rect _targetPosition;

        private Rect _previousPosition;

        public MoveGraphElement(GraphElement graphElement, Rect targetPosition)
        {
            _graphElement = graphElement;
            _targetPosition = targetPosition;
        }
        
        public void Undo() =>
            _graphElement.SetPosition(_previousPosition);

        public void Redo()
        {
            _previousPosition = _graphElement.GetPosition();
            _graphElement.SetPosition(_targetPosition);
        }
    }
}