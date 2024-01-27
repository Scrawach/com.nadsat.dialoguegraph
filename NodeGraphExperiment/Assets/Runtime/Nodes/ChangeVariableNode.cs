using System;

namespace Runtime.Nodes
{
    [Serializable]
    public class ChangeVariableNode : BaseDialogueNode
    {
        public string Name;
        public int Value;
    }
}