namespace Runtime.Nodes
{
    public static class CompositeDialogueNodeExtensions
    {
        public static void SetPhrase(this CompositeDialogueNode node, PhraseDialogueNode phrase)
        {
            node.Remove<PhraseDialogueNode>();
            node.Add(phrase);
        }

        public static void SetImage(this CompositeDialogueNode node, ImageDialogueNode image)
        {
            node.Remove<ImageDialogueNode>();
            node.Add(image);
        }

        public static bool Remove<TNode>(this CompositeDialogueNode node) where TNode : DialogueNode
        {
            var first = node.FindOrDefault<TNode>();
            
            if (first != null)
                return node.Remove(first);

            return false;
        }
    }
}