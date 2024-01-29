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
            _guidLabel = this.Q<Label>("title");
            _choicesContainer = this.Q<VisualElement>("button-container");
            _addChoiceButton = this.Q<Button>("add-button");

            _addChoiceButton.clicked += OnAddChoiceClicked;
            _node.Changed += OnModelChanged;
        }

        private void OnAddChoiceClicked()
        {
            var card = new CardControl("test", "test");
            _choicesContainer.Add(card);
            _node.AddChoice("Test");
        }

        private void OnModelChanged()
        {
            
        }
    }
}