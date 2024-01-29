using System;
using System.Collections.Generic;

namespace Runtime.Nodes
{
    [Serializable]
    public class ChoicesNode : BaseDialogueNode
    {
        public List<string> Buttons = new();
        
        public void AddChoice(string buttonId)
        {
            Buttons.Add(buttonId);
            NotifyChanged();
        }
    }
}