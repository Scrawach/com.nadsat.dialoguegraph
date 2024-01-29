using System;
using Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Factories.NodeListeners
{
    public class NodeViewListener : IDialogueNodeListener
    {
        public event Action<Node> Selected;
        public event Action<Node> Unselected;
        
        public void Register(Node view)
        {
            if (view is not ISelectableNode selectableNode)
                return;
            
            selectableNode.Selected += OnNodeSelected;
            selectableNode.UnSelected += OnNodeUnselected;
        }

        public void Unregister(Node view)
        {
            if (view is not ISelectableNode selectableNode)
                return;
            
            selectableNode.Selected -= OnNodeSelected;
            selectableNode.UnSelected -= OnNodeUnselected;
        }

        private void OnNodeSelected(Node view) =>
            Selected?.Invoke(view);

        private void OnNodeUnselected(Node view) =>
            Unselected?.Invoke(view);
    }
}