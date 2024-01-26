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
        public List<NodeLinks> Links;
    }

}