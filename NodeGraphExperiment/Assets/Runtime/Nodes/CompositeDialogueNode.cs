using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Runtime.Nodes
{
    public class CompositeDialogueNode : DialogueNode
    {
        private readonly List<DialogueNode> _nodes;

        public CompositeDialogueNode(params DialogueNode[] nodes) =>
            _nodes = new List<DialogueNode>(nodes);

        public IReadOnlyList<DialogueNode> Nodes => _nodes;
        
        public event Action Updated;

        public void Add(DialogueNode node)
        {
            _nodes.Add(node);
            Updated?.Invoke();
        }

        public bool Remove(DialogueNode node)
        {
            var result = _nodes.Remove(node);
            Updated?.Invoke();
            return result;
        }

        public DialogueNode FirstOfDefault(string guid) =>
            _nodes.FirstOrDefault(node => node.Guid == guid);
        
        public void ChangePerson(string personId)
        {
            var previousPerson = FindOrDefault<PersonDialogueNode>();
            previousPerson.PersonId = personId;
            Updated?.Invoke();
        }

        public TNode FindOrDefault<TNode>() where TNode : DialogueNode
        {
            foreach (var node in _nodes)
            {
                if (node is TNode find)
                    return find;
            }

            return null;
        }

        public void InvokeUpdate()
        {
            Updated?.Invoke();
        }
    }
}