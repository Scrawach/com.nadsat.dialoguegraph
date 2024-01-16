using System.Collections.Generic;
using Runtime.Nodes;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "DialogueGraphData", menuName = "Dialogue Graph/Create", order = 0)]
    public class DialogueGraph : ScriptableObject
    {
        public string EntryNodeGuid;
        public List<DialogueNode> Nodes;
        public List<NodeLinks> Links;
    }
}