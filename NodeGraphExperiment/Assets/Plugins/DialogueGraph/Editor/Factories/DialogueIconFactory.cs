using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using UnityEditor;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class DialogueIconFactory
    {
        public static IconView SoundIcon()
        {
            var soundIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/icons8-sound-64.png");
            return new IconView("Sound", soundIcon);
        }
    }
}