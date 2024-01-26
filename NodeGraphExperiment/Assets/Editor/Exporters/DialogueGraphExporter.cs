using System.Collections.Generic;
using System.Linq;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Runtime;
using Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Exporters
{
    public class DialogueGraphExporter
    {
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;
        private readonly DialogueGraphContainer _graph;

        public DialogueGraphExporter(DialogueGraphView graphView, NodesProvider nodes, DialogueGraphContainer graph)
        {
            _graphView = graphView;
            _nodes = nodes;
            _graph = graph;
        }

        public void Export()
        {
            _graph.Graph.Nodes = _nodes.Nodes.Select(node => node.Model).ToList();
            _graph.Graph.Links = GetLinksFrom(_graphView.edges).ToList();
            _graph.Graph.RedirectNodes = GetRedirectNodesFrom(_graphView.nodes).ToList();

            if (_nodes.RootNode == null)
                _nodes.RootNode = _nodes.Nodes.First();
            
            _graph.Graph.EntryNodeGuid = _nodes.RootNode.Model.Guid;
            
            AssetDatabase.SaveAssetIfDirty(_graph);
            EditorGUIUtility.PingObject(_graph);
        }

        private static IEnumerable<RedirectNode> GetRedirectNodesFrom(UQueryState<Node> nodes) =>
            nodes
                .Where(n => n is RedirectNodeView)
                .Select(node => ((RedirectNodeView) node).Model);

        private static IEnumerable<NodeLinks> GetLinksFrom(UQueryState<Edge> edges)
        {
            foreach (var edge in edges.Where(e => e.input.node != null))
            {
                var parentNode = (dynamic) edge.output.node;
                var childNode = (dynamic) edge.input.node;

                yield return new NodeLinks()
                {
                    FromGuid = parentNode!.Model.Guid,
                    ToGuid = childNode!.Model.Guid
                };
            }
        }
    }
}