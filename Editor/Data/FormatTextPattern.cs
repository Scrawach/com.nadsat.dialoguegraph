using System.Text;

namespace Nadsat.DialogueGraph.Editor.Data
{
    public struct FormatTextPattern
    {
        private readonly string _oldValue;

        public FormatTextPattern(string oldValue, string newValue)
        {
            _oldValue = oldValue;
            NewValue = newValue;
            IsCompleted = false;
        }
        
        public bool IsCompleted { get; private set; }
        
        public string NewValue { get; }

        public bool Contains(StringBuilder symbols)
        {
            int index;
            
            for (index = 0; index < symbols.Length; index++)
            {
                if (_oldValue[index] != symbols[index])
                    return false;
            }

            IsCompleted = index == _oldValue.Length;

            return true;
        }
    }
}