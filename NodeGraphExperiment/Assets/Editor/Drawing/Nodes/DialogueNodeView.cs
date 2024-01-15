using System;
using Editor.AssetManagement;
using Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Nodes
{
    public class DialogueNodeView : Node
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/DialogueNodeView.uxml";
        private const string UssEntryNode = "entry-node";
        
        private readonly PhraseRepository _phrases;
        private readonly PersonRepository _persons;
        private readonly EditorAssets _assets;
        
        private readonly Label _personNameLabel;
        private readonly Label _phraseTitleLabel;
        private readonly Label _phraseTextLabel;

        private readonly VisualElement _header;
        private readonly VisualElement _avatar;

        private readonly VisualElement _image;
        private readonly VisualElement _imageContainer;
        
        private readonly VisualElement _iconContainer;
        private readonly VisualElement _nodeBorder;

        public DialogueNode Model;
        
        public DialogueNodeView(PhraseRepository phrases, PersonRepository persons, EditorAssets assets) 
            : base(UxmlPath)
        {
            _phrases = phrases;
            _persons = persons;
            _assets = assets;
            
            _personNameLabel = this.Q<Label>("person-name-label");
            _phraseTitleLabel = this.Q<Label>("title-label");
            _phraseTextLabel = this.Q<Label>("description-label");
            _header = this.Q<VisualElement>("header");
            _avatar = this.Q<VisualElement>("avatar");
            _image = this.Q<VisualElement>("image");
            _imageContainer = this.Q<VisualElement>("image-container");
            _iconContainer = this.Q<VisualElement>("icons-container");
            _nodeBorder = this.Q<VisualElement>("node-border");
        }

        public event Action<DialogueNodeView> Selected;
        public event Action<DialogueNodeView> Unselected; 

        public override void OnSelected()
        {
            base.OnSelected();
            Selected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            Unselected?.Invoke(this);
        }

        public void Bind(DialogueNode node)
        {
            Model = node;
            Model.Changed += OnModelChanged;
            OnModelChanged();
        }

        public void Unbind()
        {
            Model.Changed -= OnModelChanged;
            Model = null;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Model.SetPosition(newPos);
        }

        public void MarkAsRoot(bool isRoot)
        {
            if (isRoot)
                _nodeBorder.AddToClassList(UssEntryNode);
            else
                _nodeBorder.RemoveFromClassList(UssEntryNode);
        }
        
        private void OnModelChanged() =>
            Draw(Model);

        private void Draw(DialogueNode model)
        {
            var person = _persons.Get(model.PersonId);
            var phrase = _phrases.Get(model.PhraseId);
            var image = _assets.Load<Sprite>(model.PathToImage);
            var avatar = _assets.Load<Sprite>(person.PathToIcon);
            
            _personNameLabel.text = person.Name;
            _phraseTitleLabel.text = model.PhraseId;
            _phraseTextLabel.text = phrase;
            _header.style.backgroundColor = person.Color;

            if (avatar != null) 
                _avatar.style.backgroundImage = new StyleBackground(avatar);
            else
                _avatar.style.display = DisplayStyle.None;

            if (image != null)
                _image.style.backgroundImage = new StyleBackground(image);
            else
                _imageContainer.style.display = DisplayStyle.None;
            
            if (GetPosition() != model.Position)
                SetPosition(model.Position);
        }
    }
}