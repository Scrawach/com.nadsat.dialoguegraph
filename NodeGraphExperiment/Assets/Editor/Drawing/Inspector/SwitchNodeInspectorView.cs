using System;
using Editor.Drawing.Controls;
using Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Inspector
{
    public class SwitchNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/ChoicesNodeInspectorView";

        private readonly SwitchNode _node;
        private readonly Label _guidLabel;
        private readonly VisualElement _casesContainer;
        private readonly Button _addCaseButton;
        
        public SwitchNodeInspectorView(SwitchNode node) : base(Uxml)
        {
            _node = node;
            _guidLabel = this.Q<Label>("guid-label");
            _casesContainer = this.Q<VisualElement>("button-container");
            _addCaseButton = this.Q<Button>("add-button");
            _addCaseButton.text = "Add Case";

            _addCaseButton.clicked += OnAddClicked;
            _node.Changed += OnNodeChanged;
            OnNodeChanged();
        }

        private void OnNodeChanged()
        {
            _casesContainer.Clear();
            foreach (var branch in _node.Branches) 
                CreateCardControl(branch);
        }

        private void OnAddClicked()
        {
            _node.AddBranch(new Branch { Guid = Guid.NewGuid().ToString() });
        }

        private CardControl CreateCardControl(Branch branch)
        {
            var card = new CardControl(string.Empty, branch.Condition);
            card.Closed += () =>
            {
                _node.RemoveBranch(branch.Condition);
            };
            card.TextEdited += (value) =>
            {
                branch.Condition = value;
                _node.NotifyChanged();
            };
            _casesContainer.Add(card);
            return card;
        }
    }
}