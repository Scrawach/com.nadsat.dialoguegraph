using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DialogueIconViewFactory
    {
        public static DialogueIconView SoundIcon()
        {
            var soundIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/icons8-sound-64.png");
            return new DialogueIconView("Sound", soundIcon);
        }
    }
}