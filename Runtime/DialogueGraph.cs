using System;
using System.Collections.Generic;
using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Runtime
{
    [Serializable]
    public class DialogueGraph
    {
        public string Name;
        public string EntryNodeGuid;
        public List<DialogueNode> Nodes = new();
        public List<ChoicesNode> ChoiceNodes = new();
        public List<SwitchNode> SwitchNodes = new();
        public List<VariableNode> VariableNodes = new();
        public List<RedirectNode> RedirectNodes = new();
        public List<NodeLinks> Links = new();

        public List<NoteNode> Notes = new();

        public List<AudioEventNode> AudioEventNodes = new();

        public List<BaseDialogueNode> GetNodes()
        {
            var nodes = new List<BaseDialogueNode>();
            nodes.AddRange(Nodes);
            nodes.AddRange(ChoiceNodes);
            nodes.AddRange(SwitchNodes);
            nodes.AddRange(VariableNodes);
            nodes.AddRange(RedirectNodes);
            nodes.AddRange(AudioEventNodes);
            return nodes;
        }
    }
}