using System;
using System.IO;
using Codice.CM.Common;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeView : Node
    {
        public string Guid;

        private Label _personNameLabel;
        private Label _titleLabel;
        private Label _descriptionLabel;

        private VisualElement _header;
        private VisualElement _avatar;

        private VisualElement _image;
        private VisualElement _imageContainer;
        
        private VisualElement _iconContainer;

        public DialogueNodeView(DialogueNodeViewData data) : base(Path.Combine("Assets", "Editor", "DialogueNodeView.uxml"))
        {
            _personNameLabel = this.Q<Label>("person-name-label");
            _titleLabel = this.Q<Label>("title-label");
            _descriptionLabel = this.Q<Label>("description-label");
            _header = this.Q<VisualElement>("header");
            _avatar = this.Q<VisualElement>("avatar");
            _image = this.Q<VisualElement>("image");
            _imageContainer = this.Q<VisualElement>("image-container");
            _iconContainer = this.Q<VisualElement>("icons-container");
            
            _personNameLabel.text = data.PersonName;
            _titleLabel.text = data.Title;
            _descriptionLabel.text = data.Description;
            
            _header.style.backgroundColor = data.BackgroundColor;

            if (data.Icon != null)
            {
                _avatar.style.backgroundImage = new StyleBackground(data.Icon);
            }

            if (data.BackgroundImage != null)
            {
                _image.style.backgroundImage = new StyleBackground(data.BackgroundImage);
            }
            else
            {
                _imageContainer.style.display = DisplayStyle.None;
            }
        }
        
        public event Action<DialogueNodeView> OnNodeSelected;

        public override void OnSelected() =>
            OnNodeSelected?.Invoke(this);
    }
}