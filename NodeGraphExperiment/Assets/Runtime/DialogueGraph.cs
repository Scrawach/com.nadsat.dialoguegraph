using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class DialogueGraph : ScriptableObject
    {
        public string EntryNodeGuid;
        public List<DialogueNode> Nodes;
    }
}