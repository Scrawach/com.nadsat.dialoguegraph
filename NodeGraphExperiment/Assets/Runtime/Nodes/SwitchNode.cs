using System;
using System.Collections.Generic;
using Runtime.Cases;

namespace Runtime.Nodes
{
    [Serializable]
    public class SwitchNode : BaseDialogueNode
    {
        public List<Branch> Branches = new();
    }

    public class Branch
    {
        public string ToGuid;
        public Case Case;
    }
}