using Editor.AssetManagement;
using Editor.Drawing.Controls;
using Runtime.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Drawing.Inspector
{
    public class ChoicesNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/ChoicesNodeInspectorView";

        private readonly ChoicesRepository _choices;
        private readonly ChoicesNode _node;

        private readonly Label _guidLabel;
        private readonly VisualElement _choicesContainer;
        private readonly Button _addChoiceButton;
        
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

        private void OnAddChoiceClicked() =>
            _node.AddChoice(_choices.Create());

        private void OnModelChanged()
        {
            _guidLabel.text = _node.Guid;
            
            _choicesContainer.Clear();
            foreach (var button in _node.Choices) 
                CreateCardControl(button, _choices.Get(button));
        }

        private CardControl CreateCardControl(string id, string description)
        {
            var card = new CardControl(id, description);
            card.Closed += () =>
            {
                var isOk = EditorUtility.DisplayDialog("Warning", "This action delete phrase from table", "Ok", "Cancel");
                
                if (!isOk)
                    return;
                
                _node.RemoveChoice(id);
            };
            card.TextEdited += (value) =>
            {
                _choices.Update(id, value);
                _node.NotifyChanged();
            };
            _choicesContainer.Add(card);
            return card;
        }
    }
}