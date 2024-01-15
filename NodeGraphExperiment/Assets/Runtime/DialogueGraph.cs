using System.Collections.Generic;
using Runtime.Nodes;
using Runtime.Serialization;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Dialogue Tree", menuName = "Dialogue Graph/Dialogue Tree", order = 0)]
    public class DialogueGraph : ScriptableObject
    {
        public string EntryNodeGuid;
        
        public List<DialogueNode> _nodes;
        public List<DialogueNodeLink> _links;

        public IReadOnlyList<DialogueNode> Nodes => _nodes;
        public IReadOnlyList<DialogueNodeLink> Links => _links;

        public void Add(DialogueNode node)
        {
            Undo.RecordObject(this, "Add node to graph");
            _nodes.Add(node);
        }
    }
}