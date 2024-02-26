using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Drawing;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Factories.NodeListeners;
using Nadsat.DialogueGraph.Runtime;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Serialization
{
    public class DialogueGraphSerializer
    {
        private readonly DialogueGraphView _graphView;
        private readonly NodesProvider _nodes;
        private readonly DialogueGraphProvider _dialogueGraphProvider;

        public DialogueGraphSerializer(DialogueGraphView graphView, NodesProvider nodes, DialogueGraphProvider dialogueGraphProvider)
        {
            _graphView = graphView;
            _nodes = nodes;
            _dialogueGraphProvider = dialogueGraphProvider;
        }

        public Runtime.DialogueGraph Serialize()
        {
            var graph = new Runtime.DialogueGraph { Name = _dialogueGraphProvider.Graph.Name };
            FillGraph(graph, _graphView);
            graph.Links = GetLinksFrom(_graphView.edges).ToList();
            _nodes.RootNode ??= _nodes.Nodes.FirstOrDefault();
            graph.EntryNodeGuid = _nodes.RootNode?.Model.Guid;
            return graph;
        }
        
        private static void FillGraph(Runtime.DialogueGraph graph, GraphView view)
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