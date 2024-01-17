using System.Collections.Generic;
using Editor.Drawing.Nodes;
using UnityEngine.UIElements;

namespace Editor.Data
{
    public class NodesProvider
    {
        private readonly List<DialogueNodeView> _nodes = new();
        public IReadOnlyList<DialogueNodeView> Nodes => _nodes;

        public void Register(DialogueNodeView node)
        {
            _nodes.Add(node);
            node.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public void Unregister(DialogueNodeView node)
        {
            _nodes.Remove(node);
            node.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
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
                node.Model.PersonId = node.Model.PersonId;
        }
    }
}