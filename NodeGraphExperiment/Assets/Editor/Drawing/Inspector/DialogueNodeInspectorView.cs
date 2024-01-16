using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Controls;
using Editor.Windows.Search;
using Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Inspector
{
    public class DialogueNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/DialogueNodeInspectorView";
        
        private readonly DialogueNode _node;
        private readonly EditorWindow _owner;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;
        private readonly VisualElement _phrasesContainer;
        private readonly Button _addPhraseButton;

        private readonly VisualElement _imagesContainer;
        private readonly Button _addImageButton;

        private readonly SearchWindowProvider _searchWindow;
        private readonly PhraseRepository _phrases;

        private PhraseTextControl _activePhrase;
        private ImageFieldControl _activeImage;
        
        public DialogueNodeInspectorView(DialogueNode node, SearchWindowProvider searchWindow, PhraseRepository phrases)
            : base(Uxml)
        {
            _node = node;
            _searchWindow = searchWindow;
            _phrases = phrases;

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);
            _guidLabel = this.Q<Label>("guid-label");
            _phrasesContainer = this.Q<VisualElement>("phrases-container");
            
            _addPhraseButton = this.Q<Button>("add-phrase-button");
            _addPhraseButton.clicked += OnAddPhraseButtonClicked;

            _imagesContainer = this.Q<VisualElement>("images-container");
            _addImageButton = this.Q<Button>("add-image-button");
            _addImageButton.clicked += OnAddImageButtonClicked;

            _node.Changed += OnNodeUpdated;
            OnNodeUpdated();
        }

        private void OnNodeUpdated() =>
            Draw(_node);

        private void Draw(DialogueNode node)
        {
            _guidLabel.text = node.Guid;
            _dropdownField.SetValueWithoutNotify(node.PersonId);
            
            if (!string.IsNullOrWhiteSpace(node.PhraseId))
                SetPhrase(node.PhraseId);
            
            if (!string.IsNullOrWhiteSpace(node.PathToImage))
                SetImage(node.PathToImage);
        }

        private void SetPhrase(string phraseId)
        {
            if (_activePhrase != null)
                _phrasesContainer.Remove(_activePhrase);
            
            var phrase = _phrases.Get(phraseId);
            var control = new PhraseTextControl(phraseId, phrase);
            
            _activePhrase = control;
            _addPhraseButton.style.display = DisplayStyle.None;
            _phrasesContainer.Add(control);

            control.Closed += () =>
            {
                _activePhrase = null;
                _node.SetPhraseId(string.Empty);
                _phrasesContainer.Remove(control);
                _addPhraseButton.style.display = DisplayStyle.Flex;
            };
        }
        
        private void SetImage(string pathToImage)
        {
            if (_activeImage != null)
                _imagesContainer.Remove(_activeImage);

            var item = new ImageFieldControl();
            _activeImage = item;

            if (!string.IsNullOrWhiteSpace(pathToImage))
            {
                _addImageButton.style.display = DisplayStyle.None;
                item.SetImage(AssetDatabase.LoadAssetAtPath<Sprite>(pathToImage));
            }
            
            _imagesContainer.Add(item);

            item.Closed += () =>
            {
                _activeImage = null;
                _imagesContainer.Remove(item);
                _addImageButton.style.display = DisplayStyle.Flex;
                _node.SetPathToImage(string.Empty);
            };

            item.Selected += (sprite) =>
            {
                _node.SetPathToImage(AssetDatabase.GetAssetPath(sprite));
            };

        }

        private void OnAddImageButtonClicked()
        {
            _addImageButton.style.display = DisplayStyle.None;
            SetImage(string.Empty);
        }
        
        private void OnAddPhraseButtonClicked()
        {
            var point = _addPhraseButton.LocalToWorld(Vector2.zero); 
            _searchWindow.FindPhrase(point, (key) => _node.SetPhraseId(key));
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.SetPersonId(action.newValue);

        public void UpdateDropdownChoices(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}