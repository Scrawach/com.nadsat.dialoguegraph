using System;
using System.Collections.Generic;
using Nadsat.DialogueGraph.Runtime.Data;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class ChoicesNode : BaseDialogueNode
    {
        public List<ChoiceData> Choices = new();

        public void AddChoice(ChoiceData choice)
        {
            Choices.Add(choice);
            NotifyChanged();
        }

        public void RemoveChoice(ChoiceData choice)
        {
            Choices.Remove(choice);
            NotifyChanged();
        }
    }
}