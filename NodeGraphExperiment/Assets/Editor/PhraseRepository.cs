using System.Collections.Generic;
using System.Linq;
using Editor.Localization;

namespace Editor
{
    public class PhraseRepository
    {
        private readonly Dictionary<string, LocalizedString> _content = new();
        
        public void Initialize()
        {
            var table = new CsvAsset("phrases");
            var content = new LocalizedTable(table);
            foreach (var (key, localizedString) in content.All()) 
                _content[key] = localizedString;
        }
        
        public string[] AllKeys() =>
            _content.Keys.ToArray();

        public string Find(string key) =>
            _content[key].Text["Russian"];
    }
}