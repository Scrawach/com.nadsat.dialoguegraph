using System;
using System.Collections.Generic;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Runtime;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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
            
            foreach (var node in graph.GetNodes())
            {
                var view = CreateFrom(node);
                mapping.Add(node.Guid, view);
            }

            foreach (var edge in ConnectNodes(mapping, graph)) 
                _graphView.AddElement(edge);
            
            var rootNode = mapping[graph.EntryNodeGuid] as DialogueNodeView;
            _nodes.RootNode = rootNode;
            rootNode.MarkAsRoot(true);
        }
        
        private Node CreateFrom<TModel>(TModel model) 
            where TModel : BaseDialogueNode =>
            model switch
            {
                DialogueNode dialogue => _factory.CreateDialogue(dialogue),
                ChoicesNode dialogue => _factory.CreateChoices(dialogue),
                SwitchNode dialogue => _factory.CreateSwitch(dialogue),
                VariableNode dialogue => _factory.CreateVariable(dialogue),
                RedirectNode dialogue => _factory.CreateRedirect(dialogue),
                _ => throw new ArgumentException()
            };

        private static IEnumerable<Edge> ConnectNodes(IReadOnlyDictionary<string, Node> mapping, DialogueGraph graph)
        {
            foreach (var link in graph.Links)
            {
                var parent = mapping[link.FromGuid];
                var child = mapping[link.ToGuid];
                Port inputPort;
                Port outputPort;

                if (child.inputContainer.childCount > 1)
                {
                    inputPort = FindPort(child.inputContainer, link.ToPortId);
                }
                else
                {
                    inputPort = child.inputContainer[0] as Port;
                }

                if (parent.outputContainer.childCount > 1)
                {
                    outputPort = FindPort(parent.outputContainer, link.FromPortId);
                }
                else
                {
                    outputPort = parent.outputContainer[0] as Port;
                }
                
                yield return Connect(outputPort, inputPort);
            }
        }

        private static Port FindPort(VisualElement container, string portId)
        {
            foreach (var element in container.Children())
            {
                if (element is Port port && port.viewDataKey == portId)
                    return port;
            }

            throw new ArgumentException($"Not find {portId}");
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