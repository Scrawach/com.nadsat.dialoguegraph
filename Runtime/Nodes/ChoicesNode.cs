using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SetLockedState(string id, bool value)
        {
            FindChoiceDataById(id).IsLocked = value;
            NotifyChanged();
        }

        public void SetUnlockCondition(string id, string value)
        {
            FindChoiceDataById(id).UnlockedCondition = value;
            NotifyChanged();
        }

        private ChoiceData FindChoiceDataById(string choiceId) => 
            Choices.First(choice => choice.ChoiceId == choiceId);
    }
}