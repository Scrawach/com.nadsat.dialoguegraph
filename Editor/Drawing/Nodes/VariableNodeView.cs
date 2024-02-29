using System.Linq;
using Nadsat.DialogueGraph.Editor.Windows.Variables;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class VariableNodeView : BaseNodeView<VariableNode>
    {
        private const string UxmlPath = "UXML/VariableNodeView";
        private readonly IntegerField _numberField;

        private readonly VariablesProvider _variables;

        private readonly DropdownField _variablesDropdown;

        public VariableNodeView(VariablesProvider variables) : base(UxmlPath)
        {
            _variables = variables;

            _variablesDropdown = this.Q<DropdownField>("variables-dropdown");
            _numberField = this.Q<IntegerField>("number-field");

            _variablesDropdown.RegisterValueChangedCallback(OnDropdownChanged);
            _variables.Changed += UpdateDropdownChoices;
            UpdateDropdownChoices();
        }

        private void OnDropdownChanged(ChangeEvent<string> evt) => 
            Model.SetName(evt.newValue);

        protected override void OnModelChanged() => 
            _variablesDropdown.SetValueWithoutNotify(Model.Name);

        private void UpdateDropdownChoices()
        {
            _variablesDropdown.choices = _variables.All().Select(v => v.Name).ToList();
            if (!_variables.Contains(_variablesDropdown.value)) _variablesDropdown.value = string.Empty;
        }
    }
}