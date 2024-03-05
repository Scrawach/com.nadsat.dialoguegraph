using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class VariableNode : BaseDialogueNode
    {
        public string Name;

        public void SetName(string newName)
        {
            Name = newName;
            NotifyChanged();
        }
    }
}