using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Controls
{
    public class IconView : VisualElement
    {
        public Texture2D Icon;
        public string Tooltip;

        public IconView(string tooltip, Texture2D icon)
        {
            Tooltip = tooltip;
            Icon = icon;

            this.Q<VisualElement>().style.backgroundImage = new StyleBackground(icon);
            this.Q<VisualElement>().style.height = 16;
            this.Q<VisualElement>().style.width = 16;
        }
    }
}