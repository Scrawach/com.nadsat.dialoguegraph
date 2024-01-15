using System;

namespace DialogueGraph.Runtime
{
    [Serializable]
    public class DialogueNode : BaseDialogueNode
    {
        public bool IsEntryNode;
        
        public string PersonId;
        public string PhraseId;
        public string PathToImage;
        
        public void SetPersonId(string newPersonId)
        {
            PersonId = newPersonId;
            NotifyChanged();
        }

        public void SetPhraseId(string newPhraseId)
        {
            PhraseId = newPhraseId;
            NotifyChanged();
        }

        public void SetPathToImage(string newPathToImage)
        {
            PathToImage = newPathToImage;
            NotifyChanged();
        }

        public void SetEntryNode()
        {
            IsEntryNode = true;
            NotifyChanged();
        }

        public void ResetEntryNode()
        {
            IsEntryNode = false;
            NotifyChanged();
        }
    }
}