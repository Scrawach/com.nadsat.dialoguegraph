using System;
using Editor.Drawing.Nodes;

namespace Editor.Factories.NodeListeners
{
    public class NodeViewListener : IDialogueNodeListener
    {
        public event Action<DialogueNodeView> Selected;
        public event Action<DialogueNodeView> Unselected;
        
        public void Register(DialogueNodeView view)
        {
            view.Selected += OnNodeSelected;
            view.Unselected += OnNodeUnselected;
        }

        public void Unregister(DialogueNodeView view)
        {
            view.Selected -= OnNodeSelected;
            view.Unselected -= OnNodeUnselected;
        }

        private void OnNodeSelected(DialogueNodeView view) =>
            Selected?.Invoke(view);

        private void OnNodeUnselected(DialogueNodeView view) =>
            Unselected?.Invoke(view);
    }
}