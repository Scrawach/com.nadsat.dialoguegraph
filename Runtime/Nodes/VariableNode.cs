using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class VariableNode : BaseDialogueNode
    {
        public string Name;
        public int Value;

        public void SetName(string newName)
        {
            Name = newName;
            NotifyChanged();
        }

        public void SetValue(int value)
        {
            Value = value;
            NotifyChanged();
        }
    }
}