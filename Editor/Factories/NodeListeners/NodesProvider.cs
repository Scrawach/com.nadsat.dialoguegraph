using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Factories.NodeListeners
{
    public class NodesProvider
    {
        private readonly GraphView _graphView;


        public IModelHandle RootNode;

        public NodesProvider(GraphView graphView) =>
            _graphView = graphView;

        public IEnumerable<IModelHandle> Nodes => _graphView.nodes.OfType<IModelHandle>();

        public void MarkAsRootNode(IModelHandle view)
        {
            RootNode?.MarkAsRoot(false);
            RootNode = view;
            RootNode.MarkAsRoot(true);
        }

        public void UpdateLanguage()
        {
            foreach (var node in Nodes)
                node.Model.NotifyChanged();
        }

        public IModelHandle GetById(string guid) =>
            Nodes.First(node => node.Model.Guid == guid);
    }
}