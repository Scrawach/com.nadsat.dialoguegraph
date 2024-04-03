using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class DialogueNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/DialogueNodeInspectorView";
        private readonly Button _addImageButton;
        private readonly Button _addPhraseButton;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;

        private readonly VisualElement _imagesContainer;

        private readonly DialogueNode _node;
        private readonly EditorWindow _owner;
        private readonly PhraseRepository _phrases;
        private readonly VisualElement _phrasesContainer;

        private ImageFieldControl _activeImage;

        private CardControl _activePhrase;

        public DialogueNodeInspectorView(DialogueNode node, PhraseRepository phrases)
            : base(Uxml)
        {
            _node = node;
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

            if (HasImage(node.BackgroundImage))
                SetImage(node.BackgroundImage);
        }

        private bool HasImage(BackgroundImageData data) => 
            data != null && !string.IsNullOrEmpty(data.PathToImage);

        private void SetPhrase(string phraseId)
        {
            if (_activePhrase != null)
                _phrasesContainer.Remove(_activePhrase);

            var phrase = _phrases.Get(phraseId);
            var control = new CardControl(phraseId, phrase);

            _activePhrase = control;
            _addPhraseButton.style.display = DisplayStyle.None;
            _phrasesContainer.Add(control);

            control.Closed += () =>
            {
                var isOk = EditorUtility.DisplayDialog("Warning", "This action delete phrase from table", "Ok", "Cancel");

                if (!isOk)
                    return;

                _activePhrase = null;
                _node.SetPhraseId(string.Empty);
                _phrasesContainer.Remove(control);
                _phrases.Remove(phraseId);
                _addPhraseButton.style.display = DisplayStyle.Flex;
            };

            control.TextEdited += value =>
            {
                _phrases.Update(phraseId, value);
                _node.NotifyChanged();
            };
        }

        private void SetImage(BackgroundImageData data)
        {
            if (_activeImage != null)
                _imagesContainer.Remove(_activeImage);

            var item = new ImageFieldControl();
            _activeImage = item;

            if (!string.IsNullOrWhiteSpace(data.PathToImage))
            {
                _addImageButton.style.display = DisplayStyle.None;
                item.SetImage(AssetDatabase.LoadAssetAtPath<Sprite>(data.PathToImage));
            }

            _imagesContainer.Add(item);

            item.Closed += () =>
            {
                _activeImage = null;
                _imagesContainer.Remove(item);
                _addImageButton.style.display = DisplayStyle.Flex;
                _node.SetBackgroundImage(data);
            };

            item.Selected += sprite =>
            {
                var pathToSprite = AssetDatabase.GetAssetPath(sprite);

                if (Validate(pathToSprite))
                {
                    _node.BackgroundImage.PathToImage = pathToSprite;
                    _node.NotifyChanged();
                }
                else
                {
                    item.RemoveImage();
                    _node.SetBackgroundImage(null);
                }
            };
        }

        private static bool Validate(string pathToSprite)
        {
            var inResourcesFolder = pathToSprite.Contains("Resources");

            if (!inResourcesFolder)
                EditorUtility.DisplayDialog("Warning", "Sprite should be in Resources/ folder!", "Ok");
            
            return inResourcesFolder;
        }

        public void StartEditPhrase() =>
            _activePhrase?.StartEdit();

        private void OnAddImageButtonClicked()
        {
            _addImageButton.style.display = DisplayStyle.None;
            SetImage(new BackgroundImageData());
        }

        private void OnAddPhraseButtonClicked()
        {
            var phraseId = _phrases.Create(_node.PersonId);
            _node.SetPhraseId(phraseId);
            SetPhrase(phraseId);
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.SetPersonId(action.newValue);

        public void UpdateDropdownChoices(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}