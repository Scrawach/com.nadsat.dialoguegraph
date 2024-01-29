using Editor.Drawing.Controls;
using Runtime.Nodes;
using UnityEngine.UIElements;

namespace Editor.Drawing.Inspector
{
    public class ChoicesNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/ChoicesNodeInspectorView";

        private readonly ChoicesNode _node;

        private readonly Label _guidLabel;
        private readonly VisualElement _choicesContainer;
        private readonly Button _addChoiceButton;
        
        public ChoicesNodeInspectorView(ChoicesNode node) : base(Uxml)
        {
            _node = node;
            _guidLabel = this.Q<Label>("guid-label");
            _choicesContainer = this.Q<VisualElement>("button-container");
            _addChoiceButton = this.Q<Button>("add-button");

            _addChoiceButton.clicked += OnAddChoiceClicked;
            _node.Changed += OnModelChanged;
            OnModelChanged();
        }

        private void OnAddChoiceClicked() =>
            _node.AddChoice("Test");

        private void OnModelChanged()
        {
            _guidLabel.text = _node.Guid;
            
            _choicesContainer.Clear();
            foreach (var button in _node.Choices) 
                CreateCardControl(button, "Test Description");
        }

        private CardControl CreateCardControl(string id, string description)
        {
            var card = new CardControl(id, description);
            card.Closed += () => _node.RemoveChoice(id);
            _choicesContainer.Add(card);
            return card;
        }
    }
}