using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Cases
{
    public class Case
    {
        public CaseType Type;
        public List<string> Conditions;
    }

    public class VariableCase
    {
        public string VariableName;
        public Vector2Int Range;
    }

    public enum CaseType
    {
        Or = 0,
        And = 1
    }
}