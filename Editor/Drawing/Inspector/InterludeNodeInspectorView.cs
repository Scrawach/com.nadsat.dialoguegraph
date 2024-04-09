using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class InterludeNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/InterludeNodeInspectorView";
        private readonly Button _addPhraseButton;
        private readonly DropdownField _dropdownField;
        private readonly DropdownField _emotionsDropdown;
        private readonly Label _guidLabel;

        private readonly InterludeNode _node;
        private readonly EditorWindow _owner;
        private readonly VisualElement _phrasesContainer;

        private ImageFieldControl _activeImage;

        private CardControl _activePhrase;
        
        private readonly PhraseRepository _phrases;
        private readonly DialogueDatabase _database;

        public InterludeNodeInspectorView(InterludeNode node, PhraseRepository phrases, DialogueDatabase dialogueDatabase)
            : base(Uxml)
        {
            _node = node;
            _phrases = phrases;
            _database = dialogueDatabase;

            _dropdownField = this.Q<DropdownField>("person-dropdown");
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);

            _emotionsDropdown = this.Q<DropdownField>("emotion-dropdown");
            _emotionsDropdown.RegisterValueChangedCallback(OnEmotionDropdownChanged);
            
            _guidLabel = this.Q<Label>("guid-label");
            _phrasesContainer = this.Q<VisualElement>("phrases-container");

            _addPhraseButton = this.Q<Button>("add-phrase-button");
            _addPhraseButton.clicked += OnAddPhraseButtonClicked;

            _dropdownField.choices = _database.All().ToList();
            
            _node.Changed += OnNodeUpdated;
            OnNodeUpdated();
        }

        private void OnNodeUpdated() =>
            Draw(_node);

        private void Draw(InterludeNode node)
        {
            _guidLabel.text = node.Guid;
            _dropdownField.SetValueWithoutNotify(node.PersonId);
            UpdateEmotions(node.PersonId, node.Emotion);

            if (!string.IsNullOrWhiteSpace(node.PhraseId))
                SetPhrase(node.PhraseId);
        }

        private void UpdateEmotions(string personId, string selectedEmotion)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                _emotionsDropdown.choices = new List<string>();
                _emotionsDropdown.SetValueWithoutNotify(string.Empty);
                return;
            }

            var availableEmotions = _database.AllEmotions(personId);
            _emotionsDropdown.choices = availableEmotions.ToList();
            _emotionsDropdown.SetValueWithoutNotify(selectedEmotion);
        }

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

        public void StartEditPhrase() =>
            _activePhrase?.StartEdit();

        private void OnAddPhraseButtonClicked()
        {
            if (string.IsNullOrWhiteSpace(_node.PersonId))
            {
                EditorUtility.DisplayDialog("Error", "Select Person!", "Ok");
                return;
            }
            
            var phraseId = _phrases.Create(_node.PersonId);
            _node.SetPhraseId(phraseId);
            SetPhrase(phraseId);
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.SetPersonId(action.newValue);
        
        private void OnEmotionDropdownChanged(ChangeEvent<string> evt) => 
            _node.SetEmotion(evt.newValue);
    }
}