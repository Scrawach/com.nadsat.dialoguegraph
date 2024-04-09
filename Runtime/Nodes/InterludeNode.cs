using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class InterludeNode : BaseDialogueNode
    {
        public string PersonId;
        public string PhraseId;
        public string Emotion;

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

        public void SetEmotion(string emotion)
        {
            Emotion = emotion;
            NotifyChanged();
        }
    }
}