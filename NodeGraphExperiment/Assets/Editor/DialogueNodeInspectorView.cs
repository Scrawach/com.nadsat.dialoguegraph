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
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Button _titleSelect;

        private readonly SearchWindowProvider _searchWindow;
        
        public DialogueNodeInspectorView(DialogueNode node, SearchWindowProvider searchWindow)
        {
            _node = node;
            _searchWindow = searchWindow;
            
            var uiFile = Path.Combine("Assets", "Editor", "DialogueNodeInspectorView.uxml");
            (EditorGUIUtility.Load(uiFile) as VisualTreeAsset)?.CloneTree((VisualElement) this);

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);
            _guidLabel = this.Q<Label>("guid-label");
            _titleLabel = this.Q<Label>("content-title-label");
            _descriptionLabel = this.Q<Label>("content-description-label");
            
            _titleSelect = this.Q<Button>("title-select");
            _titleSelect.clicked += OnTitleSelectClicked;

            _node.Title.Changed += () => Update(node);
            Update(node);
        }

        private void Update(DialogueNode node)
        {
            _dropdownField.SetValueWithoutNotify(node.PersonName.Value);
            _guidLabel.text = node.Guid;
            SetOrHide(_titleLabel, node.Title.Value);
            SetOrHide(_descriptionLabel, node.Description.Value);
        }

        private void SetOrHide(Label target, string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && text != "none")
            {
                target.text = text;
                target.style.display = DisplayStyle.Flex;
            }
            else
            {
                target.style.display = DisplayStyle.None;
            }
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