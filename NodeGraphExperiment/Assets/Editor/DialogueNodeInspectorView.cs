using System.Collections.Generic;
using System.Linq;
using Runtime.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeInspectorView : VisualElement
    {
        private readonly CompositeDialogueNode _node;
        private readonly EditorWindow _owner;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;
        private readonly VisualElement _phrasesContainer;
        private readonly Button _addPhraseButton;

        private readonly VisualElement _imagesContainer;
        private readonly Button _addImageButton;

        private readonly SearchWindowProvider _searchWindow;
        private readonly PhraseRepository _phrases;

        private PhraseTextItemView _activePhrase;
        private ImageItemView _activeImage;
        
        public DialogueNodeInspectorView(CompositeDialogueNode node, SearchWindowProvider searchWindow, PhraseRepository phrases)
        {
            _node = node;
            _searchWindow = searchWindow;
            _phrases = phrases;

            var uxml = Resources.Load<VisualTreeAsset>("UXML/DialogueNodeInspectorView");
            uxml.CloneTree(this);

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);
            _guidLabel = this.Q<Label>("guid-label");
            _phrasesContainer = this.Q<VisualElement>("phrases-container");
            
            _addPhraseButton = this.Q<Button>("add-phrase-button");
            _addPhraseButton.clicked += OnAddPhraseButtonClicked;

            _imagesContainer = this.Q<VisualElement>("images-container");
            _addImageButton = this.Q<Button>("add-image-button");
            _addImageButton.clicked += OnAddImageButtonClicked;

            _node.Updated += OnNodeUpdated;
            Update(node);
        }

        private void OnNodeUpdated()
        {
            Update(_node);
        }

        private void Update(CompositeDialogueNode node)
        {
            _guidLabel.text = node.Guid;
            
            foreach (var dialogueNode in node.Nodes)
            {
                if (dialogueNode is PersonDialogueNode personNode) 
                    _dropdownField.SetValueWithoutNotify(personNode.PersonId);
                
                if (dialogueNode is PhraseDialogueNode phraseNode && phraseNode.PhraseId != "none")
                    AddPhrase(phraseNode);

                if (dialogueNode is ImageDialogueNode image)
                    AddImage(image);
            }
        }

        private void AddPhrase(PhraseDialogueNode phrase)
        {
            if (_activePhrase != null)
                _phrasesContainer.Remove(_activePhrase);
            var item = new PhraseTextItemView(phrase.PhraseId, _phrases.Find(phrase.PhraseId));
            
            _activePhrase = item;
            _addPhraseButton.style.display = DisplayStyle.None;
            _phrasesContainer.Add(item);

            item.Closed += () =>
            {
                _node.Remove(phrase);
                _activePhrase = null;
                _phrasesContainer.Remove(item);
                _addPhraseButton.style.display = DisplayStyle.Flex;
            };
        }

        private void AddImage(ImageDialogueNode image)
        {
            if (_activeImage != null)
                _imagesContainer.Remove(_activeImage);

            var item = new ImageItemView();
            _activeImage = item;

            if (!string.IsNullOrWhiteSpace(image.PathToImage))
            {
                _addImageButton.style.display = DisplayStyle.None;
                item.SetImage(AssetDatabase.LoadAssetAtPath<Sprite>(image.PathToImage));
            }
            
            _imagesContainer.Add(item);

            item.Closed += () =>
            {
                _activeImage = null;
                _imagesContainer.Remove(item);
                _addImageButton.style.display = DisplayStyle.Flex;
                _node.Remove(image);
            };
            
            item.ImageUploaded += (sprite) =>
            {
                _node.SetImage(new ImageDialogueNode { PathToImage = AssetDatabase.GetAssetPath(sprite)});
            };
        }

        private void OnAddImageButtonClicked()
        {
            _addImageButton.style.display = DisplayStyle.None;
            AddImage(new ImageDialogueNode());
        }
        
        private void OnAddPhraseButtonClicked()
        {
            var point = _addPhraseButton.LocalToWorld(Vector2.zero); 
            _searchWindow.FindPhrase(point, (key) => _node.SetPhrase(new PhraseDialogueNode { PhraseId = key }));
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.ChangePerson(action.newValue);

        public void Update(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}