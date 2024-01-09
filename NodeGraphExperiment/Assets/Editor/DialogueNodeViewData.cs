using UnityEngine;

namespace Editor
{
    public class DialogueNodeViewData
    {
        public string PersonName;
        public string Title;
        public string Description;
        
        public Color HeaderColor;
        
        public Texture2D Icon;
        public Texture2D BackgroundImage;

        public DialogueNodeViewData() { }
        
        public DialogueNodeViewData(DialoguePersonData data)
        {
            PersonName = data.Name;
            HeaderColor = data.Color;
            Icon = data.Icon;
        }
    }
}