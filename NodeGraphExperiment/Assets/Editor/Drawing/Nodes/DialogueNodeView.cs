using System;
using Editor.AssetManagement;
using Runtime;
using UnityEditor;
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
        
        public DialogueNodeView(PhraseRepository phrases, PersonRepository persons) 
            : base(UxmlPath)
        {
            _phrases = phrases;
            _persons = persons;

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

        public void Unbind() =>
            Model.Changed -= OnModelChanged;

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
            SetPerson(model.PersonId);
            SetPhrase(model.PhraseId);
            SetImage(model.PathToImage);
            base.SetPosition(model.Position);
        }

        private void SetPhrase(string phraseId)
        {
            if (string.IsNullOrWhiteSpace(phraseId))
            {
                _phraseTitleLabel.text = "none";
                _phraseTextLabel.text = string.Empty;
            }
            else
            {
                var phrase = _phrases.Get(phraseId);
                _phraseTitleLabel.text = phraseId;
                _phraseTextLabel.text = phrase;
            }
        }

        private void SetPerson(string personId)
        {
            var person = _persons.Get(personId);
            var avatar = AssetDatabase.LoadAssetAtPath<Sprite>(person.PathToIcon);
            _personNameLabel.text = person.Name;
            _header.style.backgroundColor = person.Color;

            if (avatar != null)
            {
                _imageContainer.style.display = DisplayStyle.Flex;
                _avatar.style.backgroundImage = new StyleBackground(avatar);
            }
            else
            {
                _avatar.style.display = DisplayStyle.None;
            }
        }

        private void SetImage(string pathToImage)
        {
            if (string.IsNullOrWhiteSpace(pathToImage))
            {
                _imageContainer.style.display = DisplayStyle.None;
            }
            else
            {
                var image = AssetDatabase.LoadAssetAtPath<Sprite>(pathToImage);
                _imageContainer.style.display = DisplayStyle.Flex;
                _image.style.backgroundImage = new StyleBackground(image);
            }
        }
    }
}