using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class PhraseTextItemView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PhraseTextItemView, UxmlTraits> { }

        private readonly Label _title;
        private readonly Label _description;
        private readonly Button _closeButton;

        public PhraseTextItemView(string title, string description) : this()
        {
            _title.text = title;
            _description.text = description;
        }
        
        public PhraseTextItemView()
        {
            var uxml = Resources.Load<VisualTreeAsset>("UXML/PhraseTextItem");
            uxml.CloneTree(this);
            _title = this.Q<Label>("title");
            _description = this.Q<Label>("description");
            _closeButton = this.Q<Button>("close-button");
            _title.selection.isSelectable = true;
            _description.selection.isSelectable = true;
        }

        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }
    }
}