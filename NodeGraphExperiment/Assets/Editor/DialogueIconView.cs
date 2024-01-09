using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueIconView : VisualElement
    {
        public string Tooltip;
        public Texture2D Icon;

        public DialogueIconView(string tooltip, Texture2D icon)
        {
            Tooltip = tooltip;
            Icon = icon;

            this.Q<VisualElement>().style.backgroundImage = new StyleBackground(icon);
            this.Q<VisualElement>().style.height = 16;
            this.Q<VisualElement>().style.width = 16;
        }
    }
}