using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Editor.Windows.Variables
{
    public class VariablesProvider
    {
        private readonly List<Variable> _globalVariables;

        public VariablesProvider() =>
            _globalVariables = new List<Variable>();

        public event Action Changed;

        public IEnumerable<Variable> All() =>
            _globalVariables.ToArray();

        public void Rename(string oldName, string newName)
        {
            _globalVariables.First(v => v.Name == oldName).Name = newName;
            Changed?.Invoke();
        }

        public bool Contains(string variableName) =>
            _globalVariables.Any(v => v.Name == variableName);

        public void Add(Variable variable) =>
            _globalVariables.Add(variable);

        public void Add(string variableName)
        {
            _globalVariables.Add(new Variable(variableName));
            Changed?.Invoke();
        }

        public void Remove(string variableName)
        {
            _globalVariables.RemoveAll(v => v.Name == variableName);
            Changed?.Invoke();
        }
    }
}