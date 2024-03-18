using System;

namespace Nadsat.DialogueGraph.Runtime.Data
{
    [Serializable]
    public class ChoiceData
    {
        public string ChoiceId;
        public bool IsLocked;
        public string UnlockedCondition;
    }
}