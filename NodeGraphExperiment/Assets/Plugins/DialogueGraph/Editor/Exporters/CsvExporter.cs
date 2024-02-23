using System.IO;
using Nadsat.DialogueGraph.Editor.Localization;
using UnityEditor;

namespace Nadsat.DialogueGraph.Editor.Exporters
{
    public class CsvExporter
    {
        private readonly MultiTable _table;

        public CsvExporter(MultiTable table) => 
            _table = table;

        public void Export(string pathToFolder)
        {
            var csvInfos = _table.ExportToCsv();

            foreach (var csvInfo in csvInfos)
            {
                var pathToFile = Path.Combine(pathToFolder, $"{csvInfo.Name}.csv");
                File.WriteAllText(pathToFile, csvInfo.Content);
            }

            AssetDatabase.Refresh();
        }
    }
}