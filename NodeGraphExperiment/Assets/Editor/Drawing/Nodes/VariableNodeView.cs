using System.Linq;
using Editor.Windows.Variables;
using Runtime.Nodes;
using UnityEngine.UIElements;

namespace Editor.Drawing.Nodes
{
    public class VariableNodeView : BaseNodeView<VariableNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/VariableNodeView.uxml";

        private readonly DropdownField _variablesDropdown;
        private readonly IntegerField _numberField;

        private readonly VariablesProvider _variables;

        public VariableNodeView(VariablesProvider variables) : base(UxmlPath)
        {
            _variables = variables;
            
            _variablesDropdown = this.Q<DropdownField>("variables-dropdown");
            _numberField = this.Q<IntegerField>("number-field");
        }

        public void SetVariable(string variableName) =>
            _variablesDropdown.value = variableName;

        protected override void OnModelChanged()
        {
            _variablesDropdown.choices = _variables.All().Select(v => v.Name).ToList();
            _variablesDropdown.value = Model.Name;
            _variables.Changed += () =>
            {
                _variablesDropdown.choices = _variables.All().Select(v => v.Name).ToList();
                if (!_variables.Contains(_variablesDropdown.value))
                {
                    _variablesDropdown.value = string.Empty;
                }
            };
        }
    }
}