using System.Collections.Generic;

namespace DialogueGraph.Runtime.Localization
{
    public class LocalizedString
    {
        private readonly Dictionary<string, string> _texts;
        
        public LocalizedString() =>
            _texts = new Dictionary<string, string>();

        public string this[string language] => 
            _texts[language];

        public LocalizedString Add(string key, string text)
        {
            _texts.Add(key, text);
            return this;
        }
    }
}