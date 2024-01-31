using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Localization;
using Runtime.Localization;
using UnityEngine;

namespace Editor.Importers
{
    public class CsvImporter
    {
        private readonly MultiTable _table;

        public CsvImporter(MultiTable table) =>
            _table = table;

        public void Import()
        {
            var path = Path.Combine(Application.dataPath, "Resources/Dialogues/Tutor");
            var csvFiles = GetCsvFilesFromDirectory(path).ToArray();
            var csvNames = csvFiles.Select(Path.GetFileNameWithoutExtension).ToArray();
            var csvTexts = csvFiles.Select(s => new CsvText(File.ReadAllText(s))).ToArray();
            _table.ImportFromCsv(csvNames, csvTexts);
        }

        private IEnumerable<string> GetCsvFilesFromDirectory(string path)
        {
            const string tableExtensions = ".csv";
            var filesInDirectory = Directory.GetFiles(path);
            foreach (var filePath in filesInDirectory)
                if (Path.GetExtension(filePath) == tableExtensions)
                    yield return filePath;
        }
    }
}