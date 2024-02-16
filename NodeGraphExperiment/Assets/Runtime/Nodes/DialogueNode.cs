using System;

namespace Runtime.Nodes
{
    [Serializable]
    public class DialogueNode : BaseDialogueNode
    {
        public string PersonId;
        public string PhraseId;
        public string PathToImage;

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

        public void SetPathToImage(string pathToImage)
        {
            PathToImage = pathToImage;
            NotifyChanged();
        }
    }
}