using UnityEditor;
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
                headerColor = new Color(0.2f, 0.5f, 0.2f),
                Icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/1Helen_neutral.png")
            };
        }

        public static DialogueNodeViewData Mark(string title, string description)
        {
            return new DialogueNodeViewData()
            {
                PersonName = "Марк",
                Title = title,
                Description = description,
                headerColor = new Color(0.2f, 0.5f, 0.6f),
                Icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/1Mark-neutral.png")
            };
        }
    }
}