using System.Collections.Generic;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime.Cases
{
    public class Case
    {
        public List<string> Conditions;
        public CaseType Type;
    }

    public class VariableCase
    {
        public Vector2Int Range;
        public string VariableName;
    }

    public enum CaseType
    {
        Or = 0,
        And = 1
    }
}