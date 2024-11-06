using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class DialogueNode : BaseDialogueNode
    {
        public string PersonId;
        public string PhraseId;

        public void SetPersonId(string personId)
        {
            PersonId = personId;
            NotifyChanged();
        }

        public void SetPhraseId(string phraseId)
        {
            PhraseId = phraseId;
            NotifyChanged();
        }
    }
}