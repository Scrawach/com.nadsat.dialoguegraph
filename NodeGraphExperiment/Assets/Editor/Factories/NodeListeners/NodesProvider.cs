using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Factories.NodeListeners
{
    public class NodesProvider : IDialogueNodeListener
    {
        private readonly List<IModelHandle> _nodes = new();
        public IReadOnlyList<IModelHandle> Nodes => _nodes;

        public IModelHandle RootNode;

        public void Register(Node node)
        {
            node.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public void Unregister(Node node)
        {
            node.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            node.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public void MarkAsRootNode(IModelHandle view)
        {
            RootNode?.MarkAsRoot(false);
            RootNode = view;
            RootNode.MarkAsRoot(true);
        }
        
        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            var view = evt.target as IModelHandle;
            _nodes.Add(view);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            var view = evt.target as IModelHandle;
            _nodes.Remove(view);
        }

        public void UpdateLanguage()
        {
            foreach (var node in _nodes)
                node.Model.NotifyChanged();
        }

        public IModelHandle GetById(string guid) =>
            Nodes.First(node => node.Model.Guid == guid);
    }
}