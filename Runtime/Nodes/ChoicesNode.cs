using System;
using System.Collections.Generic;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class ChoicesNode : BaseDialogueNode
    {
        public List<string> Choices = new();

        public void AddChoice(string choiceId)
        {
            Choices.Add(choiceId);
            NotifyChanged();
        }

        public void RemoveChoice(string choiceId)
        {
            Choices.Remove(choiceId);
            NotifyChanged();
        }
    }
}