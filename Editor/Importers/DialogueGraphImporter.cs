using System;
using System.Collections.Generic;
using System.IO;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Drawing;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using Nadsat.DialogueGraph.Editor.Factories;
using Nadsat.DialogueGraph.Editor.Factories.NodeListeners;
using Nadsat.DialogueGraph.Editor.Windows.Variables;
using Nadsat.DialogueGraph.Runtime;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Importers
{
    public class DialogueGraphImporter
    {
        private readonly INodeViewFactory _factory;
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;
        private readonly VariablesProvider _variables;
        private readonly CsvImporter _csvImporter;
        private readonly DialogueGraphProvider _dialogueGraphProvider;
        private readonly ElementsFactory _elementsFactory;

        public DialogueGraphImporter(DialogueGraphView graphView, INodeViewFactory factory, NodesProvider nodes, 
            VariablesProvider variables, CsvImporter csvImporter, DialogueGraphProvider dialogueProvider,
            ElementsFactory elementsFactory)
        {
            _graphView = graphView;
            _factory = factory;
            _nodes = nodes;
            _variables = variables;
            _csvImporter = csvImporter;
            _dialogueGraphProvider = dialogueProvider;
            _elementsFactory = elementsFactory;
        }

        public void Import(DialogueGraphContainer container)
        {
            var pathToContainerFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(container));
            container = UnityEngine.Object.Instantiate(container);
            _dialogueGraphProvider.Graph = container.Graph;
            _csvImporter.Import(container.Graph.Name, pathToContainerFolder);
            ImportNodes(container.Graph);
        }

        private void ImportNodes(Runtime.DialogueGraph graph)
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
                _elementsFactory.CreateStickyNote(note);

            foreach (var edge in ConnectNodes(mapping, graph))
                _graphView.AddElement(edge);

            if (TryGetRootNode(graph, mapping, out var nodeView))
            {
                _nodes.RootNode = nodeView;
                nodeView.MarkAsRoot(true);
            }
        }

        private static bool TryGetRootNode(Runtime.DialogueGraph graph, IReadOnlyDictionary<string, Node> mapping, out IModelHandle modelHandle)
        {
            modelHandle = null;

            if (string.IsNullOrWhiteSpace(graph.EntryNodeGuid))
                return false;

            if (mapping[graph.EntryNodeGuid] is not IModelHandle nodeView)
                return false;
            
            modelHandle = nodeView;
            return true;
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

        private static IEnumerable<Edge> ConnectNodes(IReadOnlyDictionary<string, Node> mapping, Runtime.DialogueGraph graph)
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