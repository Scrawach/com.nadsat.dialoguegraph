using System.Collections.Generic;
using Editor.Drawing;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Runtime;
using UnityEditor.Experimental.GraphView;

namespace Editor.Importers
{
    public class DialogueGraphImporter
    {
        private readonly DialogueGraphView _graphView;
        private readonly INodeViewFactory _factory;
        private readonly NodesProvider _nodes;

        public DialogueGraphImporter(DialogueGraphView graphView, INodeViewFactory factory, NodesProvider nodes)
        {
            _graphView = graphView;
            _factory = factory;
            _nodes = nodes;
        }
        
        public void Import(DialogueGraph graph)
        {
            var mapping = new Dictionary<string, Node>();
            
            foreach (var graphElement in _graphView.graphElements) 
                _graphView.RemoveElement(graphElement);

            if (graph.Nodes == null)
                return;

            foreach (var node in graph.Nodes)
            {
                var view = _factory.CreateDialogue(node);
                mapping.Add(node.Guid, view);
            }

            foreach (var node in graph.RedirectNodes)
            {
                var view = _factory.CreateRedirect(node);
                mapping.Add(node.Guid, view);
            }

            foreach (var edge in ConnectNodes(mapping, graph)) 
                _graphView.AddElement(edge);
            
            var rootNode = _nodes.GetById(graph.EntryNodeGuid);
            _nodes.RootNode = rootNode;
            rootNode.MarkAsRoot(true);
        }

        private static IEnumerable<Edge> ConnectNodes(IReadOnlyDictionary<string, Node> mapping, DialogueGraph graph)
        {
            foreach (var link in graph.Links)
            {
                var parent = mapping[link.FromGuid];
                var child = mapping[link.ToGuid];
                var inputPort = child.inputContainer[0] as Port;
                var outputPort = parent.outputContainer[0] as Port;
                yield return Connect(outputPort, inputPort);
            }
        }
        
        private static Edge Connect(Port output, Port input)
        {
            var edge = new Edge() {output = output, input = input};
            input.Connect(edge);
            output.Connect(edge);
            return edge;
        }
    }
}