using System;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeView : Node
    {
        public string Guid;

        public DialogueNodeView(DialogueNodeViewData data) : base(Path.Combine("Assets", "Editor", "DialogueNodeView.uxml"))
        {
            this.Q<Label>("person-name-label").text = data.PersonName;
            this.Q<Label>("title-label").text = data.Title;
            this.Q<Label>("description-label").text = data.Description;
            
            this.Q<VisualElement>("header").style.backgroundColor = data.BackgroundColor;

            if (!string.IsNullOrWhiteSpace(data.PathToIcon))
            {
                var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(data.PathToIcon);
                this.Q<VisualElement>("avatar").style.backgroundImage = new StyleBackground(icon);
            }

            if (!string.IsNullOrWhiteSpace(data.PathToImage))
            {
                var background = AssetDatabase.LoadAssetAtPath<Texture2D>(data.PathToImage);
                this.Q<VisualElement>("image").style.backgroundImage = new StyleBackground(background);
            }
            else
            {
                this.Q<VisualElement>("image-container").style.display = DisplayStyle.None;
            }

            if (!data.HasSound)
            {
                this.Q<VisualElement>("sound-icon").style.display = DisplayStyle.None;
            }
            else
            {
                this.Q<VisualElement>("sound-icon").style.display = DisplayStyle.Flex;
            }
            
            if (!data.HasError)
            {
                this.Q<VisualElement>("error-icon").style.display = DisplayStyle.None;
            }
            else
            {
                this.Q<VisualElement>("error-icon").style.display = DisplayStyle.Flex;
            }

            this.Q<VisualElement>("node-border").style.visibility = Visibility.Visible;
        }
        
        public event Action<DialogueNodeView> OnNodeSelected;

        public override void OnSelected() =>
            OnNodeSelected?.Invoke(this);
    }
}