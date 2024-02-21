using System;
using System.Linq;
using Editor.AssetManagement;
using Editor.Windows.Variables;

namespace Editor.Data
{
    public class ExpressionVerifier
    {
        private readonly ChoicesRepository _choices;
        private readonly VariablesProvider _variables;

        public ExpressionVerifier(ChoicesRepository choices, VariablesProvider variables)
        {
            _choices = choices;
            _variables = variables;
        }

        public (bool isValid, string[] invalidKeys) Verify(string expression)
        {
            var mathSymbols = new[] {"||", ">", "<", "&&", "|", "&", " "};
            var terms = expression.Split(mathSymbols, StringSplitOptions.RemoveEmptyEntries);
            var invalidKeys = terms.Where(IsInvalidKeys).ToArray();
            return invalidKeys.Any() ? (false, invalidKeys) : (true, null);
        }

        private bool IsInvalidKeys(string key) => 
            IsNotNumber(key) && IsKeyNotFound(key);

        private bool IsKeyNotFound(string key) => 
            !_choices.Contains(key) 
            && !_variables.Contains(key);

        private bool IsNotNumber(string text) => 
            !int.TryParse(text, out _);
    }
}