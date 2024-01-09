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
        private readonly Button _titleSelect;

        private readonly SearchWindowProvider _searchWindow;
        
        public DialogueNodeInspectorView(DialogueNode node, SearchWindowProvider searchWindow)
        {
            _node = node;
            _searchWindow = searchWindow;
            
            var uiFile = Path.Combine("Assets", "Editor", "DialogueNodeInspectorView.uxml");
            (EditorGUIUtility.Load(uiFile) as VisualTreeAsset)?.CloneTree((VisualElement) this);

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.SetValueWithoutNotify(node.PersonName.Value);
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);

            _guidLabel = this.Q<Label>("guid-label");
            _guidLabel.text = node.Guid;

            _titleSelect = this.Q<Button>("title-select");
            _titleSelect.clicked += OnTitleSelectClicked;
        }

        private void OnTitleSelectClicked()
        {
            var point = _titleSelect.LocalToWorld(Vector2.zero); 
            _searchWindow.FindKeys(point, (key) => _node.Title.Value = key);
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.PersonName.Value = action.newValue;

        public void Update(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}