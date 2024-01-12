using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeInspectorView : VisualElement
    {
        private readonly DialogueNode _node;
        private readonly EditorWindow _owner;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;
        private readonly VisualElement _phrasesContainer;
        private readonly Button _addPhraseButton;

        private readonly VisualElement _imagesContainer;
        private readonly Button _addImageButton;

        private readonly SearchWindowProvider _searchWindow;
        
        private PhraseTextItemView _activePhrase;
        private ImageItemView _activeImage;
        
        public DialogueNodeInspectorView(DialogueNode node, SearchWindowProvider searchWindow)
        {
            _node = node;
            _searchWindow = searchWindow;
            
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

            _node.Title.Changed += () => Update(node);
            Update(node);
        }

        private void Update(DialogueNode node)
        {
            _dropdownField.SetValueWithoutNotify(node.PersonName.Value);
            _guidLabel.text = node.Guid;

            if (!string.IsNullOrWhiteSpace(node.Title.Value) && node.Title.Value != "none") 
                AddPhrase(node);

            if (!string.IsNullOrWhiteSpace(node.PathToImage.Value))
                AddImage(node.PathToImage.Value);
        }

        private void AddPhrase(DialogueNode node)
        {
            if (_activePhrase != null)
                _phrasesContainer.Remove(_activePhrase);
            
            var phraseItem = new PhraseTextItemView(node.Title.Value, node.Description.Value);
            _addPhraseButton.style.display = DisplayStyle.None;
            
            phraseItem.Closed += () =>
            {
                node.Title.Value = "none";
                _addPhraseButton.style.display = DisplayStyle.Flex;
                _activePhrase = null;
                _phrasesContainer.Remove(phraseItem);
            };
            _phrasesContainer.Add(phraseItem);
            _activePhrase = phraseItem;
        }

        private void OnAddImageButtonClicked()
        {
            _addImageButton.style.display = DisplayStyle.None;
            AddImage(null);
        }

        private void AddImage(string pathToSprite)
        {
            if (_activeImage != null)
                _imagesContainer.Remove(_activeImage);
            
            var imageItem = new ImageItemView();
            _activeImage = imageItem;

            if (!string.IsNullOrWhiteSpace(pathToSprite))
            {
                _addImageButton.style.display = DisplayStyle.None;
                imageItem.SetImage(AssetDatabase.LoadAssetAtPath<Sprite>(pathToSprite));
            }
            
            _imagesContainer.Add(imageItem);
            
            imageItem.Closed += () =>
            {
                _addImageButton.style.display = DisplayStyle.Flex;
                _imagesContainer.Remove(imageItem);
                _node.PathToImage.Value = null;
                _activeImage = null;
            };

            imageItem.ImageUploaded += (sprite) =>
            {
                _node.PathToImage.Value = AssetDatabase.GetAssetPath(sprite);
            };
        }
        
        private void OnAddPhraseButtonClicked()
        {
            var point = _addPhraseButton.LocalToWorld(Vector2.zero); 
            _searchWindow.FindPhrase(point, (key) => _node.Title.Value = key);
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.PersonName.Value = action.newValue;

        public void Update(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}