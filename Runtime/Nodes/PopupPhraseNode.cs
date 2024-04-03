using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class PopupPhraseNode : BaseDialogueNode
    {
        public const string TableName = "Popup";
        
        public string PhraseId;

        public void SetPhraseId(string phraseId)
        {
            PhraseId = phraseId;
            NotifyChanged();
        }
    }
}