using System.Collections.Generic;
using System.Linq;

namespace Editor.Windows.Variables
{
    public class VariablesProvider
    {
        private readonly List<Variable> _globalVariables;

        public VariablesProvider() =>
            _globalVariables = new List<Variable>();

        public void Rename(string oldName, string newName) =>
            _globalVariables.First(v => v.Name == oldName).Name = newName;

        public bool Contains(string variableName) =>
            _globalVariables.Any(v => v.Name == variableName);

        public void Add(Variable variable) =>
            _globalVariables.Add(variable);

        public void Add(string variableName) =>
            _globalVariables.Add(new Variable(variableName));

        public void Remove(string variableName) =>
            _globalVariables.RemoveAll(v => v.Name == variableName);
    }
}