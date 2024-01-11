using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeInspectorView : VisualElement
    {
        private readonly DialogueNode _node;
        private readonly EditorWindow _owner;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;
        private readonly VisualElement _phrasesContainer;
        private readonly Button _titleSelect;

        private readonly SearchWindowProvider _searchWindow;
        private PhraseTextItemView _active;
        
        public DialogueNodeInspectorView(DialogueNode node, SearchWindowProvider searchWindow)
        {
            _node = node;
            _searchWindow = searchWindow;
            
            var uxml = Resources.Load<VisualTreeAsset>("UXML/DialogueNodeInspectorView");
            uxml.CloneTree(this);

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);
            _guidLabel = this.Q<Label>("guid-label");
            _phrasesContainer = this.Q<VisualElement>("phrases-container");
            
            _titleSelect = this.Q<Button>("title-select");
            _titleSelect.clicked += OnTitleSelectClicked;

            _node.Title.Changed += () => Update(node);
            Update(node);
        }

        private void Update(DialogueNode node)
        {
            _dropdownField.SetValueWithoutNotify(node.PersonName.Value);
            _guidLabel.text = node.Guid;

            if (!string.IsNullOrWhiteSpace(node.Title.Value) && node.Title.Value != "none") 
                AddPhrase(node);
        }

        private void AddPhrase(DialogueNode node)
        {
            if (_active != null)
                _phrasesContainer.Remove(_active);
            
            var phraseItem = new PhraseTextItemView(node.Title.Value, node.Description.Value);
            _titleSelect.visible = false;

            phraseItem.Closed += () =>
            {
                node.Title.Value = "none";
                _titleSelect.visible = true;
                _active = null;
                _phrasesContainer.Remove(phraseItem);
            };
            _phrasesContainer.Add(phraseItem);
            _active = phraseItem;
        }


        private void OnTitleSelectClicked()
        {
            var point = _titleSelect.LocalToWorld(Vector2.zero); 
            _searchWindow.FindPhrase(point, (key) => _node.Title.Value = key);
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.PersonName.Value = action.newValue;

        public void Update(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}