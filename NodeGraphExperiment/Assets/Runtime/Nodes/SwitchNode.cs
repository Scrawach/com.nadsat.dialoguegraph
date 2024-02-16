using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Nodes
{
    [Serializable]
    public class SwitchNode : BaseDialogueNode
    {
        public List<Branch> Branches = new();

        public void RemoveBranch(string condition)
        {
            var branch = Branches.First(b => b.Condition == condition);
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