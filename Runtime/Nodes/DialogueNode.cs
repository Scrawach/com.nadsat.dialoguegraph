using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class DialogueNode : BaseDialogueNode
    {
        public string PersonId;
        public string PhraseId;
        public BackgroundImageData BackgroundImage;

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

        public void SetBackgroundImage(BackgroundImageData data)
        {
            BackgroundImage = data;
            NotifyChanged();
        }
    }
}