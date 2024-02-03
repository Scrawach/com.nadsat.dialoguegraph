using System;
using System.Collections.Generic;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Runtime;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Serialization
{
    public class CopyPasteFactory
    {
        private readonly DialogueGraphView _graphView;
        private readonly INodeViewFactory _factory;

        public CopyPasteFactory(DialogueGraphView graphView, INodeViewFactory factory)
        {
            _graphView = graphView;
            _factory = factory;
        }

        public IEnumerable<GraphElement> Create(CopyPaste.CopiedGraphData graphData)
        {
            var mapping = new Dictionary<string, Node>();
            foreach (var node in graphData.Nodes)
            {
                var oldGuid = node.Guid;
                var newGuid = Guid.NewGuid().ToString();
                node.Guid = newGuid;
                var old = node.Position;
                node.Position = new Vector2Data(old.X + 25, old.Y + 25);
                var nodeView = CreateFrom(node);
                mapping[oldGuid] = nodeView;
                yield return nodeView;
            }
            
            foreach (var edge in ConnectNodes(mapping, graphData.Links)) 
                _graphView.AddElement(edge);
        }
        
        private Node CreateFrom<TModel>(TModel model) 
            where TModel : BaseDialogueNode =>
            model switch
            {
                DialogueNode dialogue => CreateDialogue(dialogue),
                ChoicesNode dialogue => _factory.CreateChoices(dialogue),
                SwitchNode dialogue => _factory.CreateSwitch(dialogue),
                VariableNode dialogue => _factory.CreateVariable(dialogue),
                RedirectNode dialogue => _factory.CreateRedirect(dialogue),
                _ => throw new ArgumentException()
            };

        private DialogueNodeView CreateDialogue(DialogueNode node)
        {
            node.PhraseId = string.Empty;
            return _factory.CreateDialogue(node);
        }

        private static IEnumerable<Edge> ConnectNodes(IReadOnlyDictionary<string, Node> mapping, NodeLinks[] links)
        {
            foreach (var link in links)
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