using Runtime;
using Runtime.Nodes;
using UnityEngine;

namespace Editor.Undo.Commands
{
    public class MoveNode : IUndoCommand
    {
        private readonly DialogueNode _node;
        private readonly Rect _target;

        private readonly Rect _startPosition;
        
        public MoveNode(DialogueNode node, Rect target)
        {
            _node = node;
            _target = target;
            _startPosition = _node.Position;
            Debug.Log($"target: {_target}, start: {_startPosition}");
        }
        
        public void Undo() =>
            _node.SetPosition(_startPosition);

        public void Redo() =>
            _node.SetPosition(_target);

        public override string ToString() =>
            $"{_node.PersonId} to {_target}";
    }
}