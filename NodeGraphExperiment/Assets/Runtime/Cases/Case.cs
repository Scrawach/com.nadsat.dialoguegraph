using System.Collections.Generic;

namespace Runtime.Cases
{
    public class Case
    {
        public CaseType Type;
        public List<string> Conditions;
    }

    public enum CaseType
    {
        Or = 0,
        And = 1
    }
}