using System.Collections.Generic;
using System.Linq;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Runtime;
using Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Exporters
{
    public class DialogueGraphExporter
    {
        private readonly DialogueGraphContainer _graph;
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;

        public DialogueGraphExporter(DialogueGraphView graphView, NodesProvider nodes, DialogueGraphContainer graph)
        {
            _graphView = graphView;
            _nodes = nodes;
            _graph = graph;
        }

        public void Export()
        {
            //_graph.Graph.Nodes = _nodes.Nodes.Select(node => node.Model).ToList();
            FillGraph(_graph.Graph, _graphView);
            _graph.Graph.Links = GetLinksFrom(_graphView.edges).ToList();
            //_graph.Graph.RedirectNodes = GetRedirectNodesFrom(_graphView.nodes).ToList();

            if (_nodes.RootNode == null)
                _nodes.RootNode = _nodes.Nodes.FirstOrDefault();

            _graph.Graph.EntryNodeGuid = _nodes.RootNode?.Model.Guid;

            //var jsonExporter = new JsonExporter();
            //jsonExporter.Export("Tutor", _graphView);

            var clone = Object.Instantiate(_graph);
            AssetDatabase.CreateAsset(clone, $"Assets/Resources/Dialogues/{_graph.Graph.Name}/{_graph.Graph.Name}.asset");
            AssetDatabase.SaveAssetIfDirty(clone);
            EditorGUIUtility.PingObject(clone);
        }

        private static void FillGraph(DialogueGraph graph, GraphView view)
        {
            var dialogues = new List<DialogueNode>();
            var choices = new List<ChoicesNode>();
            var switches = new List<SwitchNode>();
            var redirects = new List<RedirectNode>();
            var variables = new List<VariableNode>();
            var stickyNotes = new List<NoteNode>();
            var audioEventNodes = new List<AudioEventNode>();

            foreach (var viewNode in view.graphElements)
                if (viewNode is DialogueNodeView dialogue)
                    dialogues.Add(dialogue.Model);
                else if (viewNode is ChoicesNodeView choice)
                    choices.Add(choice.Model);
                else if (viewNode is SwitchNodeView switchNode)
                    switches.Add(switchNode.Model);
                else if (viewNode is RedirectNodeView redirectNode)
                    redirects.Add(redirectNode.Model);
                else if (viewNode is VariableNodeView variableView)
                    variables.Add(variableView.Model);
                else if (viewNode is AudioEventNodeView audioEventView)
                    audioEventNodes.Add(audioEventView.Model);
                else if (viewNode is StickyNote stickyNote)
                    stickyNotes.Add(new NoteNode
                        {Title = stickyNote.title, Description = stickyNote.contents, Position = stickyNote.GetPosition()});

            graph.Nodes = dialogues;
            graph.ChoiceNodes = choices;
            graph.SwitchNodes = switches;
            graph.RedirectNodes = redirects;
            graph.VariableNodes = variables;
            graph.Notes = stickyNotes;
            graph.AudioEventNodes = audioEventNodes;
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

                yield return new NodeLinks
                {
                    FromGuid = parentNode.Model.Guid,
                    FromPortId = edge.output.viewDataKey,
                    ToGuid = childNode.Model.Guid,
                    ToPortId = edge.input.viewDataKey
                };
            }
        }
    }
}