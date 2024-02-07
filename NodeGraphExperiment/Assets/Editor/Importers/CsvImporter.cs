using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.AssetManagement;
using Editor.Localization;
using Runtime.Localization;
using UnityEngine;

namespace Editor.Importers
{
    public class CsvImporter
    {
        private readonly LanguageProvider _language;
        private readonly MultiTable _table;
        
        public CsvImporter(LanguageProvider language, MultiTable table)
        {
            _language = language;
            _table = table;
        }

        public void Import(string dialogueName)
        {
            var path = Path.Combine(Application.dataPath, $"Resources/Dialogues/{dialogueName}");
            var csvFiles = GetCsvFilesFromDirectory(path).ToArray();
            var csvNames = csvFiles.Select(Path.GetFileNameWithoutExtension).ToArray();
            var csvTexts = csvFiles.Select(s => new CsvText(File.ReadAllText(s))).ToArray();
            
            if (csvTexts.Length < 1)
                return;

            _table.ImportFromCsv(csvNames, csvTexts);

            var languages = GetLanguagesFromTables(csvTexts).ToArray();
            _table.AddHeaders(languages);
            foreach (var language in languages) 
                _language.AddLanguage(language);
        }

        private IEnumerable<string> GetCsvFilesFromDirectory(string path)
        {
            const string tableExtensions = ".csv";
            var filesInDirectory = Directory.GetFiles(path);
            foreach (var filePath in filesInDirectory)
                if (Path.GetExtension(filePath) == tableExtensions)
                    yield return filePath;
        }

        private static IEnumerable<string> GetLanguagesFromTables(IEnumerable<CsvText> texts)
        {
            var languages = texts
                .Select(t => t.Rows().First().Skip(1).ToArray())
                .OrderBy(array => array.Length);
            return languages.Last();
        }
    }
}