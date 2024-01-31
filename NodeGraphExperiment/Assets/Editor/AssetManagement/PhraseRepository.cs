using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Localization;

namespace Editor.AssetManagement
{
    public class PhraseRepository
    {
        private readonly Dictionary<string, TableRepository> _content = new();

        private int _lastIndex;
        
        public string Create(string personId)
        {
            if (!_content.ContainsKey(personId))
                _content[personId] = new TableRepository(personId);
            var repository = _content[personId];
            return repository.Create();
        }

        public CsvTableInfo[] ExportToCsv() =>
            _content.Values
                .Select(table => table.ExportToCsv())
                .ToArray();

        public string Get(string phraseId)
        {
            foreach (var table in Tables(phraseId))
                return table.Get(phraseId);

            return "none";
        }

        public void Update(string phraseId, string content)
        {
            foreach (var table in Tables(phraseId)) 
                table.Update(phraseId, content);
        }

        private IEnumerable<TableRepository> Tables(string phraseId) =>
            _content.Values.Where(table => table.ContainsKey(phraseId));
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