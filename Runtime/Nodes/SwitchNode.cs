using System;
using System.Collections.Generic;
using System.Linq;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class SwitchNode : BaseDialogueNode
    {
        public List<Branch> Branches = new();

        public void RemoveBranch(string guid)
        {
            var branch = Branches.First(b => b.Condition == guid);
            Branches.Remove(branch);
            NotifyChanged();
        }

        public void AddBranch(Branch branch)
        {
            Branches.Add(branch);
            NotifyChanged();
        }
    }

    [Serializable]
    public class Branch
    {
        public string Guid;
        public string Condition;
    }
}