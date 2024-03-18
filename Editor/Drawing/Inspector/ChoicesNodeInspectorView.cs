using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Data;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class ChoicesNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/ChoicesNodeInspectorView";
        private readonly Button _addChoiceButton;

        private readonly ChoicesRepository _choices;
        private readonly VisualElement _choicesContainer;

        private readonly Label _guidLabel;
        private readonly ChoicesNode _node;

        public ChoicesNodeInspectorView(ChoicesNode node, ChoicesRepository choices) : base(Uxml)
        {
            _node = node;
            _choices = choices;
            _guidLabel = this.Q<Label>("guid-label");
            _choicesContainer = this.Q<VisualElement>("button-container");
            _addChoiceButton = this.Q<Button>("add-button");

            _addChoiceButton.clicked += OnAddChoiceClicked;
            _node.Changed += OnModelChanged;
            OnModelChanged();
        }

        private void OnAddChoiceClicked()
        {
            var choiceData = new ChoiceData { ChoiceId = _choices.Create() };
            _node.AddChoice(choiceData);
        }

        private void OnModelChanged()
        {
            _guidLabel.text = _node.Guid;

            _choicesContainer.Clear();
            foreach (var choice in _node.Choices)
                CreateCardControl(choice, _choices.Get(choice.ChoiceId));
        }

        private CardControl CreateCardControl(ChoiceData data, string description)
        {
            var id = data.ChoiceId;
            var card = new CardControl(id, description);
            card.Closed += () =>
            {
                var isOk = EditorUtility.DisplayDialog("Warning", "This action delete phrase from table", "Ok", "Cancel");

                if (!isOk)
                    return;

                _choices.Remove(id);
                _node.RemoveChoice(data);
            };
            card.TextEdited += value =>
            {
                _choices.Update(id, value);
                _node.NotifyChanged();
            };
            _choicesContainer.Add(card);
            return card;
        }
    }
}