using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Localization;

namespace Editor
{
    public class PhraseRepository
    {
        private readonly Dictionary<string, LocalizedString> _content = new();

        private LocalizedTable _localizedTable;
        
        public string CurrentLanguage { get; private set; }
        
        public event Action<string> LanguageChanged;
        
        public void Initialize()
        {
            var table = new CsvAsset("phrases");
            
            _localizedTable = new LocalizedTable(table);
            foreach (var (key, localizedString) in _localizedTable.LoadLocalizedStrings()) 
                _content[key] = localizedString;
            
            ChangeLanguage(_localizedTable.AvailableLanguages().First());
        }

        public void ChangeLanguage(string target)
        {
            CurrentLanguage = target;
            LanguageChanged?.Invoke(target);
        }

        public string[] AvailableLanguages() =>
            _localizedTable.AvailableLanguages();

        public string[] AllKeys() =>
            _content.Keys.ToArray();

        public string Find(string key) =>
            _content[key].Text[CurrentLanguage];
    }
}