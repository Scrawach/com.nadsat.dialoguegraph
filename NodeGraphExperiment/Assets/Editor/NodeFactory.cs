using UnityEngine;

namespace Editor
{
    public class NodeFactory
    {
        public static DialogueNodeViewData Elena(string title, string description)
        {
            return new DialogueNodeViewData()
            {
                PersonName = "Елена",
                Title = title,
                Description = description,
                BackgroundColor = new Color(0.2f, 0.5f, 0.2f),
                PathToIcon = "Assets/1Helen_neutral.png"
            };
        }

        public static DialogueNodeViewData Mark(string title, string description)
        {
            return new DialogueNodeViewData()
            {
                PersonName = "Марк",
                Title = title,
                Description = description,
                BackgroundColor = new Color(0.2f, 0.5f, 0.6f),
                PathToIcon = "Assets/1Mark-neutral.png"
            };
        }
    }
}