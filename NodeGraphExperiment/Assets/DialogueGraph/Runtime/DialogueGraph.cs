using System.Collections.Generic;
using UnityEngine;

namespace DialogueGraph.Runtime
{
    public class DialogueGraph : ScriptableObject
    {
        public string EntryNodeGuid;
        public List<DialogueNode> Nodes;
    }
}