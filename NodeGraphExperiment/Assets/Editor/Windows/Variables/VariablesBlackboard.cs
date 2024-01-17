using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows.Variables
{
    public class VariablesBlackboard : Blackboard
    {
        private readonly VariablesProvider _variables;
        private BlackboardSection _globalVariables;
        
        public VariablesBlackboard(VariablesProvider variables, GraphView root) : base(root)
        {
            _variables = variables;
            root.Add(this);
        }

        public void Initialize()
        {
            title = "Variables";
            subTitle = string.Empty;
            SetPosition(new Rect(10, 10, 200, 300));
            addItemRequested += OnAddItemRequested;
            editTextRequested += OnEditTextRequested;
            _globalVariables = new BlackboardSection() {title = "Global Variables"};
            Add(_globalVariables);
            Hide();
        }

        private void OnEditTextRequested(Blackboard blackboard, VisualElement element, string value)
        {
            var field = (BlackboardField) element;
            var oldVariableName = field.text;

            if (_variables.Contains(value))
            {
                EditorUtility.DisplayDialog("Error", "This variable name already exist, please chose another one!", "OK");
                return;
            }

            _variables.Rename(oldVariableName, value);
            field.text = value;
        }

        public void AddVariable(string variableName)
        {
            var variable = new BlackboardField() {text = variableName, typeText = "global"};
            variable.RegisterCallback<ContextualMenuPopulateEvent>(OnBuildMenu);
            _globalVariables.Add(variable);
            _variables.Add(variableName);
        }

        private void OnBuildMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is BlackboardField field)
            {
                evt.menu.AppendAction("Delete", (action) =>
                {
                    RemoveField(field);
                });
            }
        }

        private void RemoveField(BlackboardField field)
        {
            _variables.Remove(field.text);
            _globalVariables.Remove(field);
        }

        private void OnAddItemRequested(Blackboard obj) =>
            AddVariable(GenerateVariableName());

        private string GenerateVariableName()
        {
            var initialName = "New Variable";
            while (_variables.Contains(initialName))
                initialName = $"{initialName}(1)";
            return initialName;
        }

        public void Show() =>
            SetDisplay(DisplayStyle.Flex);

        public void Hide() =>
            SetDisplay(DisplayStyle.None);

        private void SetDisplay(DisplayStyle displayStyle) =>
            this.Q<VisualElement>().style.display = displayStyle;
    }
}