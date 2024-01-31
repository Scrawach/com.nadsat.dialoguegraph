using System;
using System.Collections.Generic;
using Runtime.Nodes;

namespace Runtime
{
    [Serializable]
    public class DialogueGraph
    {
        public string Name;
        public string EntryNodeGuid;
        public List<DialogueNode> Nodes;
        public List<ChoicesNode> ChoiceNodes;
        public List<SwitchNode> SwitchNodes;
        public List<VariableNode> VariableNodes;
        public List<RedirectNode> RedirectNodes;
        public List<NodeLinks> Links;

        public List<BaseDialogueNode> GetNodes()
        {
            var nodes = new List<BaseDialogueNode>();
            nodes.AddRange(Nodes);
            nodes.AddRange(ChoiceNodes);
            nodes.AddRange(SwitchNodes);
            nodes.AddRange(VariableNodes);
            nodes.AddRange(RedirectNodes);
            return nodes;
        }
    }

}