namespace Editor
{
    public class DialogueNode
    {
        public ReactiveProperty<string> PersonName;
        public ReactiveProperty<string> Title;
        public ReactiveProperty<string> Description;

        public DialogueNode()
        {
            PersonName = new ReactiveProperty<string>();
            Title = new ReactiveProperty<string>();
            Description = new ReactiveProperty<string>();
        }
    }
}