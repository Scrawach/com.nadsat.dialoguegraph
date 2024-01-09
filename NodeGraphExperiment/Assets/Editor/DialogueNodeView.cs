using System;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeView : Node
    {
        private readonly Label _personNameLabel;
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;

        private readonly VisualElement _header;
        private readonly VisualElement _avatar;

        private readonly VisualElement _image;
        private readonly VisualElement _imageContainer;
        
        private readonly VisualElement _iconContainer;

        public DialogueNodeView() : base(Path.Combine("Assets", "Editor", "DialogueNodeView.uxml"))
        {
            _personNameLabel = this.Q<Label>("person-name-label");
            _titleLabel = this.Q<Label>("title-label");
            _descriptionLabel = this.Q<Label>("description-label");
            _header = this.Q<VisualElement>("header");
            _avatar = this.Q<VisualElement>("avatar");
            _image = this.Q<VisualElement>("image");
            _imageContainer = this.Q<VisualElement>("image-container");
            _iconContainer = this.Q<VisualElement>("icons-container");
        }

        public event Action<DialogueNodeView> OnNodeSelected;
        
        public override void OnSelected() =>
            OnNodeSelected?.Invoke(this);
        
        public void Update(DialogueNodeViewData data)
        {
            _personNameLabel.text = data.PersonName;
            _titleLabel.text = data.Title;
            _descriptionLabel.text = data.Description;
            _header.style.backgroundColor = data.headerColor;

            if (data.Icon != null) 
                _avatar.style.backgroundImage = new StyleBackground(data.Icon);

            if (data.BackgroundImage != null)
                _image.style.backgroundImage = new StyleBackground(data.BackgroundImage);
            else
                _imageContainer.style.display = DisplayStyle.None;
        }

        public void AddIcon(DialogueIconView iconView) =>
            _iconContainer.Add(iconView);

        public void RemoveIcon(DialogueIconView iconView) =>
            _iconContainer.Remove(iconView);
    }
}