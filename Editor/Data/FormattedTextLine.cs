using System.Text;

namespace Nadsat.DialogueGraph.Editor.Data
{
    public class FormattedTextLine
    {
        private readonly string _originalText;

        private readonly FormatTextPattern[] _patterns = new[]
        {
            new FormatTextPattern("--", "—"),
            new FormatTextPattern("<<", "«"),
            new FormatTextPattern(">>", "»")
        };

        public FormattedTextLine(string originalText) => 
            _originalText = originalText;

        public override string ToString() => 
            Format(_originalText);

        private string Format(string original)
        {
            var result = new StringBuilder();
            var input = new StringBuilder();

            foreach (var symbol in original)
            {
                input.Append(symbol);
                
                if (TryGetPattern(input, out var pattern))
                {
                    if (!pattern.IsCompleted) 
                        continue;
                    
                    result.Append(pattern.NewValue);
                    input.Clear();
                }
                else
                {
                    result.Append(input);
                    input.Clear();
                }
            }
            
            return result.ToString();
        }

        private  bool TryGetPattern(StringBuilder target, out FormatTextPattern pattern)
        {
            pattern = default;
            
            foreach (var textPattern in _patterns)
            {
                if (!textPattern.Contains(target)) 
                    continue;
                
                pattern = textPattern;
                return true;
            }

            return false;
        }
    }
}