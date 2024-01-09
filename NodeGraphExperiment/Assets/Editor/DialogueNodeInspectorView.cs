using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeInspectorView : VisualElement
    {
        private readonly DialogueNode _node;
        private readonly DropdownField _dropdownField;
        private readonly Label _guidLabel;
        
        public DialogueNodeInspectorView(DialogueNode node)
        {
            _node = node;
            var uiFile = Path.Combine("Assets", "Editor", "DialogueNodeInspectorView.uxml");
            (EditorGUIUtility.Load(uiFile) as VisualTreeAsset)?.CloneTree((VisualElement) this);

            _dropdownField = this.Q<DropdownField>();
            _dropdownField.SetValueWithoutNotify(node.PersonName.Value);
            _dropdownField.RegisterValueChangedCallback(OnDropdownChanged);

            _guidLabel = this.Q<Label>("guid-label");
            _guidLabel.text = node.Guid;
        }

        private void OnDropdownChanged(ChangeEvent<string> action) =>
            _node.PersonName.Value = action.newValue;

        public void Update(IEnumerable<string> dropdownChoices) =>
            _dropdownField.choices = dropdownChoices.ToList();
    }
}