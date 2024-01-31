using System.IO;
using System.Text;
using UnityEngine;
using Environment = System.Environment;

namespace Editor.AssetManagement
{
    public class TableExporter
    {
        private readonly PhraseRepository _phrases;
        private readonly ChoicesRepository _choices;

        public TableExporter(PhraseRepository phrases, ChoicesRepository choices)
        {
            _phrases = phrases;
            _choices = choices;
        }

        public void Export()
        {
            var phraseTables = _phrases.ExportToCsv();
            var choiceTable = _choices.ExportToCsv();
            var path = Path.Combine(Application.dataPath, "Resources/Dialogues/Tutor");
            foreach (var phraseTable in phraseTables) 
                CreateTable(path, phraseTable);
            CreateTable(path, choiceTable);
        }

        public void CreateTable(string path, CsvTableInfo info)
        {
            var builder = new StringBuilder();
            builder.Append(info.Headers);
            builder.Append(Environment.NewLine);
            foreach (var line in info.Lines)
            {
                builder.Append(line);
                builder.Append(Environment.NewLine);
            }

            var pathToFile = Path.Combine(path, $"{info.Name}.csv");
            File.WriteAllText(pathToFile, builder.ToString());
        }
    }
}