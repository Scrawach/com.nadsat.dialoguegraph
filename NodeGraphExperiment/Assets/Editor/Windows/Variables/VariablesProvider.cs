using System.Collections.Generic;
using System.Linq;

namespace Editor.Windows.Variables
{
    public class VariablesProvider
    {
        public readonly List<Variable> GlobalVariables;

        public VariablesProvider() =>
            GlobalVariables = new List<Variable>();

        public void Rename(string oldName, string newName) =>
            GlobalVariables.First(v => v.Name == oldName).Name = newName;

        public bool HasVariableWithName(string name) =>
            GlobalVariables.Any(v => v.Name == name);

        public void Add(Variable variable) =>
            GlobalVariables.Add(variable);

        public void Add(string variableName) =>
            GlobalVariables.Add(new Variable(variableName));

        public void Remove(string variableName) =>
            GlobalVariables.RemoveAll(v => v.Name == variableName);
    }
}