using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Importers;
using Editor.Localization;
using Runtime.Localization;

namespace Editor.AssetManagement
{
    public class PhraseRepository
    {
        private readonly MultiTable _table;

        public PhraseRepository(MultiTable table) =>
            _table = table;
        
        public string Create(string personId) =>
            _table.Create(personId);
        
        public string Get(string phraseId) =>
            _table.Get(phraseId);

        public void Update(string phraseId, string value) =>
            _table.Update(phraseId, value);
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