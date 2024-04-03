using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class PopupPhraseNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/PopupPhraseNodeInspectorView";
        
        private readonly Label _guidLabel;
        private readonly Button _addPhraseButton;
        private readonly VisualElement _phrasesContainer;

        private readonly PopupPhraseNode _node;
        private readonly PhraseRepository _phrases;

        private CardControl _activePhrase;

        public PopupPhraseNodeInspectorView(PopupPhraseNode node, PhraseRepository phrases) 
            : base(Uxml)
        {
            _node = node;
            _phrases = phrases;
            
            _guidLabel = this.Q<Label>("guid-label");
            _phrasesContainer = this.Q<VisualElement>("phrases-container");

            _addPhraseButton = this.Q<Button>("add-phrase-button");
            _addPhraseButton.clicked += OnAddPhraseButtonClicked;
            
            _node.Changed += OnNodeUpdated;
            OnNodeUpdated();
        }

        private void OnAddPhraseButtonClicked()
        {
            var phraseId = _phrases.Create(PopupPhraseNode.TableName);
            _node.SetPhraseId(phraseId);
            SetPhrase(phraseId);
        }

        private void OnNodeUpdated()
        {
            _guidLabel.text = _node.Guid;
            
            if (!string.IsNullOrWhiteSpace(_node.PhraseId))
                SetPhrase(_node.PhraseId);
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
    }
}