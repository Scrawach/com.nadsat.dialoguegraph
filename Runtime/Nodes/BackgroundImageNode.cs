using System;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class BackgroundImageNode : BaseDialogueNode
    {
        public string PathToImage;
        public float DurationInSeconds;
        public bool IgnoreDuration;

        public void SetBackgroundImage(string pathToImage)
        {
            PathToImage = pathToImage;
            NotifyChanged();
        }
    }
}