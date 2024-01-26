using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using UnityEngine.UIElements;

namespace Editor.Factories.NodeListeners
{
    public class NodesProvider : IDialogueNodeListener
    {
        private readonly List<DialogueNodeView> _nodes = new();
        public IReadOnlyList<DialogueNodeView> Nodes => _nodes;

        public DialogueNodeView RootNode;

        public void Register(DialogueNodeView node)
        {
            node.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public void Unregister(DialogueNodeView node)
        {
            node.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public void MarkAsRootNode(DialogueNodeView view)
        {
            RootNode?.MarkAsRoot(false);
            RootNode = view;
            RootNode.MarkAsRoot(true);
        }
        
        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            var view = evt.target as DialogueNodeView;
            _nodes.Add(view);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            var view = evt.target as DialogueNodeView;
            _nodes.Remove(view);
        }

        public void UpdateLanguage()
        {
            foreach (var node in _nodes)
                node.Model.NotifyChanged();
        }

        public DialogueNodeView GetById(string guid) =>
            Nodes.First(node => node.Model.Guid == guid);
    }
}