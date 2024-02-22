using Editor.Drawing.Nodes;
using Editor.Windows;
using Runtime.Nodes;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class NodeChangesDirtyMarkManipulator : Manipulator
    {
        private readonly DialogueGraphWindow _root;
        private readonly BaseDialogueNode _node;

        public NodeChangesDirtyMarkManipulator(DialogueGraphWindow root, BaseDialogueNode node)
        {
            _root = root;
            _node = node;
        }

        protected override void RegisterCallbacksOnTarget() => 
            _node.Changed += OnModelChanged;
        
        protected override void UnregisterCallbacksFromTarget() => 
            _node.Changed -= OnModelChanged;
        
        private void OnModelChanged() => 
            _root.IsDirty = true;
    }
}