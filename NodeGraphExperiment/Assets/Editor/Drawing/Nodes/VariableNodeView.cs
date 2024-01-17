using System.Linq;
using Editor.Windows.Variables;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Drawing.Nodes
{
    public class VariableNodeView : Node
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/VariableNodeView.uxml";

        private readonly DropdownField _variablesDropdown;
        private readonly IntegerField _numberField;

        public VariableNodeView(VariablesProvider variables) : base(UxmlPath)
        {
            _variablesDropdown = this.Q<DropdownField>("variables-dropdown");
            _numberField = this.Q<IntegerField>("number-field");

            _variablesDropdown.choices = variables.All().Select(v => v.Name).ToList();
            _variablesDropdown.value = _variablesDropdown.choices.First();
            variables.Changed += () =>
            {
                _variablesDropdown.choices = variables.All().Select(v => v.Name).ToList();
                if (!variables.Contains(_variablesDropdown.value))
                {
                    _variablesDropdown.value = _variablesDropdown.choices.First();
                }
            };
        }

        public void SetVariable(string variableName) =>
            _variablesDropdown.value = variableName;
    }
}