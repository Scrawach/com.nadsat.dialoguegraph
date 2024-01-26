using System.Collections.Generic;
using System.Linq;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Exporters
{
    public class DialogueGraphExporter
    {
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;
        private readonly DialogueGraph _graph;

        public DialogueGraphExporter(DialogueGraphView graphView, NodesProvider nodes, DialogueGraph graph)
        {
            _graphView = graphView;
            _nodes = nodes;
            _graph = graph;
        }

        public void Export()
        {
            _graph.Nodes = _nodes.Nodes.Select(node => node.Model).ToList();
            _graph.Links = GetLinksFrom(_graphView.edges).ToList();

            if (_nodes.RootNode == null)
                _nodes.RootNode = _nodes.Nodes.First();
            
            _graph.EntryNodeGuid = _nodes.RootNode.Model.Guid;
            
            AssetDatabase.SaveAssetIfDirty(_graph);
            EditorGUIUtility.PingObject(_graph);
        }

        private static IEnumerable<NodeLinks> GetLinksFrom(UQueryState<Edge> edges)
        {
            foreach (var edge in edges.Where(e => e.input.node != null))
            {
                var parentNode = edge.output.node as DialogueNodeView;
                var childNode = edge.input.node as DialogueNodeView;

                yield return new NodeLinks()
                {
                    FromGuid = parentNode!.Model.Guid,
                    ToGuid = childNode!.Model.Guid
                };
            }
        }
    }
}