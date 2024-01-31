using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Localization;

namespace Editor.AssetManagement
{
    public class PhraseRepository
    {
        private readonly Dictionary<string, string> _content = new();

        private int _lastIndex;
        
        public string Create(string personId)
        {
            var id = GenerateUniquePhraseId(personId);
            _content[id] = "none";
            _lastIndex++;
            return id;
        }

        public string Get(string phraseId)
        {
            if (_content.ContainsKey(phraseId))
                return _content[phraseId];
            return "none";
        }

        private string GenerateUniquePhraseId(string personId) =>
            $"LVL.{personId}.{_lastIndex:D3}";

        public void Update(string phraseId, string content) =>
            _content[phraseId] = content;
    }
    
    public class PhraseRepositoryOld
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

            var firstLanguageInTable = _localizedTable.GetAvailableLanguages().First();
            ChangeLanguage(firstLanguageInTable);
        }

        public void ChangeLanguage(string target)
        {
            CurrentLanguage = target;
            LanguageChanged?.Invoke(target);
        }

        public string[] AllLanguages() =>
            _localizedTable.GetAvailableLanguages();

        public string[] AllKeys() =>
            _content.Keys.ToArray();
        
        public string Get(string key) =>
            _content[key][CurrentLanguage];
    }
}