using System;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class SwitchNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/ChoicesNodeInspectorView";
        private readonly Button _addCaseButton;
        private readonly VisualElement _casesContainer;
        private readonly Label _guidLabel;

        private readonly SwitchNode _node;
        private readonly ExpressionVerifier _expressionVerifier;

        public SwitchNodeInspectorView(SwitchNode node, ExpressionVerifier expressionVerifier) : base(Uxml)
        {
            _node = node;
            _expressionVerifier = expressionVerifier;
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

        private void OnAddClicked() =>
            _node.AddBranch(new Branch {Guid = Guid.NewGuid().ToString()});

        private CardControl CreateCardControl(Branch branch)
        {
            var card = new CardControl(string.Empty, branch.Condition);
            card.Closed += () => { _node.RemoveBranch(branch.Condition); };
            card.TextEdited += value =>
            {
                VerifyExpression(value);
                branch.Condition = value;
                _node.NotifyChanged();
            };
            _casesContainer.Add(card);
            return card;
        }

        private void VerifyExpression(string expression)
        {
            var (isValid, invalidKeys) = _expressionVerifier.Verify(expression);

            if (isValid)
                return;
            
            if (invalidKeys.Any())
            {
                var error = invalidKeys.Aggregate((origin, text) => $"{origin}, {text}");
                var isOk = EditorUtility.DisplayDialog("Warning", $"Can't find keys: {error}", "Oops");
            }
        }
    }
}