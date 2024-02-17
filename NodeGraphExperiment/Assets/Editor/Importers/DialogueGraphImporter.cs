using System;
using System.Collections.Generic;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Windows.Variables;
using Runtime;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Importers
{
    public class DialogueGraphImporter
    {
        private readonly INodeViewFactory _factory;
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;
        private readonly VariablesProvider _variables;

        public DialogueGraphImporter(DialogueGraphView graphView, INodeViewFactory factory, NodesProvider nodes, VariablesProvider variables)
        {
            _graphView = graphView;
            _factory = factory;
            _nodes = nodes;
            _variables = variables;
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

            foreach (var note in graph.Notes)
            {
                var noteView = new StickyNote(note.Position)
                {
                    title = note.Title,
                    contents = note.Description
                };
                noteView.FitText(true);
                _graphView.AddElement(noteView);
            }

            foreach (var edge in ConnectNodes(mapping, graph))
                _graphView.AddElement(edge);

            if (!string.IsNullOrWhiteSpace(graph.EntryNodeGuid))
            {
                var rootNode = mapping[graph.EntryNodeGuid] as IModelHandle;
                _nodes.RootNode = rootNode;
                rootNode.MarkAsRoot(true);
            }
        }

        private Node CreateFrom<TModel>(TModel model)
            where TModel : BaseDialogueNode =>
            model switch
            {
                DialogueNode dialogue => _factory.CreateDialogue(dialogue),
                ChoicesNode dialogue => _factory.CreateChoices(dialogue),
                SwitchNode dialogue => _factory.CreateSwitch(dialogue),
                VariableNode variableNode => CreateVariable(variableNode),
                RedirectNode dialogue => _factory.CreateRedirect(dialogue),
                AudioEventNode audioNode => _factory.CreateAudioEvent(audioNode),
                _ => throw new ArgumentException()
            };

        private VariableNodeView CreateVariable(VariableNode node)
        {
            _variables.Add(node.Name);
            return _factory.CreateVariable(node);
        }

        private static IEnumerable<Edge> ConnectNodes(IReadOnlyDictionary<string, Node> mapping, DialogueGraph graph)
        {
            foreach (var link in graph.Links)
            {
                var parent = mapping[link.FromGuid];
                var child = mapping[link.ToGuid];
                Port inputPort;
                Port outputPort;

                if (child.inputContainer.childCount > 1)
                    inputPort = FindPort(child.inputContainer, link.ToPortId);
                else
                    inputPort = child.inputContainer[0] as Port;

                if (parent.outputContainer.childCount > 1)
                    outputPort = FindPort(parent.outputContainer, link.FromPortId);
                else
                    outputPort = parent.outputContainer[0] as Port;

                yield return Connect(outputPort, inputPort);
            }
        }

        private static Port FindPort(VisualElement container, string portId)
        {
            foreach (var element in container.Children())
                if (element is Port port && port.viewDataKey == portId)
                    return port;

            throw new ArgumentException($"Not find {portId}");
        }

        private static Edge Connect(Port output, Port input)
        {
            var edge = new Edge
                {output = output, input = input};
            input.Connect(edge);
            output.Connect(edge);
            return edge;
        }
    }
}