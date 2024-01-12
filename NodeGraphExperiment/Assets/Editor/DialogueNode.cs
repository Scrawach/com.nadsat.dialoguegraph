using System;

namespace Editor
{
    [Serializable]
    public class DialogueNode
    {
        public string Guid;
        
        public readonly ReactiveProperty<string> PersonName;
        public readonly ReactiveProperty<string> Title;
        public readonly ReactiveProperty<string> Description;
        public readonly ReactiveProperty<string> PathToImage;

        public DialogueNode(string personName, string title, string description)
        {
            PersonName = new ReactiveProperty<string>(personName);
            Title = new ReactiveProperty<string>(title);
            Description = new ReactiveProperty<string>(description);
            PathToImage = new ReactiveProperty<string>();
        }
    }
}